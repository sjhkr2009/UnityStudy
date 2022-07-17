using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class B01_Layout : EditorWindow {
    [MenuItem("Custom/02-1 Horizontal, Vertical Layout", false, 2000)]
    static void Open() {
        GetWindow<B01_Layout>();
    }

    private string[] inputTexts = new string[5];
    private void OnGUI() {
        // 공통적으로 Begin ~ End는 한 쌍을 이루어야 함에 유의할 것.
        
        // BeginHorizontal ~ EndHorizontal 사이의 요소는 가로로 나열된다.
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < 5; i++) {
            EditorGUILayout.LabelField($"Horizontal Label{i + 1}");
        }
        EditorGUILayout.EndHorizontal();
        
        // BeginVertical ~ EndVertical 사이의 요소는 세로로 나열된다.
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < 5; i++) {
            EditorGUILayout.LabelField($"Vertical Label{i + 1}");
        }
        EditorGUILayout.EndVertical();
        
        // GUI.skin.horizontalScrollbar로 가로 구분선을 그을 수 있다. GUI 스타일들에 대해서는 나중에 다루기로 한다.
        EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalScrollbar);
        
        // 가로/세로 레이아웃을 응용하여 그리드 형태의 InputField를 그려보기
        for (int i = 0; i < 3; i++) {
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.LabelField($"Title: {i + 1}");
            
            // EditorGUIUtility.fieldWidth, labelWidth는 필드 또는 라벨의 최소 크기를 나타낸다.
            // 창에 공간이 충분하지 않을 때도 이 크기 이하로 줄어들지 않게 된다.
            var originFieldWidth = EditorGUIUtility.fieldWidth;
            var originLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.fieldWidth = 50;
            EditorGUIUtility.labelWidth = 50;
            
            EditorGUILayout.BeginHorizontal();
            for (int k = 0; k < 5; k++) {
                inputTexts[k] = EditorGUILayout.TextField($"Input {k + 1}", inputTexts[k]);
            }
            EditorGUILayout.Space(20);
            EditorGUILayout.EndHorizontal();
            
            // EditorGUIUtility.fieldWidth, labelWidth는 글로벌하게 적용되는 변수이므로 다른 창에 영향이 가지 않도록 원래 값을 저장했다가 다시 넣어준다.
            EditorGUIUtility.fieldWidth = originFieldWidth;
            EditorGUIUtility.labelWidth = originLabelWidth;
            
            EditorGUILayout.EndVertical();
        }
        
        EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalScrollbar);
        
        // BeginHorizontal ~ EndHorizontal 부분을 아래와 같이 사용할수도 있다. scope.rect로 해당 Horizontal 크기를 알 수 있다.
        // 높이 또는 너비를 고정하려면 GUILayout.Height(100)와 같은 생성자 파라미터를 입력할 수 있다. 
        using (var scope = new EditorGUILayout.HorizontalScope()) {
            // scope 영역 전체를 버튼으로 사용하며, 별다른 문자를 띄우지 않음
            if (GUI.Button(scope.rect, GUIContent.none)) {
                Debug.Log("[B01_Layout.HorizontalScope] Button Clicked!");
            }

            for (int i = 0; i < 5; i++) {
                GUILayout.Label("버튼");
                
                // A02에서 다룬 IconContent와 동일하며 같은 이름을 입력하면 된다. (이미지 이름은 IconInfo/UnityEditorIconInfos.md 참고)
                // 차이는 반환형이 GUIContent가 아닌 Texture2D라는 점.
                GUILayout.Box(EditorGUIUtility.FindTexture("BuildSettings.Editor"));
            }
        }
    }
}
