using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class B04_GUIStyle : EditorWindow {
    [MenuItem("Custom/02-4 GUI Style", false, 2003)]
    static void Open() {
        GetWindow<B04_GUIStyle>();
    }


    private void OnGUI() {
        // 스타일은 GUI를 그리는 방식을 정의한다.
        var myStyle = new GUIStyle();
        
        myStyle.fontSize = 15;
        myStyle.fontStyle = FontStyle.BoldAndItalic;
        // normal은 기본 상태일 때 그려지는 방식. 버튼과 같이 input을 받는 GUI들은 hover, active 등의 상태를 가질 수 있다.
        myStyle.normal.textColor = Color.green;
        
        // 위에서 지정한 폰트의 크기나 스타일, 색상 등이 적용된다.
        GUILayout.Label("Hello, world!", myStyle);
        
        
        // 유니티에서 제공하는 스타일 프리셋은 EditorStyles 또는 GUI.skin에 정의되어 있다.
        GUILayout.Label("Bold Label!", EditorStyles.boldLabel);
        
        // 다른 스타일을 위한 프리셋도 적용된다. 아래와 같이 입력하면 텍스트 입력 창처럼 그려지지만 눌러보면 버튼처럼 동작한다.
        if (GUILayout.Button("Button?", GUI.skin.textArea)) {
            Debug.Log("Button Clicked!");
        }
        
        // 주의할 점은 GUIStyle은 클래스이므로, EditorStyles.boldLabel과 같은 프리셋을 가져와 변형하면 유니티의 모든 GUI가 그려지는 방식에 영향을 미친다.
        // EditorStyles.label.fontSize = 20 과 같이 입력하면 인스펙터창의 다른 폰트 크기들까지 바뀐다.
        
        // 따라서 프리셋 수정을 원할 경우, GUIStyle 생성자에 원하는 스타일을 입력하여 생성한다. ("button"과 같이 문자열로 입력해도 되지만 오타에 유의)
        myStyle = new GUIStyle(GUI.skin.button); // GUI.skin.button을 deep copy하여 스타일을 생성
        
        // 딥카피했으니 변형시켜도 다른 GUI 스타일은 변하지 않는다.
        myStyle.fontSize = 15;
        myStyle.normal.textColor = Color.magenta;
        myStyle.active.textColor = Color.yellow;
        myStyle.hover = new GUIStyleState() { textColor = Color.cyan };

        GUILayout.Button("My Button", myStyle);
    }
}
