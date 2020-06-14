using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B02_InputManager
{
    // 사용자의 입력을 관리할 매니저. 싱글톤인 게임매니저가 있으니 이쪽은 굳이 MonoBehavior로 만들 필요가 없다.
    // 오브젝트들의 Update문마다 입력을 체크하면 부하가 심하니, 이곳의 Update를 통해 입력을 체크하고 이벤트로 다른 곳에 넘겨준다.

    public Action OnKeyAction = null;

    public void OnUpdate()
    {
        if (!Input.anyKey) return; //아무 입력도 없으면 바로 리턴

        OnKeyAction?.Invoke(); // OnKeyAction이 null이 아닐 경우, 실행시킨다.
    }
}
