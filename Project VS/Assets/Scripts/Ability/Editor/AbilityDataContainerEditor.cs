using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityDataContainer))]
public class AbilityDataContainerEditor : Editor {
    private static AbilityIndex _abilityIndex = AbilityIndex.None;
    
    private AbilityData targetData;
    private AbilityValueType targetDataType;
    
    private bool showRawData;
    private bool showMultiIcon;
    private Vector2 normalScrollPos;
    private Vector2 rawDataScrollPos;

    private AbilityIndex[] ItemIndexArray { get; set; }

    private GUIStyle MiddleAlignLabel => new GUIStyle("label") { alignment = TextAnchor.MiddleCenter };
    private GUIStyle RightAlignLabel => new GUIStyle("label") { alignment = TextAnchor.MiddleRight };

    private const float DetailHeaderWidth = 150f;
    private const float DetailFieldWidth = 50f;

    public override void OnInspectorGUI() {
        var itemDataContainer = target as AbilityDataContainer;
        if (!itemDataContainer) return;
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(itemDataContainer), typeof(MonoScript), true);
        EditorGUI.EndDisabledGroup();

        ItemIndexArray ??= Enum.GetValues(typeof(AbilityIndex)) as AbilityIndex[];

        serializedObject.Update();

        var selectedIndex = (AbilityIndex)EditorGUILayout.EnumPopup("Item Index", _abilityIndex);
        var prevIndex = ItemIndexArray![(Array.IndexOf(ItemIndexArray, selectedIndex) - 1).ClampMin(0)];
        var nextIndex = ItemIndexArray![(Array.IndexOf(ItemIndexArray, selectedIndex) + 1).ClampMax(ItemIndexArray.Length - 1)];
        
        using (new EditorGUILayout.HorizontalScope()) {
            if (GUILayout.Button("<<")) {
                selectedIndex = (Enum.GetValues(typeof(AbilityIndex)) as AbilityIndex[])!.First();
            }
            if (GUILayout.Button("<")) {
                selectedIndex = prevIndex;
            }
            if (GUILayout.Button(">")) {
                selectedIndex = nextIndex;
            }
            if (GUILayout.Button(">>")) {
                selectedIndex = (Enum.GetValues(typeof(AbilityIndex)) as AbilityIndex[])!.Last();
            }
        }

        using (new EditorGUILayout.HorizontalScope()) {
            bool isFirst = prevIndex == selectedIndex || prevIndex == AbilityIndex.None;
            bool isLast = nextIndex == selectedIndex;
            GUILayout.Label($"{(isFirst ? "(None)" : " < " + prevIndex)}");
            GUILayout.Label($"{(isLast ? "(None)" : nextIndex + " > ")}", RightAlignLabel);
        }
        
        EditorGUILayout.Space(20);
        if (targetData == null || selectedIndex != _abilityIndex) {
            _abilityIndex = selectedIndex;
            normalScrollPos = Vector2.zero;
            ResetTargetData();
        }

