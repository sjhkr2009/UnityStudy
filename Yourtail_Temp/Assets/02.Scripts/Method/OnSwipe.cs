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
    int count;

    public void OnPointerDown(PointerEventData p)
    {
        moveDist = 0f;
        moveTime = 0f;

        startPos = p.position;
        prevPos = p.position;
    }
    public void OnDrag(PointerEventData p)
    {
        float delta = Vector2.Distance(prevPos, p.position);
        //Debug.Log($"{count++}회차 거리 : {delta}");
        prevPos = p.position;
    }
    public void OnPointerUp(PointerEventData p)
    {
        endPos = p.position;
        moveDist = Vector2.Distance(startPos, endPos);
        moveSpeed = moveDist / moveTime;
        Debug.Log($"총 이동거리: {moveDist} / 이동 시간: {moveTime} / 이동 속도: {moveSpeed}");
    }

    private void Update()
    {
        moveTime += Time.deltaTime;
    }
}
