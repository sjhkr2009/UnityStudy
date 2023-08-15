using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSetting))]
public class GameSettingEditor : Editor {
    public override void OnInspectorGUI() {
        var setting = target as GameSetting;
        
        using (new EditorGUILayout.HorizontalScope()) {
            EditorGUILayout.LabelField("경험치 테이블");
            
            var expData = setting!.expByLevel ?? new List<int>();
            if (GUILayout.Button("+")) expData.Add(expData.LastOrDefault());
            if (expData.Count > 0 && GUILayout.Button("-")) expData.RemoveAt(expData.Count - 1);
        }
        
        serializedObject.Update();

        SerializedProperty expByLevelProperty = serializedObject.FindProperty("expByLevel");

        for (int i = 0; i < expByLevelProperty.arraySize; i++) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Level {i + 1} -> {i + 2}", GUILayout.Width(150));
            EditorGUILayout.PropertyField(expByLevelProperty.GetArrayElementAtIndex(i), GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        setting.maxGameTime = EditorGUILayout.FloatField("최대 게임시간 (초)", setting.maxGameTime);
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Save Data")) {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
    }
}
