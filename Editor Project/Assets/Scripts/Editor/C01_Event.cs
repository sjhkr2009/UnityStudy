using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class C01_Event : EditorWindow {
    [MenuItem("Custom/03-1 Event", false, 2003)]
    static void Open() {
        GetWindow<C01_Event>();
    }

    private void OnGUI() {
        // 유니티 에디터 내에서 하는 모든 행동들은 이벤트를 발생시키고, 이에 따라 OnGUI 함수가 발동한다.
        // 로그를 찍어보면 EditorWindow 위에서 키보드 입력이나 마우스 진입, 이탈, 이동, 드래그, 클릭 등에 대해 반응하는 것을 알 수 있다.
        //Debug.Log(Event.current.type);
        
        
        // 한 프레임 내에서도 여러 차례 이벤트가 발생할 수 있으나 모든 이벤트가 GUI를 그리는 건 아니다.
        // 마우스가 창에 진입하거나 이동할 때 Layout -> repaint 순으로 이벤트가 발생하는 것을 볼 수 있다.
        // Layout에서 미리 GUILayout 관련 코드들을 조사해서 위치값들을 계산하고, 이 값에 따라 repaint를 호출하여 요소들을 다시 그리기 때문.

        // 이벤트를 조합해서 조건을 걸 수 있다.
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A) {
            Debug.Log("<color=lime>Keyboard Down: A</color>");
        }
        
        // Use()를 호출하면 해당 이벤트를 없앤다. 즉 이후로는 이 이벤트에 대한 반응을 하지 않는다. 
        if (Event.current.isMouse) {
            Event.current.Use();
        }
        
        // 위에서 이벤트를 소거했으므로 버튼은 마우스 이벤트를 받을 수 없어 클릭되지 않는다.
        if (GUILayout.Button("Try Click!")) {
            Debug.LogError("Button Is Clicked!!");
        }
        
    }
}
