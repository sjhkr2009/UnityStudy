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

    private void Update() {
        var deltaTime = Time.deltaTime;
        Weapons.ForEach(w => w.OnUpdate(deltaTime));
    }
}
