using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 자주 쓰이는 기능을 전역 함수로 저장하고 있는 클래스
/// </summary>
public class Utility
{
	/// <summary>
	/// 게임오브젝트에서 컴포넌트를 가져오고, 없을 경우 추가하여 반환합니다.
	/// </summary>
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

/// <summary>
/// 자주 쓰이는 기능을 확장 메서드로 제공합니다. 인스턴스화되지 않습니다.
/// </summary>
public static class CustomExtension
{
	/// <summary>
	/// 이 오브젝트에서 컴포넌트를 가져오고, 없을 경우 추가하여 반환합니다.
	/// </summary>
	public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
	{
		return Utility.GetOrAddComponent<T>(obj);
	}
	public static T GetOrAddComponent<T>(this Transform tr) where T : Component
	{
		return Utility.GetOrAddComponent<T>(tr.gameObject);
	}

	/// <summary>
	/// 이 리스트에 해당 원소를 추가합니다. 이미 있을 경우 중복해서 추가되지 않습니다.
	/// </summary>
	/// <returns>리스트에 요소를 추가했다면 true, 이미 해당 요소가 있다면 false를 반환합니다.</returns>
	public static bool AddUnique<T>(this List<T> list, T element)
	{
		if (!list.Contains(element))
		{
			list.Add(element);
			return true;
		}

		return false;
	}

	/// <summary>
	/// 이 리스트와 다른 리스트를 병합합니다. 이미 이 리스트에 있는 요소는 중복 추가되지 않습니다.
	/// </summary>
	public static void OverlapFrom<T>(this List<T> to, List<T> from)
	{
		foreach (T element in from)
		{
			to.AddUnique(element);
		}
	}
}