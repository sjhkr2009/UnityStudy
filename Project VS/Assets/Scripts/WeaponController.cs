using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    public EnemyScanner EnemyScanner { get; private set; }
    private List<WeaponBase> Weapons { get; } = new List<WeaponBase>();

    private void Awake() {
        EnemyScanner = GetComponent<EnemyScanner>();
    }

    [Button]
    public void AddOnEditor() {
        if (Weapons.Count == 0) AddWeapon<SpinnerWeapon>();
        else if (Weapons.Count == 1) AddWeapon<FireBulletWeapon>();
    }

    private T CreateWeapon<T>() where T : WeaponBase {
        var weaponTr = new GameObject(typeof(T).Name).transform;
        weaponTr.SetParent(transform);
        weaponTr.ResetTransform();

        var weapon = weaponTr.AddComponent<T>();
        return weapon;
    }

    private WeaponBase LoadWeapon(string prefabName) {
        var weapon = PoolManager.Get<WeaponBase>(prefabName, transform);
        if (!weapon) {
            Debugger.Error($"[WeaponController.AddWeapon] Cannot Find Weapon: {prefabName}");
        }
        return weapon;
    }

    public void AddWeapon<T>() where T : WeaponBase {
        var weapon = CreateWeapon<T>();

        weapon.Initialize(this);
        Weapons.Add(weapon);
    }

    public bool AddWeapon(string prefabName) {
        var weapon = LoadWeapon(prefabName);
        if (!weapon) return false;
        
        weapon.Initialize(this);
        Weapons.Add(weapon);
        return true;
    }
    
    public bool RemoveWeapon<T>() where T : WeaponBase {
        var target = Weapons.FirstOrDefault(w => w is T);
        if (!target) {
            Debugger.Error($"[WeaponController.RemoveWeapon] Cannot Find: {typeof(T).Name}");
            return false;
        }
        
        target.Abandon();
        Weapons.Remove(target);
        return true;
    }

    private void Update() {
        var deltaTime = Time.deltaTime;
        Weapons.ForEach(w => w.OnUpdate(deltaTime));
    }
}
