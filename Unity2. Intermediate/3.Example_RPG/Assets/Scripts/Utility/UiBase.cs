using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBase : MonoBehaviour
{
	Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected void Bind<T>(Type type, bool findAllChild = true) where T : UnityEngine.Object
	{
		string[] names = Enum.GetNames(type);

		UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
		_objects.Add(typeof(T), objects);

		for (int i = 0; i < names.Length; i++)
		{
			if (typeof(T) == typeof(GameObject))
				objects[i] = CustomUtility.FindChild(gameObject, names[i], findAllChild);
			else
				objects[i] = CustomUtility.FindChild<T>(gameObject, names[i], findAllChild);

			if (objects[i] == null)
				Debug.Log($"Failed to Bind : {names[i]} ({type.Name})");
		}
	}

	protected T Get<T>(int index) where T : UnityEngine.Object
	{
		UnityEngine.Object[] objects;
		if (_objects.TryGetValue(typeof(T), out objects))
			return objects[index] as T;
		else
			return null;
	}

	protected Text GetText(int index) => Get<Text>(index);
	protected Image GetImage(int index) => Get<Image>(index);
	protected Button GetButton(int index) => Get<Button>(index);
	protected RectTransform GetRectTransform(int index) => Get<RectTransform>(index);
}
