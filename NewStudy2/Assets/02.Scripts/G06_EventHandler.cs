using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;



public class G06_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    // 이벤트 핸들러가 UI에 컴포넌트로 붙을 경우, 해당 오브젝트와 그 하위 오브젝트는 이벤트 핸들러의 영향을 받는다.
    // 이벤트 핸들러는 UI 이벤트 관련 인터페이스를 상속받음으로써 만들 수 있다.

    // 이벤트 발생 시 각 오브젝트에 뿌려줄 이벤트를 만들고, 각 오브젝트에서 이벤트 발생 시 수행할 동작을 추가해준다.
    // 이 방식으로 인스펙터에서 드래그 앤 드롭으로 함수를 설정하는 것을 코드로 대체할 수 있다.
    public Action<PointerEventData> EventOnDrag = p => { };
    public Action<PointerEventData> EventOnClick = p => { };

    public void OnDrag(PointerEventData eventData)
    {
        EventOnDrag(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventOnClick(eventData);
    }
}
