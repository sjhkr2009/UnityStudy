using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemBehavior : MonoBehaviour, IPoolHandler {
    [SerializeField] private DropItemIndex itemIndex;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(Define.Tag.Player)) return;

        Debugger.Log("아이템 획득");
        GameManager.Controller.GainExp(itemIndex.GetItemExp());
        PoolManager.Abandon(gameObject);
    }


    public void OnInitialize() {
        DropItemManager.SpawnedDropItems.Add(this);
    }

    public void OnRelease() {
        DropItemManager.SpawnedDropItems.Remove(this);
    }
}
