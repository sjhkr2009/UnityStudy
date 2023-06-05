using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDataContainer))]
public class ItemDataContainerEditor : OdinEditor {
    private static ItemIndex itemIndex = ItemIndex.None;
    
    private ItemData targetData;
    private ItemValueType targetDataType;
    
    private bool showRawData;
    private Vector2 normalScrollPos;
    private Vector2 rawDataScrollPos;

    private ItemIndex[] ItemIndexArray { get; set; }

    private GUIStyle MiddleAlignLabel => new GUIStyle("label") { alignment = TextAnchor.MiddleCenter };
    private GUIStyle RightAlignLabel => new GUIStyle("label") { alignment = TextAnchor.MiddleRight };

    public override void OnInspectorGUI() {
        var itemDataContainer = target as ItemDataContainer;
        if (!itemDataContainer) return;

        ItemIndexArray ??= Enum.GetValues(typeof(ItemIndex)) as ItemIndex[];

        serializedObject.Update();
        
        var selectedIndex = (ItemIndex)EditorGUILayout.EnumPopup("Item Index", itemIndex);
        var prevIndex = ItemIndexArray![(Array.IndexOf(ItemIndexArray, selectedIndex) - 1).ClampMin(0)];
        var nextIndex = ItemIndexArray![(Array.IndexOf(ItemIndexArray, selectedIndex) + 1).ClampMax(ItemIndexArray.Length - 1)];
        
        using (new EditorGUILayout.HorizontalScope()) {
            if (GUILayout.Button("<<")) {
                selectedIndex = (Enum.GetValues(typeof(ItemIndex)) as ItemIndex[])!.First();
            }
            if (GUILayout.Button("<")) {
                selectedIndex = prevIndex;
            }
            if (GUILayout.Button(">")) {
                selectedIndex = nextIndex;
            }
            if (GUILayout.Button(">>")) {
                selectedIndex = (Enum.GetValues(typeof(ItemIndex)) as ItemIndex[])!.Last();
            }
        }

        using (new EditorGUILayout.HorizontalScope()) {
            bool isFirst = prevIndex == selectedIndex || prevIndex == ItemIndex.None;
            bool isLast = nextIndex == selectedIndex;
            GUILayout.Label($"{(isFirst ? "(None)" : " < " + prevIndex)}");
            GUILayout.Label($"{(isLast ? "(None)" : nextIndex + " > ")}", RightAlignLabel);
        }
        
        EditorGUILayout.Space(20);
        if (targetData == null || selectedIndex != itemIndex) {
            itemIndex = selectedIndex;
            normalScrollPos = Vector2.zero;
            ResetTargetData();
        }

        if (targetData != null) {
            normalScrollPos = EditorGUILayout.BeginScrollView(normalScrollPos);
            EditorGUILayout.LabelField($"[{targetData.itemIndex}]", MiddleAlignLabel);
            GUILayout.Label(string.Empty, GUI.skin.horizontalSlider);
            GUILayout.Space(20);
            DrawItemData();
            EditorGUILayout.EndScrollView();
        } else if (itemIndex == ItemIndex.None) {
            EditorGUILayout.LabelField("추가하거나 수정할 아이템 데이터를 선택해주세요.");
        } else if (GUILayout.Button("Add Data")) {
            if (itemIndex == ItemIndex.None) {
                EditorUtility.DisplayDialog("경고", "추가할 아이템 유형을 선택해주세요.", "ㅇㅇ");
            } else {
                itemDataContainer.AddData(itemIndex);
                ResetTargetData();
            }
        }

        EditorGUILayout.Space(20);
        
        if (GUILayout.Button("Validate")) {
            serializedObject.ApplyModifiedProperties();
            itemDataContainer.Validate(false);
        }
        if (GUILayout.Button("Save Data")) {
            serializedObject.ApplyModifiedProperties();
            itemDataContainer.Validate(true);
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
        
        EditorGUILayout.Space(20);

        showRawData = EditorGUILayout.Foldout(showRawData, "Show Raw Data");
        if (showRawData) {
            rawDataScrollPos = EditorGUILayout.BeginScrollView(rawDataScrollPos);
            base.OnInspectorGUI();
            EditorGUILayout.EndScrollView();
        }
    }

    void ResetTargetData() {
        var itemDataContainer = (ItemDataContainer)target;
        targetData = itemDataContainer.dataContainer.FirstOrDefault(d => d.itemIndex == itemIndex);
    }

    void DrawItemData() {
        if (targetData == null) return;

        targetData.itemName = EditorGUILayout.TextField("아이템 이름", targetData.itemName);
        targetData.itemType = (ItemType0)EditorGUILayout.EnumPopup("타입", targetData.itemType);
        targetData.itemIcon = (Sprite)EditorGUILayout.ObjectField("아이콘", targetData.itemIcon, typeof(Sprite), false);
        
        EditorGUILayout.Space(10);
        GUILayout.Label(string.Empty, GUI.skin.horizontalSlider);
        EditorGUILayout.Space(20);
        
        DrawDetailValueMetadata(targetData);
        
        GUILayout.Label("Data Type By Level");
        foreach (var detailValue in targetData.detailValues) {
            DrawItemIndexedValueEditor(detailValue);
        }
        
        EditorGUILayout.Space(10);
        GUILayout.Label(string.Empty, GUI.skin.horizontalSlider);
        EditorGUILayout.Space(20);

        DrawItemDescriptions(targetData);
    }

    void DrawDetailValueMetadata(ItemData drawTargetData) {
        drawTargetData.maxLevel = EditorGUILayout.IntField("최고레벨", drawTargetData.maxLevel).Clamp(1, 10);
        SetListCountByMaxLevel(drawTargetData);
        
        EditorGUILayout.LabelField("[세부 수치 설정]");
        targetDataType = (ItemValueType)EditorGUILayout.EnumPopup("Editing...", targetDataType);
        using (new EditorGUILayout.HorizontalScope()) {
            if (GUILayout.Button("Add Data")) {
                if (targetDataType == ItemValueType.Default) {
                    EditorUtility.DisplayDialog("Warning", "추가할 데이터 타입을 선택해주세요.", "ㅇㅇ...");
                } else if (drawTargetData.detailValues.Any(v => v.Type == targetDataType)) {
                    EditorUtility.DisplayDialog("Warning", $"{targetDataType} 데이터가 이미 있습니다!", "ㅇㅇ!");
                } else {
                    var detailValue = ItemDetailValue.Create(targetDataType, drawTargetData.maxLevel);
                    targetData.detailValues.Add(detailValue);
                }
            }
            
            if (GUILayout.Button("Remove Data")) {
                if (targetDataType == ItemValueType.Default) {
                    EditorUtility.DisplayDialog("Warning", "삭제할 데이터 타입을 선택해주세요.", "ㅇㅇ");
                } else if (drawTargetData.detailValues.All(v => v.Type != targetDataType)) {
                    EditorUtility.DisplayDialog("Warning", $"{targetDataType} 데이터가 없습니다.", "ㅇㅇ");
                } else {
                    var deleteTarget = drawTargetData.detailValues.First(v => v.Type == targetDataType); 
                    targetData.detailValues.Remove(deleteTarget);
                }
            }
        }
    }

    void SetListCountByMaxLevel(ItemData drawTargetData) {
        foreach (var detailValue in drawTargetData.detailValues) {
            detailValue.SetValueCount(drawTargetData.maxLevel);
        }
        
        while (drawTargetData.descriptions.Count < drawTargetData.maxLevel) {
            drawTargetData.descriptions.Add(string.Empty);
        }
        while (drawTargetData.maxLevel < drawTargetData.descriptions.Count) {
            drawTargetData.descriptions.RemoveAt(drawTargetData.descriptions.Count - 1);
        }
    }

    void DrawItemIndexedValueEditor(ItemDetailValue detailValue) {
        using (new EditorGUILayout.HorizontalScope()) {
            GUILayout.Label($"[{detailValue.Type}]");
            for (int i = 0; i < targetData.maxLevel; i++) {
                var value = EditorGUILayout.FloatField(detailValue.GetValue(i));
                detailValue.SetValue(i, value);
            }
        }
    }

    void DrawItemDescriptions(ItemData drawTargetData) {
        EditorGUILayout.LabelField("[아이템 설명]");
        
        for (int i = 0; i < drawTargetData.maxLevel; i++) {
            EditorGUILayout.Space(3);
            EditorGUILayout.LabelField($"[Level {i} >> {i + 1}] 업그레이드 설명");
            drawTargetData.descriptions[i] = EditorGUILayout.TextArea(drawTargetData.descriptions[i]);
        }
        
    }
}
