using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utility {
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ShowIfAttribute : PropertyAttribute {
        public string TargetMember { get; set; }

        public ShowIfAttribute(string targetMember) {
            TargetMember = targetMember;
        }
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawer : PropertyDrawer {
        private float showingHeight = -1f;
        private SerializedProperty targetMember;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            targetMember ??= property.serializedObject.FindProperty((attribute as ShowIfAttribute)?.TargetMember);
            var canShow = targetMember is { propertyType: SerializedPropertyType.Boolean, boolValue: true };
            
            float height = 0;
            if (canShow) {
                EditorGUI.PropertyField(position, property, label, true);
                height = EditorGUI.GetPropertyHeight(property, label, true);
            }

            if (showingHeight < 0) showingHeight = height;
            else showingHeight = Mathf.Lerp(showingHeight, height, 0.1f);

            if (Mathf.Approximately(showingHeight, height)) showingHeight = height;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return Mathf.Max(showingHeight, 0f);
        }
    }
#endif
}