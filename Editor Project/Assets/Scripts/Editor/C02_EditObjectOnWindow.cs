using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class C02_EditObjectOnWindow : EditorWindow {
    [MenuItem("Custom/03-2 Edit Object on Window", false, 3000)]
    static void Open() {
        GetWindow<C02_EditObjectOnWindow>();
    }
    
    private Dictionary<SerializedObject, List<SerializedProperty>> Targets { get; } = new();
    private bool isFocused;

    private void OnGUI() {
        if (GUILayout.Button("Refresh")) {
            Refresh();
        }

        if (Targets.Count == 0) {
            EditorGUILayout.LabelField("편집 대상이 없습니다.\n" +
                                       "C_ComponentExample를 사용중인 씬을 열고 Refresh를 눌러주세요.");
            return;
        }
        
        // 각각의 SerializedObject에 대해 인스펙터처럼 PropertyField를 그려서 편집이 가능하게 한다.
        foreach (var pair in Targets) {
            EditorGUI.BeginChangeCheck();

            var obj = pair.Key;
            EditorGUILayout.LabelField(obj.targetObject.name, EditorStyles.boldLabel);
            
            EditorGUI.indentLevel++; // 들여쓰기
            foreach (var property in pair.Value) {
                EditorGUILayout.PropertyField(property);
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            
            // 만약 수정되었다면 원본 오브젝트에 적용한다.
            if (EditorGUI.EndChangeCheck()) obj.ApplyModifiedProperties();
        }
    }
    
    // 씬에서 해당 타입의 컴포넌트를 모두 가져와서 SerializedObject로 직렬화하고, 변수들을 찾아서 저장해 둔다. 
    void Refresh() {
        Targets.Clear();

        var findAll = FindObjectsOfType<C_ComponentExample>();
        if (findAll == null) return;

        foreach (var component in findAll) {
            var obj = new SerializedObject(component);
            var properties = new List<SerializedProperty>() {
                obj.FindProperty(nameof(C_ComponentExample.myObject)),
                obj.FindProperty(nameof(C_ComponentExample.myString)),
                obj.FindProperty("myInt")
            };

            Targets.Add(obj, properties);
        }
    }
    
    // 이 EditorWindow가 포커싱되지 않는 동안은 창을 다시 그리지 않아서, 외부에서 오브젝트 수정 시 창에 반영되지 않는다.
    // 포커싱되지 않은 동안은 Update문에서 직접 SerializedObject를 업데이트하고 창을 다시 그린다.
    private void Update() {
        if (isFocused) return;
        
        foreach (var pair in Targets) {
            pair.Key.Update();
        }
        Repaint();
    }
    
    private void OnFocus() {
        isFocused = true;

        foreach (var pair in Targets) {
            pair.Key.Update();
        }
    }

    private void OnLostFocus() {
        isFocused = false;
    }
}
