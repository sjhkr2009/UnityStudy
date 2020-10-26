using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnSwipe : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    Vector2 startPos;
    Vector2 endPos;
    float moveDist;
    float moveTime;
    float moveSpeed;

    Vector2 prevPos;
    float startTime;
    int deltaCount;
    [ReadOnly] public bool dragLeft;

    // 화면 스와이프 시 동작을 실행합니다. 매개변수가 true면 왼쪽으로, false면 오른쪽으로 스와이프했음을 의미합니다.
    public Action<bool> EventOnSwipe = b => { };

    public void OnPointerDown(PointerEventData p)
    {
        moveDist = 0f;
        moveTime = 0f;
        startTime = GameManager.Instance.PlayTime;
        deltaCount = 0;

        startPos = p.position;
        prevPos = p.position;
    }
    public void OnDrag(PointerEventData p)
    {
        deltaCount++;
        if(deltaCount > 3)
        {
            prevPos = p.position;
            deltaCount = 0;
        }
    }
    public void OnPointerUp(PointerEventData p)
    {
        endPos = p.position;
        dragLeft = (prevPos.x - endPos.x) > 0;

        float endTime = GameManager.Instance.PlayTime;
        moveTime = endTime - startTime;

        moveDist = Vector2.Distance(startPos, endPos);
        moveSpeed = moveDist / moveTime;
        //Debug.Log($"총 이동거리: {moveDist} / 이동 시간: {moveTime} / 이동 속도: {moveSpeed}");

        SwipeCheck();
    }

    void SwipeCheck()
    {
        if (moveSpeed > 350f || moveDist > 300f)
        {
            Swipe();
        }
    }

    virtual protected void Swipe()
    {
        EventOnSwipe(dragLeft);
    }

    private void OnDestroy()
    {
        EventOnSwipe = null;
    }
}
