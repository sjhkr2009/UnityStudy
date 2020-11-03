using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
	Dictionary<Key, Value> LoadData();
}

public class DataManager
{
	public Dictionary<int, Stat> StatDict { get; private set; }

    public void Init()
	{
		StatDict = LoadJson<StatData, int, Stat>("StatData").LoadData();
	}

	Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
	{
		if (!path.Contains(Define.ResourcesPath.Data))
			path = $"{Define.ResourcesPath.Data}{path}";
		
		TextAsset textAsset = GameManager.Resource.Load<TextAsset>(path);
		return JsonUtility.FromJson<Loader>(textAsset.text);
	}
}
