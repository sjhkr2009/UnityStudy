using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J01_PoolManager
{
    //오브젝트를 불러올 때, 두 가지 최적화를 생각할 수 있다.
    // 1. 오브젝트 정보를 알고 있으면 Load할 필요 없음
    // 2. 오브젝트가 씬 상에 배치(=pooling)되어 있으면 Instantiate할 필요 없음

    class Pool
    {
        public GameObject Origin { get; private set; }
        public Transform Root { get; set; }
        public int Count { get; set; }

        Stack<J02_Poolable> poolStack = new Stack<J02_Poolable>();


        public void Init(GameObject original)
        {
            Origin = original;
            Root = new GameObject().transform;
            Root.name = $"{Origin.name}_Root";

            for (int i = 0; i < Count; i++)
                Push(Create());
        }

        J02_Poolable Create()
        {
            GameObject newObject = Object.Instantiate(Origin);
            newObject.name = Origin.name;
            return newObject.GetOrAddComponent<J02_Poolable>();
        }

        public void Push(J02_Poolable poolable)
        {
            if (poolable == null) return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;

            poolStack.Push(poolable);
        }

        public J02_Poolable Pop(Transform parent)
        {
            J02_Poolable poolable = null;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            // 부모 오브젝트가 명시되지 않으면, 게임 씬이 아니라 Pooling Root가 있는 DontDestroyOnLoad 씬에서 생성된다. 게임 씬에 생성되지 않은 채 씬이 종료된다면 이 오브젝트들은 파괴되지 않는다.
            // DontDestroyOnLoad는 임의로 해제할 수 없으며, DontDestroyOnLoad 씬에서 메인 씬으로 직접 옮겨야 한다.
            // 따라서 게임 씬에 있는 아무 씬에나 넣은 다음 다시 parent를 null로 바꾸면 게임 씬에 자리잡게 된다. 여기서는 씬 매니저를 이용하나, 카메라 등 다른 오브젝트에 넣었다 빼도 상관없다.
            if (parent == null) poolable.transform.parent = A01_Manager.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.isUsing = true;

            return poolable;
        }
    }


    Transform root;
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    public int basicCount = 10;

    public void Init()
    {
        if(root == null)
        {
            root = new GameObject("@Pooling_Root").transform;
            Object.DontDestroyOnLoad(root);
        }
    }

    public void Push(J02_Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (!_pool.ContainsKey(name))
        {
            Object.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public void CreatePool(GameObject original, int count)
    {
        Pool pool = new Pool();
        pool.Count = count;
        pool.Init(original);
        pool.Root.parent = root;

        _pool.Add(original.name, pool);
    }

    public J02_Poolable Pop(GameObject gameObject, Transform parent = null)
    {
        if (!_pool.ContainsKey(gameObject.name))
            CreatePool(gameObject, basicCount);

        return _pool[gameObject.name].Pop(parent);
    }

    // 원본 프리팹을 찾기 위한 함수. 입력받은 이름의 오브젝트가 풀링되어 있으면 그 원본 오브젝트를 반환한다.
    public GameObject GetOrigin(string name)
    {
        if (!_pool.ContainsKey(name))
            return null;

        return _pool[name].Origin;
    }

    public void Clear()
    {
        foreach (Transform child in root)
        {
            Object.Destroy(child.gameObject);
        }
        _pool.Clear();
    }
}
