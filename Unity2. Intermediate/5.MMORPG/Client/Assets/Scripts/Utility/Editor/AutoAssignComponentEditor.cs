using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AutoAssignComponentAttribute))]
public class AutoAssignComponentEditor : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // 타입이 Component고 필드가 비어 있다면, 자신의 오브젝트에 대해 GetComponent를 할당합니다.
        var fieldType = fieldInfo.FieldType;
        if (property.objectReferenceValue == null && fieldType.IsSubclassOf(typeof(Component))) {
            property.objectReferenceValue = ((Component)property.serializedObject.targetObject).GetComponent(fieldType);
        }
        
        EditorGUI.PropertyField(position, property, label);
    }
}