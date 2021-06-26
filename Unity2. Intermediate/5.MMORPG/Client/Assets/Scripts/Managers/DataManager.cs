using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 클래스는 저장하고 있는 자료들을 Value로, 자료를 식별할 수 있는 요소를 Key로 설정하여 DIctionary를 반환해야 합니다.
public interface ILoader<Key, Value>
{
	Dictionary<Key, Value> LoadData();
}

public class DataManager
{
	//example: public Dictionary<int, Data.Stat> StatDict { get; private set; }

    public void Init()
	{
		//example: StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").LoadData();
	}

	/// <summary>
	/// Json 파일을 클래스를 통해 읽어와서, 해당 클래스들을 담은 데이터 클래스를 반환합니다.
	/// (예를 들어, 레벨에 따른 스탯 정보를 읽어온다면 Loader, Key, Value는 StatData, int(레벨), Stat 형태가 됩니다.)
	/// </summary>
	/// <typeparam name="Loader">자료들을 저장한 데이터 클래스. ILoader가 상속되어야 합니다.</typeparam>
	/// <typeparam name="Key">자료를 구분할 수 있는 식별자.</typeparam>
	/// <typeparam name="Value">Json에서 자료를 읽어올 클래스.</typeparam>
	/// <param name="path"></param>
	/// <returns></returns>
	Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
	{
		if (!path.Contains(Define.ResourcesPath.Data))
			path = $"{Define.ResourcesPath.Data}{path}";
		
		TextAsset textAsset = Director.Resource.Load<TextAsset>(path);
		return JsonUtility.FromJson<Loader>(textAsset.text);
	}
}
