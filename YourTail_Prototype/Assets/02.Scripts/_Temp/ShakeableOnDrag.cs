using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;

public class ShakeableOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField, ReadOnly] bool isEnd = false;
    Vector2 originPos;
    Vector2 startPos;

    private void OnEnable()
    {
        originPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEnd) return;

        transform.DOKill();
        startPos = eventData.position;

        if(originPos.x < startPos.x)
        {
            transform.DORotate(new Vector3(0f, 0f, 20f), 1f);
        }
        else
        {
            transform.DORotate(new Vector3(0f, 0f, -20f), 1f);
        }
        transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isEnd) return;

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isEnd) return;

        transform.DOMove(originPos, 1.5f);
    }
}
