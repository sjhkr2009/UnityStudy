using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponDataContainer))]
public class WeaponDataContainerEditor : Editor {
    public override void OnInspectorGUI() {
        var container = target as WeaponDataContainer;
        
        serializedObject.Update();
        var datas = serializedObject.FindProperty("data");
        for (int i = 0; i < datas.arraySize; i++) {
            var data = datas.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(data);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
