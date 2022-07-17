using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class A01_BasicEditorWindow : EditorWindow {
    // MenuItem으로 상단바에 커스텀 항목을 추가한다.
    // 단축키 지정이 가능하다.
    /*
     * % : Ctrl
     * & : Alt
     * # : Shift
     * 숫자나 문자, F1~F12 지정 가능
     */
    [MenuItem("Custom/01-1 Basic %&#f1")]
    static void OpenWindow() {
        // static 함수인 EditorWindow.GetWindow를 사용해서 창을 띄울 수 있다.
        GetWindow<A01_BasicEditorWindow>("ㅎㅎ");
    }
    
    // 에디터에서 매 프레임 호출되는 함수
    private void OnGUI() {
        // 에디터에서만 쓰일 경우 EditorGUI, 에디터뿐 아니라 런타임에도 사용될 수 있는 기능의 경우 GUI의 함수를 사용한다.
        GUI.Label(new Rect(200, 0, 150, 50), "GUI.Label");
        EditorGUI.LabelField(new Rect(200, 50, 150, 50), "EditorGUI.LabelField");
        
        // Layout이 붙은 클래스들은 자동으로 요소를 배치해주기 때문에 별도의 Rect를 지정하지 않아도 된다.
        // 위치를 커스텀하게 지정할 필요가 없을 경우 사용한다.
        GUILayout.Label("GUILayout.Label");
        EditorGUILayout.LabelField("EditorGUILayout.LabelField");
    }
}
