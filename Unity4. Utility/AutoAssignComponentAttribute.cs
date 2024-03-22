using UnityEngine;

/// <summary>
/// 타입이 Component고 필드가 비어 있다면, 자신의 오브젝트에 대해 GetComponent를 할당합니다.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class AutoAssignComponentAttribute : PropertyAttribute {
    /// <param name="findInChild">true일 경우, GetComponent에 실패하면 하위 오브젝트에서 추가로 탐색합니다. 하위 오브젝트는 한 단계만 탐색합니다.</param>
    public AutoAssignComponentAttribute(bool findInChild) {
        FindInChild = findInChild;
    }

    public AutoAssignComponentAttribute() : this(true) { }
    public bool FindInChild { get; private set; }
}

#if UNITY_EDITOR

[UnityEditor.CustomPropertyDrawer(typeof(AutoAssignComponentAttribute))]
public class AutoAssignComponentEditor : UnityEditor.PropertyDrawer {
    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {
        var findInChild = ((AutoAssignComponentAttribute)attribute).FindInChild;
        var fieldType = fieldInfo.FieldType;
        var targetObject = (property.serializedObject?.targetObject as Component)?.gameObject;
        
        if (targetObject && !property.objectReferenceValue && fieldType.IsSubclassOf(typeof(Component))) {
            property.objectReferenceValue = targetObject.GetComponent(fieldType);
            if (findInChild && !property.objectReferenceValue && targetObject.transform.childCount > 0) {
                property.objectReferenceValue = targetObject.transform.GetChild(0)?.GetComponent(fieldType);
            }
        }
        
        UnityEditor.EditorGUI.PropertyField(position, property, label);
    }
}

#endif