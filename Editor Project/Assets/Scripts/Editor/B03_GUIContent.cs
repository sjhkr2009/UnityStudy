using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class B03_GUIContent : EditorWindow {
    [MenuItem("Custom/02-3 GUI Content", false, 2002)]
    static void Open() {
        GetWindow<B03_GUIContent>();
    }


    private void OnGUI() {
        // 여러 곳에서 공통적으로 쓰일 양식같은게 있다면 GUIContent로 정의해둘 수 있다.
        var myContent = new GUIContent();

        myContent.text = "Hello, world!";
        myContent.image = EditorGUIUtility.FindTexture("BuildSettings.Editor");
        myContent.tooltip = "Why so serious?";
        
        // 이후 myContent로 GUI를 그리면 위의 텍스트, 이미지, 툴팁이 기본적으로 적용된다.
        EditorGUILayout.LabelField(myContent);
        GUILayout.Button(myContent);
    }
}
