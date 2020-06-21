using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class G07_Extension
{
    // 익스텐션
    // 특정 변수에 .함수명(매개변수) 을 입력하여 발동가능한 함수를 추가할 수 있다. C++에서는 지원하지 않는다.
    // 익스텐션 클래스와 함수들은 static으로 선언해야 하며, 익스텐션으로 확장할 대상 앞에 this를 붙이면 된다.

    // 아래와 같이 선언하면 GameObject 클래스 내에 없는 함수라도 obj.AddUIEvent로 사용할 수 있다. 이렇게 발동할 경우 매개 변수 gameObject에는 자동으로 obj가 들어간다.
    public static void AddUIEvent(this GameObject gameObject, Action<PointerEventData> action, E02_Define.EventType eventType = E02_Define.EventType.Click)
    {
        G05_UIBase.AddEvent(gameObject, action, eventType);
    }
}
