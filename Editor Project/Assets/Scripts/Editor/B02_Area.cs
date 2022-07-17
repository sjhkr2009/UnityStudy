using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class B02_Area : EditorWindow {
    [MenuItem("Custom/02-2 Area, Scroll", false, 2001)]
    static void Open() {
        GetWindow<B02_Area>();
    }

    private Vector2 scrollPos01;
    private Vector2 scrollPos02;

    private void OnGUI() {
        var offset = new Vector2(20, 40);
        var rectArea = new Rect(offset.x, offset.y,
            // EditorWindow.position으로 현재 창의 Rect 정보를 알 수 있다. 이 창의 너비에 따라 가로 크기를 할당.
            position.width - offset.x * 2,
            200 // 세로 크기는 고정값으로 써보기로 한다.
        );
        
        GUILayout.BeginArea(rectArea, GUI.skin.window);
        
        var rectSubArea = new Rect(10, 10, rectArea.width * 0.5f - 20f, rectArea.height - 20f);
        { // 왼쪽 영역. Area 중첩으로 코드 가독성이 떨어져 임의로 중괄호를 넣었다. 
            GUILayout.BeginArea(rectSubArea, "Sub Area 01", GUI.skin.window);
            
            // BeginScrollView ~ EndScrollView 사이의 영역은 스크롤이 가능해진다.
            // 이 때 파라미터로 현재 스크롤 위치를 넣게 되는데, 스크롤한 위치를 반환받아 다시 넣어줘야 실제로 스크롤이 가능하다.
            // 그렇지 않으면 스크롤 위치가 고정되어 스크롤해도 다음 프레임에 원래 위치로 돌아간다.
            scrollPos01 = EditorGUILayout.BeginScrollView(scrollPos01);

            for (int i = 0; i < 30; i++) {
                EditorGUILayout.LabelField("Hello!");
            }
            
            EditorGUILayout.EndScrollView();
        
            GUILayout.EndArea();
        }
        
        { // 오른쪽 영역.
            rectSubArea.Set(10 + rectArea.width * 0.5f, 10, rectArea.width * 0.5f - 20f, rectArea.height - 20f);
            GUILayout.BeginArea(rectSubArea, "Sub Area 02", GUI.skin.window);

            scrollPos02 = EditorGUILayout.BeginScrollView(scrollPos02);
            
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < 30; i++) {
                EditorGUILayout.LabelField("World!");
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndScrollView();
        
            GUILayout.EndArea();
        }
        
        GUILayout.EndArea();
    }
}
