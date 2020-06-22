using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //이벤트 시스템 사용 시

public class EventTrigger : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler //인터페이스 사용 가능
{
    public void OnPointerClick(PointerEventData data) //위 인터페이스 사용 시 마우스가 오브젝트 클릭 시 발동하는 함수를 사용해야 함.
    {
        Debug.Log(data.position); //data에는 클릭에 관한 데이터를 가져온다. (위치, 선택한 오브젝트 등)
    }

    public void OnBeginDrag(PointerEventData data) //드래그 시작 시 발동되는 함수
    {
        Debug.Log("Event: 드래그 시작");
        transform.position = data.position;
    }

    public void OnDrag(PointerEventData data) //드래그 중 계속 발동되는 함수
    {
        Debug.Log("Event: 드래그 중");
        transform.position = data.position;
    }
    public void OnEndDrag(PointerEventData data) //드래그 종료 시 발동되는 함수
    {
        Debug.Log("Event: 드래그 종료");
        transform.position = data.position;
    }

    public void OnClick()
    {
        Debug.Log("클릭");
    }

    public void OnDrag()
    {
        Debug.Log("드래그 중");
    }

    public void UnClick()
    {
        Debug.Log("클릭 해제");
    }

    public void GetClick()
    {
        Debug.Log("이 오브젝트 선택");
    }

    public void MouseIn()
    {
        Debug.Log("마우스 올림");
    }
    public void MouseOut()
    {
        Debug.Log("마우스 떨어짐");
    }
}
