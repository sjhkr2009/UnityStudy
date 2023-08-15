using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class PoolManager {
    private static HashSet<GameObject> CreatedObjects { get; } = new HashSet<GameObject>();

    class PoolInfo {
        public GameObject Origin { get; }
        private Queue<GameObject> Pool { get; }
        private bool HasHandler { get; }
        public string Name { get; }

        public PoolInfo(GameObject originPrefab, string name) {
            Origin = originPrefab;
            HasHandler = originPrefab.GetComponent<IPoolHandler>() != null;
            Pool = new Queue<GameObject>();
            Name = name;
        }

        private GameObject CreateItem() {
            var obj = Object.Instantiate(Origin, PoolParent);
            obj.name = Name;

            return obj;
        }

        public void Warmup(int count) {
            while (Pool.Count >= count) {
                Pool.Enqueue(CreateItem());
            }
        }
        
        public GameObject GetItem() {
            GameObject item = (Pool.Count > 0) ? Pool.Dequeue() : CreateItem();
            item.SetActive(true);
            if (HasHandler) item.GetComponent<IPoolHandler>()?.OnInitialize();
            CreatedObjects.Add(item);
            return item;
        }

        public void ReleaseItem(GameObject item) {
            if (HasHandler) item.GetComponent<IPoolHandler>()?.OnRelease();
            item.SetActive(false);
            item.transform.SetParent(PoolParent);
            CreatedObjects.Remove(item);
            Pool.Enqueue(item);
        }
        
        public PoolInfo(GameObject originPrefab) : this(originPrefab, originPrefab.name) { }
    }
    
    
    private static Dictionary<string, PoolInfo> Pools { get; } = new Dictionary<string, PoolInfo>();

    private static Transform poolParent;
    private static Transform PoolParent {
        get {
            if (poolParent) return poolParent;

            var poolObj = new GameObject("Pool");
            Object.DontDestroyOnLoad(poolObj);
            poolParent = poolObj.transform;
            return poolParent;
        }
    }
    
    public static T GetByType<T>(Transform parent = null, bool stayTransform = false) where T : Component {
        return Get<T>(typeof(T).Name, parent, stayTransform);
    }

    public static T Get<T>(string name, Transform parent = null, bool stayTransform = false) where T : Component {
        var item = Get(name, parent, stayTransform);
        return item ? item.GetComponent<T>() : null;
    }
    
    public static GameObject Get(string name, Transform parent = null, bool stayTransform = false) {
        if (string.IsNullOrEmpty(name)) {
            Debugger.Error($"[PoolManager.Get] Object Name Cannot null or Empty!!");
            return null;
        }
        
        GameObject item = null;
        if (Pools.TryGetValue(name, out var poolInfo)) {
            item = poolInfo.GetItem();
        }

        if (!item) {
            var prefab = Resources.Load<GameObject>(name);
            if (!prefab) {
                Debugger.Error($"[PoolManager.Get] Cannot Find Object '{name}' in Resources!!");
                return null;
            }

            poolInfo = new PoolInfo(prefab);
            Pools.Add(name, poolInfo);
            item = poolInfo.GetItem();
        }

        if (parent) {
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
        
        Debugger.Error($"[PoolManager.Abandon] Cannot Find PoolInfo of '{item.name}'!!");
        Object.Destroy(item);
    }

    public static void AbandonAll() {
        List<GameObject> targets = new List<GameObject>();
        CreatedObjects.ForEach(obj => {
            if (obj) targets.Add(obj);
        });
        
        targets.ForEach(Abandon);
        CreatedObjects.Clear();
    }
}
