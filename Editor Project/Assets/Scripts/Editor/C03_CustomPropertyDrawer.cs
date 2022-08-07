using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 직렬화가 가능한 타입에 대해 인스펙터에서 그릴 방식을 선택할 수 있다.
// 여기선 직접 만든 클래스로 예를 들었지만 typeof(int) 와 같이 C# 기본 타입들을 포함해 직렬화 가능한 요소라면 무엇이든 오버라이드할 수 있다. (단, 프로젝트 전체에 적용되니 주의)
[CustomPropertyDrawer(typeof(C_ClassExample))]
public class C03_CustomPropertyDrawer : PropertyDrawer {
    const float PropertyHeight = 20f;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // 참고: 여기서는 GUILayout, EditorGUILayout 등 자동 레이아웃 함수를 사용할 수 없다.
        // 매개변수로 이 프로퍼티가 그려지는 위치(position), 변수의 직렬화된 값(property), 이 변수의 이름(label)이 들어온다.
        label.text = "변수 이름: " + label.text;
        
        // 프로퍼티가 그려질 공간에 박스를 하나 그려준다.
        GUI.Box(position, GUIContent.none, GUI.skin.window);
        
        // 변수 이름을 표기한다.
        EditorGUI.PrefixLabel(position, label);

        EditorGUI.indentLevel++;
        
        // 변수 하나당 어느정도 공간을 차지할지 Rect를 만들어둔다. 여기선 임의로 20으로 설정한다.
        var propertyRect = new Rect(position.x,
            position.y + GUIStyle.none.CalcSize(label).y + 5, // 라벨 + 5 만큼 여유공간을 둔다.
            position.width,
            PropertyHeight
        );
        
        // SerializedProperty는 foreach로 순회가 가능하다. SerializedProperty.FindPropertyRelative("변수명") 으로 찾을수도 있다.
        foreach (SerializedProperty prop in property) {
            var name = new GUIContent($"멤버 변수 {prop.name} : ");
            
            // 타입이나 이름으로 어떤 변수인지 확인할 수 있다.
            if (prop.propertyType == SerializedPropertyType.Integer) {
                prop.intValue = EditorGUI.IntSlider(propertyRect, name, prop.intValue, 1, 300);
            } else {
                EditorGUI.PropertyField(propertyRect, prop, name);
            }
            propertyRect.y += PropertyHeight; // 변수 하나 그릴때마다 높이만큼 y 위치값을 이동한다.
        }
        
        
        EditorGUI.indentLevel--;
    }
    
    // 이 프로퍼티가 그려지는 공간의 높이. OnGUI()의 파라미터로 들어오는 position의 높이에 반영된다.
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        var height = EditorGUIUtility.singleLineHeight; // 변수 한칸이 차지하는 크기. 기본값으로 18f 이다.
        height += property.CountRemaining() * PropertyHeight; // 위에서 변수 하나당 20의 공간을 차지하게 했으니 변수당 20만큼 더한다.
        
        //위에서 변수명(라벨)과 내부 요소들 간에 5만큼 여유를 두었으니, 높이 계산에도 반영해준다.
        return height + 5;
    }
}
