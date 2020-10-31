using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
	{
		return Resources.Load<T>(path);
	}

	public GameObject Instantiate(string path, Transform parent = null, bool setOriginName = true)
	{
		GameObject prefab = Load<GameObject>(Define.ResourcesPath.ToPrefab(path));
		if(prefab == null)
		{
			Debug.Log($"Failed to load Prefab : {path}");
			return null;
		}

		GameObject go = Object.Instantiate(prefab, parent);

		if (setOriginName)
		{
			int index = go.name.IndexOf("(Clone)");
			if (index > 0)
				go.name = go.name.Substring(0, index);
		}

		return go;
	}

	public void Destroy(GameObject target, float delay = 0f)
	{
		if (target == null) return;

		Object.Destroy(target, delay);
	}
}
