using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class ItemController : MonoBehaviour {
    public Scanner Scanner { get; private set; }
    public List<ItemBase> Items { get; } = new List<ItemBase>();

    private void Awake() {
        Scanner = GetComponent<Scanner>();
        GameManager.Item = this;
        GameManager.EnemyScanner = Scanner;
    }

    public Transform CreateDummyTransform(ItemBase item) => CreateDummyTransform(item?.GetType().Name);
    private Transform CreateDummyTransform(string dummyObjectName) {
        if (string.IsNullOrEmpty(dummyObjectName)) {
            Debugger.Error("[ItemController.CreateDummyTransform] Parameter is Empty!!");
        }
        
        var dummy = new GameObject(dummyObjectName).transform;
        dummy.SetParent(transform);
        dummy.ResetTransform();

        return dummy;
    }

    private void AddItem(ItemBase item) {
        item.Initialize(this);
        Items.Add(item);
        SendChangeItemToOther(item);
    }
    
    public bool RemoveItem<T>() where T : ItemBase {
        var target = Items.FirstOrDefault(i => i is T);
        if (target == null) {
            Debugger.Error($"[ItemController.RemoveItem] Cannot Find: {typeof(T).Name}");
            return false;
        }
        
        target.Abandon();
        Items.Remove(target);
        return true;
    }

    public T GetItem<T>() where T : ItemBase {
        return Items.FirstOrDefault(w => w is T) as T;
    }
    
    public ItemBase GetItem(ItemIndex itemIndex) {
        return Items.FirstOrDefault(w => w.Index == itemIndex);
    }

    public ItemBase AddOrUpgradeItem(ItemIndex itemIndex) {
        var item = GetItem(itemIndex);
        if (item != null) {
            UpgradeItem(item);
            return item;
        }
        
        // TODO: 모든 무기 정보를 가진 데이터를 만들어서 WeaponIndex와 구현 클래스를 연결할 것 
        item = ItemFactory.Create(itemIndex);
        AddItem(item);
        return item;
    }

    private void UpgradeItem(ItemBase item) {
        item.Upgrade();
        SendChangeItemToOther(item);
        GameManager.Controller?.CallUpdateItem(item);
    }

    private void SendChangeItemToOther(ItemBase updatedItem) {
        Items.ForEach(w => {
            if (w != updatedItem) w.OnChangeOtherItem(updatedItem);
        });
    }

    public int GetLevel(ItemIndex itemIndex) {
        return Items.FirstOrDefault(i => i.Index == itemIndex)?.Level ?? 0;
    }

    private void Update() {
        if (GameManager.IsPause) return;
        
        var deltaTime = Time.deltaTime;
        Items.ForEach(w => w.OnEveryFrame(deltaTime));
    }
}
