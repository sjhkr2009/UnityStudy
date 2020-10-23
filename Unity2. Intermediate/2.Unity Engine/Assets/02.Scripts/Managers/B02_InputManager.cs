using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class B02_InputManager
{
    // 사용자의 입력을 관리할 매니저. 싱글톤인 게임매니저가 있으니 이쪽은 굳이 MonoBehavior로 만들 필요가 없다.
    // 오브젝트들의 Update문마다 입력을 체크하면 부하가 심하니, 이곳의 Update를 통해 입력을 체크하고 이벤트로 다른 곳에 넘겨준다.

    public Action OnKeyAction = () => { };
    public Action<E02_Define.MouseEvent> OnMouseEvent = m => { };

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // EventSystem을 이용한 이벤트가 발생한 경우(UI 등), 게임 내 조작이 아닌 UI상의 조작일 수 있으니 바로 리턴한다.


        if (Input.anyKey && OnKeyAction != null) OnKeyAction();

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            OnMouseEvent(E02_Define.MouseEvent.Press);
        }
        if (Input.GetMouseButtonUp(1))
        {
            OnMouseEvent(E02_Define.MouseEvent.Click);
        }
    }

    public void Clear()
    {
        OnKeyAction = null;
        OnMouseEvent = null;
    }
}
