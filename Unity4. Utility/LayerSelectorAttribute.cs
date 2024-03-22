using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/** string 필드에 Sorting Layer를 입력할 때 오타 없이 드롭다운으로 선택하게 해 주는 Attribute */
public class LayerSelectorAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LayerSelectorAttribute))]
public class LayerSelectorAttributeEditor : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (property.propertyType != SerializedPropertyType.String) {
            EditorGUI.PropertyField(position, property, label);
            return;
        }
        
        EditorGUI.BeginProperty(position, label, property);
        
        List<string> layerList = SortingLayer.layers.Select(l => l.name).ToList();
        
        string value = property.stringValue;
        int index = 0;
        for (int i = 0; i < layerList.Count; i++) {
            if (layerList[i] == value) {
                index = i;
                break;
            }
        }
        
        
        index = EditorGUI.Popup(position, label.text, index, layerList.ToArray());
        property.stringValue = layerList[index];
            
        EditorGUI.EndProperty();
    }
}
#endif