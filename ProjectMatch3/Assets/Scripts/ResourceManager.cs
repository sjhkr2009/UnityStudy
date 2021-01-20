using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임의 리소스를 생성 및 관리합니다. Load 및 Instantiate를 매핑하며 자주 로딩하는 요소는 저장합니다.
/// TODO: 오브젝트 풀 구현 시 이 클래스를 통한 Instantiate/Destory는 오브젝트 풀을 통해 생성/비활성화 처리
/// </summary>
public class ResourceManager
{
    Dictionary<string, Sprite> _loadedSprites = new Dictionary<string, Sprite>();
	Dictionary<int, StageData> _loadedStageData = new Dictionary<int, StageData>();

	/// <summary>
	/// 지정한 타입의 Resource를 로딩해서 반환합니다.
	/// </summary>
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

	/// <summary>
	/// Sprite 타입에 대한 Load 동작을 수행합니다. 로딩된 요소를 저장할 수 있습니다.
	/// </summary>
	/// <param name="saveSprite">true일 경우 로딩한 Sprite를 딕셔너리에 저장합니다. 기본값은 true입니다.</param>
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

	/// <summary>
	/// 스테이지에 대한 데이터를 로딩합니다. 로딩한 데이터는 저장됩니다.
	/// </summary>
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

	/// <summary>
	/// 프리팹을 로딩하여 생성합니다.
	/// TODO: 오브젝트 풀 추가 시 로딩된 프리팹을 통해 오브젝트 풀을 생성할 수 있게 할 것
	/// </summary>
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

	/// <summary>
	/// 저장된 리소스들을 해제합니다.
	/// </summary>
	public void Clear()
	{
		_loadedSprites.Clear();
		_loadedStageData.Clear();
	}
}
