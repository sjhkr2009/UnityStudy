using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{

    public GameObject Player { get; private set; }
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

	public Action<int> OnSpawnEvent;

    public GameObject Spawn(Define.ObjectType type, string path, Transform parent = null)
	{
        GameObject go = GameManager.Resource.Instantiate(path, parent);

		switch (type)
		{
			case Define.ObjectType.Player:
				Player = go;
				break;
			case Define.ObjectType.Monster:
				_monsters.Add(go);
				OnSpawnEvent?.Invoke(1);
				break;
			default:
				break;
		}

		return go;
	}

	public Define.ObjectType GetObjectType(GameObject go)
	{
		BaseUnitController bc = go.GetComponent<BaseUnitController>();

		if (bc == null)
			return Define.ObjectType.Unknown;

		return bc.ObjectType;
	}

	public void Despawn(GameObject go)
	{
		Define.ObjectType type = GetObjectType(go);

		switch (type)
		{
			case Define.ObjectType.Player:
				if(Player == go)
					Player = null;
				break;
			case Define.ObjectType.Monster:
				if (_monsters.Contains(go))
				{
					_monsters.Remove(go);
					OnSpawnEvent?.Invoke(-1);
				}
				break;
			default:
				break;
		}

		GameManager.Resource.Destroy(go);
	}
}
