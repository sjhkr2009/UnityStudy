using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class CustomExtension
{
	public static T FindChild<T>(GameObject parent, string name, bool recursive = true) where T : UnityEngine.Object
	{
		return CustomUtility.FindChild<T>(parent, name, recursive);
	}

	public static GameObject FindChild(GameObject parent, string name, bool recursive = true)
	{
		return CustomUtility.FindChild(parent, name, recursive);
	}

	public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
	{
		return CustomUtility.GetOrAddComponent<T>(obj);
	}

	public static GameObject BindEvent(this GameObject go, Action<PointerEventData> action, Define.UiEvent type = Define.UiEvent.Click)
	{
		return CustomUtility.BindEvent(go, action, type);
	}

	public static bool IsValid(this GameObject go)
	{
		return (go != null) && go.activeSelf;
	}
}
