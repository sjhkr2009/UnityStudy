using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    Dictionary<string, Sprite> _loadedSprites = new Dictionary<string, Sprite>();
	Dictionary<int, StageData> _loadedStageData = new Dictionary<int, StageData>();

    public T Load<T>(string path) where T : Object
	{
		T source = Resources.Load<T>(path);
		if(source == null)
		{
			Debug.LogError($"Failed to Load Resource({typeof(T)}): {path}");
			return null;
		}

		return source;
	}

	public Sprite Load(string path, bool saveSprite = true)
	{
		if (saveSprite && _loadedSprites.ContainsKey(path))
		{
			return _loadedSprites[path];
		}

		Sprite sprite = Resources.Load<Sprite>(path);
		if(sprite == null)
		{
			return null;
		}

		_loadedSprites.Add(path, sprite);
		return sprite;
	}

	public StageData LoadStageData(int stageNumber)
	{
		if(_loadedStageData.TryGetValue(stageNumber, out StageData data))
		{
			return data;
		}

		string path = $"StageData/Stage{stageNumber}";
		data = Load<StageData>(path);
		if(data == null)
		{
			Debug.Log($"Failed to Load Stage {stageNumber} Data");
			return null;
		}

		_loadedStageData.Add(stageNumber, data);
		return data;
	}

	public GameObject Instantiate(string prefabName, Transform parent = null)
	{
		GameObject origin = Load<GameObject>($"Prefabs/{prefabName}");
		if (origin == null)
		{
			Debug.Log($"Failed to Load Prefab : {prefabName} (Prefab has to be in Resources/Prefabs Folder)");
			return null;
		}

		GameObject gameObject = Object.Instantiate(origin, parent);

		return gameObject;
	}
}
