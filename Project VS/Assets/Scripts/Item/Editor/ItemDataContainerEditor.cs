using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDataContainer))]
public class ItemDataContainerEditor : Editor {
    private static ItemIndex itemIndex = ItemIndex.None;
    
    private SerializedProperty targetData;
    private bool showRawData;
    private Vector2 scrollPos;

    private string[] ItemIndexArray { get; } = Enum.GetNames(typeof(ItemIndex));
    
    public override void OnInspectorGUI() {
        var itemDataContainer = target as ItemDataContainer;
        if (!itemDataContainer) return;
        
        serializedObject.Update();

        var selectedIndex = (ItemIndex)EditorGUILayout.EnumPopup("Item Index", itemIndex);
        using (new EditorGUILayout.HorizontalScope()) {
            if (GUILayout.Button("<<")) {
                selectedIndex = (Enum.GetValues(typeof(ItemIndex)) as ItemIndex[]).First();
            }
            if (GUILayout.Button("<")) {
                var indexArray = Enum.GetValues(typeof(ItemIndex)) as ItemIndex[];
                var prevIdx = (Array.IndexOf(indexArray, selectedIndex) - 1).ClampMin(0);
                selectedIndex = indexArray[prevIdx];
            }
            if (GUILayout.Button(">")) {
                var indexArray = Enum.GetValues(typeof(ItemIndex)) as ItemIndex[];
                var nextIdx = (Array.IndexOf(indexArray, selectedIndex) + 1).ClampMax(indexArray.Length - 1);
                selectedIndex = indexArray[nextIdx];
            }
            if (GUILayout.Button(">>")) {
                selectedIndex = (Enum.GetValues(typeof(ItemIndex)) as ItemIndex[]).Last();
            }
        }
        
        EditorGUILayout.Space(20);
        if (targetData == null || selectedIndex != itemIndex) {
            itemIndex = selectedIndex;
            ResetTargetData();
        }

        if (targetData != null) {
            EditorGUILayout.PropertyField(targetData);
            serializedObject.ApplyModifiedProperties();
        } else if (itemIndex == ItemIndex.None) {
            EditorGUILayout.LabelField("추가하거나 수정할 아이템 데이터를 선택해주세요.");
            EditorGUILayout.LabelField("Value: 무기의 공격력, 패시브 아이템의 보정 수치 등");
            EditorGUILayout.LabelField("Sub Value: 무기의 공격 딜레이(ms)");
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
        targetData = null;
        
        serializedObject.Update();
        var dataContainer = serializedObject.FindProperty("dataContainer");
        for (int i = 0; i < dataContainer.arraySize; i++) {
            var data = dataContainer.GetArrayElementAtIndex(i);
            var dataIndex = data.FindPropertyRelative("itemIndex");
            if (dataIndex.intValue == (int)itemIndex) {
                targetData = data;
                break;
            }
        }
    }
}
