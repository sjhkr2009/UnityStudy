using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

/// <summary>
/// DiagonalScrollRect.angle 멤버가 인스펙터에 안보여서 만들었음...
/// 겸사겸사 ScrollRect에서 우클릭해서 DiagonalScrollRect로 바꾸는 메뉴도 추가
/// </summary>
[CustomEditor(typeof(DiagonalScrollRect))]
public class DiagonalScrollRectEditor : ScrollRectEditor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        serializedObject.Update();
        
        var angleProp = serializedObject.FindProperty("angle");
        EditorGUILayout.PropertyField(angleProp);
        
        serializedObject.ApplyModifiedProperties();
    }
    
    [MenuItem("CONTEXT/ScrollRect/Replace to DiagonalScrollRect")]
    private static void ScrollRectToDiagonalScrollRect(MenuCommand menuCommand) {
        var existingScrollRect = (ScrollRect)menuCommand.context;
        var go = existingScrollRect.gameObject;

        var viewport = existingScrollRect.viewport;
        var content = existingScrollRect.content;
        var movementType = existingScrollRect.movementType;
        var elasticity = existingScrollRect.elasticity;
        var inertia = existingScrollRect.inertia;
        var decelerationRate = existingScrollRect.decelerationRate;
        var scrollSensitivity = existingScrollRect.scrollSensitivity;
        var verticalScrollbar = existingScrollRect.verticalScrollbar;
        var onValueChanged = existingScrollRect.onValueChanged;

        DestroyImmediate(existingScrollRect);

        var newScrollRect = go.AddComponent<DiagonalScrollRect>();

        newScrollRect.viewport = viewport;
        newScrollRect.content = content;
        newScrollRect.movementType = movementType;
        newScrollRect.elasticity = elasticity;
        newScrollRect.inertia = inertia;
        newScrollRect.decelerationRate = decelerationRate;
        newScrollRect.scrollSensitivity = scrollSensitivity;
        newScrollRect.verticalScrollbar = verticalScrollbar;
        newScrollRect.onValueChanged = onValueChanged;
    }
}