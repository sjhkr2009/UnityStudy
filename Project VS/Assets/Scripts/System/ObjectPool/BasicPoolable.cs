using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPoolable : MonoBehaviour, IPoolable {
    protected virtual void Awake() {
        var poolables = GetComponents<IPoolable>();
        if (poolables.Length != 1) {
            Debug.LogError($"[BasicPoolable.Awake] {gameObject.name} has {poolables.Length} IPoolable!!");
        }
    }

    public virtual void Initialize() {
        gameObject.SetActive(true);
    }

    public virtual void Release() {
        gameObject.SetActive(false);
        transform.SetParent(PoolManager.PoolParent);
    }
}
