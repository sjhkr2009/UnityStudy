using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    float maxDistance;
    public float maxDragDistance = 40f;

    void Start()
    {
        maxDistance = transform.parent.localScale.x / 2;
    }
    public void OnDrag(PointerEventData data)
    {
        Vector2 touchPositionInWorld = Camera.main.ScreenToWorldPoint(data.position);

        transform.position = touchPositionInWorld;

        if (maxDistance < Vector2.Distance(transform.parent.position, transform.position))
        {
            Vector2 currentPos = transform.localPosition;
            Vector2 currentDistance = currentPos - Vector2.zero;
            currentDistance.Normalize();

            transform.localPosition = currentDistance * maxDragDistance;
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("Drag Begin!");
    }

    public void OnEndDrag(PointerEventData data)
    {
        transform.localPosition = Vector2.zero;
        Debug.Log("End Drag");
    }

    public Vector2 PlayerMove()
    {
        Vector2 playerMove = transform.localPosition;
        playerMove.Normalize();
        return playerMove;
    }
}