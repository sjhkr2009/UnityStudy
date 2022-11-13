using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorHolder : EditorWindow {
    [MenuItem("Custom/EditorHolder", false, 30000)]
    static void Open() {
        GetWindow<EditorHolder>();
    }
    
    private int _selectionValue = 0;

    private int SelectionValue {
        get => _selectionValue;
        set => _selectionValue = Mathf.Clamp(value, 0, tabName.Length - 1);
    }
    private string[] tabName = new[] { "A-01", "A-02", "B-01", "B-02", "B-03" };
    private Dictionary<int, ScriptableObject> cachedTargets = new Dictionary<int, ScriptableObject>();

    private void OnGUI() {
        // 참고: 여러 줄을 가진 툴바는 GUILayout.SelectionGrid를 사용
        SelectionValue = GUILayout.Toolbar(SelectionValue, tabName);

        if (!cachedTargets.TryGetValue(SelectionValue, out var target)) {
            target = Resources.Load<ScriptableObject>($"EventManager{SelectionValue + 1}");
            cachedTargets[SelectionValue] = target;
        }

        var editor = Editor.CreateEditor(target);
        editor.OnInspectorGUI();
    }
}
