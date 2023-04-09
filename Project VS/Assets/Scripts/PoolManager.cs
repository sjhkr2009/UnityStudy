using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class PoolManager {
    class PoolInfo {
        public GameObject Origin { get; }
        private Queue<GameObject> Pool { get; }
        public string Name { get; }

        public PoolInfo(GameObject originPrefab, string name) {
            Origin = originPrefab;
            Pool = new Queue<GameObject>();
            Name = name;
        }

        private GameObject CreateItem() {
            var obj = Object.Instantiate(Origin, PoolParent);
            obj.name = Name;
            obj.GetOrAddPoolable();

            return obj;
        }

        public void Warmup(int count) {
            while (Pool.Count >= count) {
                Pool.Enqueue(CreateItem());
            }
        }
        
        public GameObject GetItem() {
            GameObject item = (Pool.Count > 0) ? Pool.Dequeue() : CreateItem();
            item.GetOrAddPoolable().Initialize();
            return item;
        }

        public void ReleaseItem(GameObject item) {
            item.GetOrAddPoolable().Release();
            Pool.Enqueue(item);
        }
        
        public PoolInfo(GameObject originPrefab) : this(originPrefab, originPrefab.name) { }
    }
    
    
    private static Dictionary<string, PoolInfo> Pools { get; } = new Dictionary<string, PoolInfo>();

    private static Transform poolParent;
    public static Transform PoolParent {
        get {
            if (poolParent) return poolParent;

            var poolObj = new GameObject("Pool");
            Object.DontDestroyOnLoad(poolObj);
            poolParent = poolObj.transform;
            return poolParent;
        }
    }

    private static IPoolable GetOrAddPoolable(this GameObject gameObject) {
        var poolable = gameObject.GetComponent<IPoolable>();
        if (poolable == null) {
            gameObject.AddComponent<BasicPoolable>();
        }

        return poolable;
    }

    public static T Get<T>(string name, Transform parent = null, bool stayTransform = false) where T : Component {
        var item = Get(name, parent, stayTransform);
        return item ? item.GetComponent<T>() : null;
    }
    
    public static GameObject Get(string name, Transform parent = null, bool stayTransform = false) {
        GameObject item = null;
        if (Pools.TryGetValue(name, out var poolInfo)) {
            item = poolInfo.GetItem();
        }

        if (item == null) {
            var prefab = Resources.Load<GameObject>(name);
            if (prefab == null) {
                Debug.LogError($"[PoolManager.Get] Cannot Find Object '{name}' in Resources!!");
                return null;
            }

            poolInfo = new PoolInfo(prefab);
            Pools.Add(name, poolInfo);
            item = poolInfo.GetItem();
        }

        if (parent != null) {
            item.transform.SetParent(parent);
            if (!stayTransform) item.transform.ResetTransform();
        }

        return item;
    }

    public static void Abandon(GameObject item) {
        if (Pools.TryGetValue(item.name, out var poolInfo)) {
            poolInfo.ReleaseItem(item);
            return;
        }
        
        Debug.LogError($"[PoolManager.Abandon] Cannot Find PoolInfo of '{item.name}'!!");
        Object.Destroy(item);
    }
}
