using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlyawayPriorityData))]
public class FlyawayPriorityDataEditor : Editor {
    private FlyawayPriorityData flyawayPriorityData;
    private List<EntityMetaData> metaDatas;
    
    private static string[] GroupOptions { get; } = { "1군", "2군", "3군", "4군", "5군", "6군", "7군" };
    private static int[] GroupValues { get; } = { 100, 50, 40, 30, 20, 10, 0 };
    
    private void OnEnable() {
        flyawayPriorityData = (FlyawayPriorityData)target;
        metaDatas = EntityGetter.Get().Values
            .Where(e => e is IFlyawayCalculateTarget)
            .SelectMany(e => (e as IFlyawayCalculateTarget)!.GetMetaDatas())
            .ToList();

        if (flyawayPriorityData.entityPriorities == null || flyawayPriorityData.entityPriorities.Count == 0) {
            flyawayPriorityData.ResetByMetaData(metaDatas);
        }
    }

    public override void OnInspectorGUI() {
        if (GUILayout.Button("Reset All")) {
            if (EditorUtility.DisplayDialog("확인", "모든 데이터를 초기화하시겠습니까?", "ㅇㅇ!", "ㄴㄴㄴㄴ")) {
                flyawayPriorityData.ResetByMetaData(metaDatas);
            }
        }
        
        serializedObject.Update();

        for (int i = 0; i < flyawayPriorityData.entityPriorities.Count; i++) {
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{metaDatas[i].entityName}");
            EditorGUILayout.LabelField($"{metaDatas[i].spriteName}");
            EditorGUILayout.EndHorizontal();

            var entityPriority = flyawayPriorityData.entityPriorities[i];
            
            var index = Array.IndexOf(GroupValues, entityPriority.priorityGroup);
            var selectedGroupIndex = EditorGUILayout.Popup("그룹", index, GroupOptions);
            entityPriority.priorityGroup = GroupValues[Mathf.Clamp(selectedGroupIndex, 0, GroupValues.Length-1)];
            entityPriority.priority = EditorGUILayout.IntField("우선순위", entityPriority.priority);
            entityPriority.isBlocker = EditorGUILayout.Toggle("하위 장치 계산에서 제외", entityPriority.isBlocker);

            EditorGUILayout.Space(5);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

