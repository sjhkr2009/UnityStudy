using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform handle;

    float backgroundRadius;
    bool isMove;
    Vector2 moveVelocity = Vector2.zero;

    public Action<Vector2> EventOnDrag;

    void Start()
    {
        backgroundRadius = background.rect.width * 0.5f;
        isMove = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isMove = true;
        OnDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dir = eventData.position - (Vector2)background.position;
        float distance = Mathf.Min(dir.magnitude, backgroundRadius);
        dir.Normalize();

        handle.localPosition = dir * distance;
        moveVelocity = handle.localPosition.normalized * (distance / backgroundRadius);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMove = false;
        handle.localPosition = Vector2.zero;
        moveVelocity = Vector2.zero;
    }
    
    void Update()
    {
        if(isMove) EventOnDrag(moveVelocity);
    }
}
