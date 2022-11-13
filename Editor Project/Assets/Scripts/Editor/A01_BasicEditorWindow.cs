using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class A01_BasicEditorWindow : EditorWindow {
    // MenuItem으로 상단바에 커스텀 항목을 추가한다.
    // 단축키 지정이 가능하다.
    /*
     * % : Ctrl (맥에서 Command)
     * & : Alt (맥에서 Option)
     * # : Shift
     * 숫자나 문자, F1~F12, HOME, END, PGUP, PUDN 지정 가능 (키패드의 숫자와 문자는 KP를 붙인다. KP4, KP/, KP* 등등)
     */
    // MenuItem 경로가 "GameObject/"로 시작하면 하이어라키에서 우클릭시 나오는 메뉴, "Assets/"로 시작하면 프로젝트 창 우클릭시 나오는 메뉴에 나온다.
    // "CONTEXT/{컴포넌트명}/" 으로 시작하면 인스펙터에서 해당 컴포넌트 우클릭 시 나오게 된다.
    [MenuItem("Custom/01-1 Basic %&#f1")]
    static void OpenWindow() {
        // static 함수인 EditorWindow.GetWindow를 사용해서 창을 띄울 수 있다.
        GetWindow<A01_BasicEditorWindow>("ㅎㅎ");
    }
    
    // 에디터 창이 Draw 될 때마다 호출되는 함수. 명시적인 Repaint 요청이나, 창 위에 (마우스 이동을 포함한) 사용자의 입력이 감지된 경우 창을 다시 그리게 된다.
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
