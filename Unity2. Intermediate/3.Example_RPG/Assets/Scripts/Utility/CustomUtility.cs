using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomUtility
{
    public static T FindChild<T>(GameObject parent, string name, bool recursive = true) where T : UnityEngine.Object
	{

		if ((parent == null) || (string.IsNullOrEmpty(name)))
			return null;

		if (recursive)
		{
			foreach (T component in parent.GetComponentsInChildren<T>())
			{
				if (component.name == name)
					return component;
			}
		}
		else
		{
			for (int i = 0; i < parent.transform.childCount; i++)
			{
				Transform tr = parent.transform.GetChild(i);
				if(tr.name == name)
				{
					T component = tr.GetComponent<T>();
					if (component != null)
						return component;
				}
			}
		}
		
		return null;
	}
	public static GameObject FindChild(GameObject parent, string name, bool recursive = true)
	{
		Transform tr = FindChild<Transform>(parent, name, recursive);
		if (tr == null)
			return null;

		return tr.gameObject;
	}
	public static GameObject BindEvent(GameObject go, Action<PointerEventData> action, Define.UiEvent type = Define.UiEvent.Click)
	{
		UiEventHandler evt = go.GetOrAddComponent<UiEventHandler>();

		switch (type)
		{
			case Define.UiEvent.Click:
				evt.OnClickHandler -= action;
				evt.OnClickHandler += action;
				break;
			case Define.UiEvent.Drag:
				evt.OnDragHandler -= action;
				evt.OnDragHandler += action;
				break;
			default:
				break;
		}

		return go;
	}
	public static T GetOrAddComponent<T>(GameObject obj) where T : Component
	{
		T component = obj.GetComponent<T>();

		if (component != null)
			return component;
		else
			return obj.AddComponent<T>();
	}
}
