using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
	public static T GetOrAddComponent<T>(GameObject obj) where T : Component
	{
		T component = obj.GetComponent<T>();

		return (component != null) ? component : obj.AddComponent<T>();
	}
	public static T GetOrAddComponent<T>(Transform tr) where T : Component
	{
		return GetOrAddComponent<T>(tr.gameObject);
	}
}

public static class CustomExtension
{
	public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
	{
		return Utility.GetOrAddComponent<T>(obj);
	}
	public static T GetOrAddComponent<T>(this Transform tr) where T : Component
	{
		return Utility.GetOrAddComponent<T>(tr.gameObject);
	}
}