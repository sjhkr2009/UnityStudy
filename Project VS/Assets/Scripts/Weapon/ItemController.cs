using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class ItemController : MonoBehaviour {
    public Scanner Scanner { get; private set; }
    public List<AbilityBase> Items { get; } = new List<AbilityBase>();

    private void Awake() {
        Scanner = GetComponent<Scanner>();
        GameManager.Item = this;
        GameManager.EnemyScanner = Scanner;
    }

    public Transform CreateDummyTransform(AbilityBase ability) => CreateDummyTransform(ability?.GetType().Name);
    private Transform CreateDummyTransform(string dummyObjectName) {
        if (string.IsNullOrEmpty(dummyObjectName)) {
            Debugger.Error("[ItemController.CreateDummyTransform] Parameter is Empty!!");
        }
        
        var dummy = new GameObject(dummyObjectName).transform;
        dummy.SetParent(transform);
        dummy.ResetTransform();

        return dummy;
    }

    private void AddItem(AbilityBase ability) {
        ability.Initialize(this);
        Items.Add(ability);
        SendChangeItemToOther(ability);
    }
    
    public bool RemoveItem<T>() where T : AbilityBase {
        var target = Items.FirstOrDefault(i => i is T);
        if (target == null) {
            Debugger.Error($"[ItemController.RemoveItem] Cannot Find: {typeof(T).Name}");
            return false;
        }
        
        target.Abandon();
        Items.Remove(target);
        return true;
    }

    public T GetItem<T>() where T : AbilityBase {
        return Items.FirstOrDefault(w => w is T) as T;
    }
    
    public AbilityBase GetItem(AbilityIndex abilityIndex) {
        return Items.FirstOrDefault(w => w.Index == abilityIndex);
    }

    public AbilityBase AddOrUpgradeItem(AbilityIndex abilityIndex) {
        var item = GetItem(abilityIndex);
        if (item != null) {
            UpgradeItem(item);
            return item;
        }
        
        // TODO: 모든 무기 정보를 가진 데이터를 만들어서 WeaponIndex와 구현 클래스를 연결할 것 
        item = AbilityFactory.Create(abilityIndex);
        AddItem(item);
        return item;
    }

    private void UpgradeItem(AbilityBase ability) {
        ability.Upgrade();
        SendChangeItemToOther(ability);
        GameManager.Controller?.CallUpdateItem(ability);
    }

    private void SendChangeItemToOther(AbilityBase updatedAbility) {
        Items.ForEach(w => {
            if (w != updatedAbility) w.OnChangeOtherAbility(updatedAbility);
        });
    }

    public int GetLevel(AbilityIndex abilityIndex) {
        return Items.FirstOrDefault(i => i.Index == abilityIndex)?.Level ?? 0;
    }

    private void Update() {
        if (GameManager.IsPause) return;
        
        var deltaTime = Time.deltaTime;
        Items.ForEach(w => w.OnEveryFrame(deltaTime));
    }
}
