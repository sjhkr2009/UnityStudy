using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
	class Pool
	{
		public GameObject Origin { get; private set; }
		public Transform Parent { get; set; }

		Stack<Poolable> _poolStack = new Stack<Poolable>();

		public void Init(GameObject origin, int count)
		{
			if (count <= 0)
				count = Define.DefaultSetting.PoolCount;

			Origin = origin;
			Parent = new GameObject(Define.DefaultName.ToPoolable(origin.name)).transform;

			for (int i = 0; i < count; i++)
			{
				Push(Create());
			}
		}

		Poolable Create()
		{
			GameObject go = Object.Instantiate(Origin);
			go.name = Origin.name;
			return go.GetOrAddComponent<Poolable>();
		}

		public void Push(Poolable poolable)
		{
			if (poolable == null)
				return;

			poolable.transform.parent = Parent;
			poolable.gameObject.SetActive(false);

			_poolStack.Push(poolable);
		}

		public Poolable Pop(Transform parent)
		{
			Poolable poolable = (_poolStack.Count > 0) ? _poolStack.Pop() : Create();

			poolable.gameObject.SetActive(true);

			if (parent == null)
				poolable.transform.parent = GameManager.Scene.CurrentScene.transform;

			poolable.transform.parent = parent;

			return poolable;
		}
	}
	
	Transform _root;
	Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();

    public void Init()
	{
		if(_root == null)
		{
			_root = new GameObject(Define.DefaultName.PoolRoot).transform;
			Object.DontDestroyOnLoad(_root);
		}
	}

	public void Push(Poolable poolable)
	{
		string name = poolable.gameObject.name;

		if (!_pool.ContainsKey(name))
		{
			Object.Destroy(poolable.gameObject);
			return;
		}

		_pool[name].Push(poolable);
	}

	public Poolable Pop(GameObject origin, Transform parent = null)
	{
		Pool pool;
		if (!_pool.TryGetValue(origin.name, out pool))
			pool = CreatePool(origin);

		return pool.Pop(parent);
	}

	Pool CreatePool(GameObject origin)
	{
		int count = GetPoolCount(origin);
		if (count <= 0)
			count = Define.DefaultSetting.PoolCount;

		Pool pool = new Pool();
		pool.Init(origin, count);
		pool.Parent.parent = _root;

		_pool.Add(origin.name, pool);

		return pool;
	}

	int GetPoolCount(GameObject origin)
	{
		Poolable poolable = origin.GetComponent<Poolable>();
		if (poolable == null)
			return -1;

		return poolable.PoolCount;
	}

	public GameObject GetOrigin(string name)
	{
		if (!_pool.ContainsKey(name))
			return null;

		return _pool[name].Origin;
	}

	public void Clear()
	{
		foreach (Transform child in _root)
			Object.Destroy(child.gameObject);

		_pool.Clear();
	}
}