        if (targetData != null) {
            normalScrollPos = EditorGUILayout.BeginScrollView(normalScrollPos);
            EditorGUILayout.LabelField($"[{targetData.abilityIndex}]", MiddleAlignLabel);
            GUILayout.Label(string.Empty, GUI.skin.horizontalSlider);
            GUILayout.Space(20);
            DrawItemData();
            EditorGUILayout.EndScrollView();
            
            showMultiIcon = EditorGUILayout.Foldout(showMultiIcon, "레벨별 아이콘 설정하기");
            if (showMultiIcon) DrawIconByLevel(targetData);
            
        } else if (_abilityIndex == AbilityIndex.None) {
            EditorGUILayout.LabelField("추가하거나 수정할 아이템 데이터를 선택해주세요.");
        } else if (GUILayout.Button("Add Data")) {
            if (_abilityIndex == AbilityIndex.None) {
                EditorUtility.DisplayDialog("경고", "추가할 아이템 유형을 선택해주세요.", "ㅇㅇ");
            } else {
                itemDataContainer.AddData(_abilityIndex);
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
        var itemDataContainer = (AbilityDataContainer)target;
        targetData = itemDataContainer.dataContainer.FirstOrDefault(d => d.abilityIndex == _abilityIndex);
    }

    void DrawItemData() {
        if (targetData == null) return;

        targetData.itemName = EditorGUILayout.TextField("아이템 이름", targetData.itemName);
        targetData.itemIcon = (Sprite)EditorGUILayout.ObjectField("아이콘", targetData.itemIcon, typeof(Sprite), false);

        EditorGUILayout.Space(10);
        GUILayout.Label(string.Empty, GUI.skin.horizontalSlider);
        EditorGUILayout.Space(20);
        
        DrawDetailValueMetadata(targetData);
        
        EditorGUILayout.Space();
        using (new EditorGUILayout.HorizontalScope()) {
            GUILayout.Label($"[데이터 타입]", GUILayout.Width(DetailHeaderWidth));
            for (int i = 0; i < targetData.maxLevel; i++) {
                EditorGUILayout.LabelField($"Lv.{i + 1}", GUILayout.Width(DetailFieldWidth));
            }
        }
        foreach (var detailValue in targetData.detailValues) {
            DrawItemIndexedValueEditor(detailValue);
        }
        
        EditorGUILayout.Space(10);
        GUILayout.Label(string.Empty, GUI.skin.horizontalSlider);
        EditorGUILayout.Space(20);

        DrawItemDescriptions(targetData);
    }

    void DrawDetailValueMetadata(AbilityData drawTargetData) {
        using (new EditorGUILayout.HorizontalScope()) {
            EditorGUILayout.LabelField($"최고레벨: {drawTargetData.maxLevel}", GUILayout.Width(150));
            if (GUILayout.Button("+", GUILayout.Width(40))) drawTargetData.maxLevel++;
            if (GUILayout.Button("-", GUILayout.Width(40))) drawTargetData.maxLevel--;
            drawTargetData.maxLevel = drawTargetData.maxLevel.Clamp(1, 10);
        }
        
        SetListCountByMaxLevel(drawTargetData);
        
        EditorGUILayout.LabelField("[세부 수치 설정]");
        targetDataType = (AbilityValueType)EditorGUILayout.EnumPopup("    타입 추가/삭제 >>", targetDataType);
        using (new EditorGUILayout.HorizontalScope()) {
            if (GUILayout.Button("Add Data")) {
                if (targetDataType == AbilityValueType.Default) {
                    EditorUtility.DisplayDialog("Warning", "추가할 데이터 타입을 선택해주세요.", "ㅇㅇ...");
                } else if (drawTargetData.detailValues.Any(v => v.Type == targetDataType)) {
                    EditorUtility.DisplayDialog("Warning", $"{targetDataType} 데이터가 이미 있습니다!", "ㅇㅇ!");
                } else {
                    var detailValue = AbilityDetailValue.Create(targetDataType, drawTargetData.maxLevel);
                    targetData.detailValues.Add(detailValue);
                }
            }
            
            if (GUILayout.Button("Remove Data")) {
                if (targetDataType == AbilityValueType.Default) {
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
    
    void SetListCountByMaxLevel(AbilityData drawTargetData) {
        foreach (var detailValue in drawTargetData.detailValues) {
            detailValue.SetValueCount(drawTargetData.maxLevel);
        }
        
        while (drawTargetData.descriptions.Count < drawTargetData.maxLevel) {
            drawTargetData.descriptions.Add(string.Empty);
        }
        while (drawTargetData.maxLevel < drawTargetData.descriptions.Count) {
            drawTargetData.descriptions.RemoveAt(drawTargetData.descriptions.Count - 1);
        }
        
        while (drawTargetData.iconsByLevel.Count < drawTargetData.maxLevel) {
            drawTargetData.iconsByLevel.Add(drawTargetData.itemIcon);
        }
        while (drawTargetData.maxLevel < drawTargetData.iconsByLevel.Count) {
            drawTargetData.iconsByLevel.RemoveAt(drawTargetData.iconsByLevel.Count - 1);
        }
    }

    void DrawItemIndexedValueEditor(AbilityDetailValue detailValue) {
        using (new EditorGUILayout.HorizontalScope()) {
            GUILayout.Label($"[{detailValue.Type}]", GUILayout.Width(DetailHeaderWidth));
            for (int i = 0; i < targetData.maxLevel; i++) {
                var value = EditorGUILayout.FloatField(detailValue.GetValue(i), GUILayout.Width(DetailFieldWidth));
                detailValue.SetValue(i, value);
            }
        }
    }

    void DrawItemDescriptions(AbilityData drawTargetData) {
        EditorGUILayout.LabelField("[아이템 설명]");
        
        for (int i = 0; i < drawTargetData.maxLevel; i++) {
            EditorGUILayout.Space(3);
            EditorGUILayout.LabelField($"[Level {i} >> {i + 1}] 업그레이드 설명");
            drawTargetData.descriptions[i] = EditorGUILayout.TextArea(drawTargetData.descriptions[i]);
        }
        
    }
    
    void DrawIconByLevel(AbilityData drawTargetData) {
        EditorGUILayout.LabelField("[레벨별 아이콘]");
        using (new EditorGUILayout.HorizontalScope()) {
            for (int i = 0; i < drawTargetData.maxLevel; i++) {
                drawTargetData.iconsByLevel[i] = (Sprite)EditorGUILayout.ObjectField(drawTargetData.iconsByLevel[i], typeof(Sprite), false);
                if (drawTargetData.iconsByLevel[i] == null) drawTargetData.iconsByLevel[i] = drawTargetData.itemIcon;
            }
        }
    }
}
