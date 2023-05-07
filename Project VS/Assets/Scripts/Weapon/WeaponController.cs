using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    public Scanner Scanner { get; private set; }
    private List<WeaponBase> Weapons { get; } = new List<WeaponBase>();

    private void Awake() {
        Scanner = GetComponent<Scanner>();
        GameManager.Weapon = this;
    }

    [Button]
    public void AddOnEditor() {
        if (Weapons.Count == 0) AddWeapon(new SpinnerWeapon());
        else if (Weapons.Count == 1) AddWeapon(new FireBulletWeapon());
    }

    public Transform CreateDummyTransform(WeaponBase weapon) => CreateDummyTransform(weapon?.GetType().Name);
    public Transform CreateDummyTransform(string dummyObjectName) {
        if (string.IsNullOrEmpty(dummyObjectName)) {
            Debugger.Error("[WeaponController.CreateDummyTransform] Parameter is Empty!!");
        }
        
        var dummy = new GameObject(dummyObjectName).transform;
        dummy.SetParent(transform);
        dummy.ResetTransform();

        return dummy;
    }

    public void AddWeapon(WeaponBase weapon) {
        weapon.Initialize(this);
        Weapons.Add(weapon);
        SendUpdateWeaponToOther(weapon);
    }
    
    public bool RemoveWeapon<T>() where T : WeaponBase {
        var target = Weapons.FirstOrDefault(w => w is T);
        if (target == null) {
            Debugger.Error($"[WeaponController.RemoveWeapon] Cannot Find: {typeof(T).Name}");
            return false;
        }
        
        target.Abandon();
        Weapons.Remove(target);
        return true;
    }

    public T GetWeapon<T>() where T : WeaponBase {
        return Weapons.FirstOrDefault(w => w is T) as T;
    }
    
    public WeaponBase GetWeapon(WeaponIndex weaponIndex) {
        return Weapons.FirstOrDefault(w => w.Index == weaponIndex);
    }

    public void AddOrUpgradeWeapon(WeaponIndex weaponIndex) {
        var weapon = GetWeapon(weaponIndex);
        if (weapon != null) {
            UpgradeWeapon(weapon);
            return;
        }
        
        // TODO: 모든 무기 정보를 가진 데이터를 만들어서 WeaponIndex와 구현 클래스를 연결할 것 
        weapon = weaponIndex switch {
            WeaponIndex.AutoGun => new FireBulletWeapon(),
            WeaponIndex.SpinAround => new SpinnerWeapon(),
            _ => null
        };
        AddWeapon(weapon);
    }

    private void UpgradeWeapon(WeaponBase weapon) {
        weapon.Upgrade();
        SendUpdateWeaponToOther(weapon);
    }

    private void SendUpdateWeaponToOther(WeaponBase updatedWeapon) {
        Weapons.ForEach(w => {
            if (w != updatedWeapon) w.OnAddOrUpgradeOtherWeapon(updatedWeapon);
        });
    }

    private void Update() {
        var deltaTime = Time.deltaTime;
        Weapons.ForEach(w => w.OnUpdate(deltaTime));
    }
}
