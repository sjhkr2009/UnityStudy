using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
	{
		if (typeof(T) == typeof(GameObject))
		{
			string name = path;
			int index = name.LastIndexOf('/');
			if (index > 0)
				name = name.Substring(index + 1);

			GameObject go = GameManager.Pool.GetOrigin(name);
			if (go != null)
				return go as T;
		}

		
		return Resources.Load<T>(path);
	}

	public GameObject Instantiate(string path, Transform parent = null, bool setOriginName = true)
	{
		GameObject origin = Load<GameObject>(Define.ResourcesPath.ToPrefab(path));
		if(origin == null)
		{
			Debug.Log($"Failed to load Prefab : {path}");
			return null;
		}

		if (origin.GetComponent<Poolable>() != null)
			return GameManager.Pool.Pop(origin, parent).gameObject;

		GameObject go = Object.Instantiate(origin, parent);

		if (setOriginName)
			go.name = origin.name;

		return go;
	}

	public void Destroy(GameObject target, float delay = 0f)
	{
		if (target == null)
			return;

		Poolable poolable = target.GetComponent<Poolable>();
		if(poolable != null)
		{
			GameManager.Pool.Push(poolable);
			return;
		}

		Object.Destroy(target, delay);
	}
}
