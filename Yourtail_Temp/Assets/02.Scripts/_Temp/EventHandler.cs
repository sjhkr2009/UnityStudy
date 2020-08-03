using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
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