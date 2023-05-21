using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDataContainer))]
public class ItemDataContainerEditor : OdinEditor {
    private static ItemIndex itemIndex = ItemIndex.None;
    
    private ItemData targetData;
    private int targetLevelIndex;
    
    private bool showRawData;
    private Vector2 scrollPos;

    private ItemIndex[] ItemIndexArray { get; set; }

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
                selectedIndex = (Enum.GetValues(typeof(ItemIndex)) as ItemIndex[]).First();
            }
            if (GUILayout.Button("<")) {
                selectedIndex = prevIndex;
            }
            if (GUILayout.Button(">")) {
                selectedIndex = nextIndex;
            }
            if (GUILayout.Button(">>")) {
                selectedIndex = (Enum.GetValues(typeof(ItemIndex)) as ItemIndex[]).Last();
            }
        }

        using (new EditorGUILayout.HorizontalScope()) {
            bool isFirst = prevIndex == selectedIndex || prevIndex == ItemIndex.None;
            bool isLast = nextIndex == selectedIndex;
            GUILayout.Label($"{(isFirst ? "(None)" : " < " + prevIndex)}");
            GUILayout.Label($"{(isLast ? "(None)" : nextIndex + " > ")}",
                new GUIStyle("label") { alignment = TextAnchor.MiddleRight }
            );
        }
        
        EditorGUILayout.Space(20);
        if (targetData == null || selectedIndex != itemIndex) {
            itemIndex = selectedIndex;
            ResetTargetData();
        }

        if (targetData != null) {
            DrawItemData();
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
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
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
        
        EditorGUILayout.LabelField($"[{targetData.itemIndex}]");
        
        targetData.itemName = EditorGUILayout.TextField("아이템 이름", targetData.itemName);
        targetData.itemType = (ItemType)EditorGUILayout.EnumPopup("타입", targetData.itemType);
        targetData.itemIcon = (Sprite)EditorGUILayout.ObjectField("아이콘", targetData.itemIcon, typeof(Sprite));
        
        targetData.maxLevel = EditorGUILayout.IntField("최고레벨", targetData.maxLevel).Clamp(1, 10);
        SetListCountByMaxLevel();

        using (new EditorGUILayout.HorizontalScope()) {
            DrawItemIndexedValueHeader();
            for (int i = 0; i < targetData.maxLevel; i++) {
                using (new EditorGUILayout.VerticalScope()) {
                    var modified = DrawItemIndexedValueEditor(targetData.indexedValues[i]);
                    targetData.indexedValues[i] = modified;
                }
            }
        }
        
        EditorGUILayout.LabelField("[아이템 설명]");
        using (new EditorGUILayout.HorizontalScope()) {
            for (int i = 0; i < targetData.maxLevel; i++) {
                if (GUILayout.Button($"Level {i + 1}")) targetLevelIndex = i;
            }
        }
        EditorGUILayout.LabelField($"[Level {targetLevelIndex} >> {targetLevelIndex + 1}] 업그레이드 설명");
        targetData.descriptions[targetLevelIndex] = EditorGUILayout.TextArea(targetData.descriptions[targetLevelIndex]);
    }

    void SetListCountByMaxLevel() {
        while (targetData.indexedValues.Count < targetData.maxLevel) {
            targetData.indexedValues.Add(new ItemIndexedValue());
        }
        while (targetData.maxLevel < targetData.indexedValues.Count) {
            targetData.indexedValues.RemoveAt(targetData.indexedValues.Count - 1);
        }
        while (targetData.descriptions.Count < targetData.maxLevel) {
            targetData.descriptions.Add(string.Empty);
        }
        while (targetData.maxLevel < targetData.descriptions.Count) {
            targetData.descriptions.RemoveAt(targetData.descriptions.Count - 1);
        }
    }

    void DrawItemIndexedValueHeader() {
        using (new EditorGUILayout.VerticalScope()) {
            GUILayout.Label("Damage");
            GUILayout.Label("Attack Range");
            GUILayout.Label("Move Speed");
            GUILayout.Label("Attack Interval");
            GUILayout.Label("Attack Count");
            GUILayout.Label("Penetration");
            GUILayout.Label("Object Speed");
        }
    }

    ItemIndexedValue DrawItemIndexedValueEditor(ItemIndexedValue indexedValue) {
        indexedValue.damage = EditorGUILayout.FloatField(indexedValue.damage);
        indexedValue.attackRange = EditorGUILayout.FloatField(indexedValue.attackRange);
        indexedValue.moveSpeed = EditorGUILayout.FloatField(indexedValue.moveSpeed);
        indexedValue.attackInterval = EditorGUILayout.FloatField(indexedValue.attackInterval);
        indexedValue.attackCount = EditorGUILayout.IntField(indexedValue.attackCount);
        indexedValue.penetration = EditorGUILayout.FloatField(indexedValue.penetration);
        indexedValue.objectSpeed = EditorGUILayout.FloatField(indexedValue.objectSpeed);
        return indexedValue;
    }
}
