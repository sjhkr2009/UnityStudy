using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class CustomExtension
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
	{
		return CustomUtility.GetOrAddComponent<T>(obj);
	}

	public static void AddUiEvent(this GameObject go, Action<PointerEventData> action, Define.UiEvent type = Define.UiEvent.Click)
	{
		CustomUtility.AddUiEvent(go, action, type);
	}
}
