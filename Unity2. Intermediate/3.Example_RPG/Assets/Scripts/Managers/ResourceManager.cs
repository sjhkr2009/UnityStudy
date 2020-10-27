using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
	{
		return Resources.Load<T>(path);
	}

	public GameObject Instantiate(string path, Transform parent = null)
	{
		GameObject prefab = Load<GameObject>($"{Define.ResourcesPath.Prefab}{path}");
		if(prefab == null)
		{
			Debug.Log($"Failed to load Prefab : {path}");
			return null;
		}

		return Object.Instantiate(prefab, parent);
	}

	public void Destroy(GameObject target, float delay = 0f)
	{
		if (target == null) return;

		Object.Destroy(target, delay);
	}
}
