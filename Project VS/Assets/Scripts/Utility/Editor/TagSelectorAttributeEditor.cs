using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorAttributeEditor : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (property.propertyType != SerializedPropertyType.String) {
            EditorGUI.PropertyField(position, property, label);
            return;
        }
        
        EditorGUI.BeginProperty(position, label, property);
        
        List<string> tagList = new List<string> { "(None)" };
        tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);
        
        string value = property.stringValue;
        int index = 0;
        for (int i = 1; i < tagList.Count; i++) {
            if (tagList[i] == value) {
                index = i;
                break;
            }
        }
        
        
        index = EditorGUI.Popup(position, label.text, index, tagList.ToArray());
        property.stringValue = tagList[index];
            
        EditorGUI.EndProperty();
    }
}
