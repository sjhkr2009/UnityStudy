﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiEventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
	public event Action<PointerEventData> OnDragHandler = null;
	public event Action<PointerEventData> OnClickHandler = null;

	public void OnDrag(PointerEventData eventData)
	{
		OnDragHandler?.Invoke(eventData);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClickHandler?.Invoke(eventData);
	}
}
