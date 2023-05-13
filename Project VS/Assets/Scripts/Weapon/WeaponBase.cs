using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase {
    public abstract ItemIndex Index { get; }
    /** 같은 Index 내에서 무기의 구분이 필요할 때 사용한다. */
    public virtual int InternalType { get; set; } = 0;
    public abstract Transform Transform { get; set; }
    
    public virtual int Level { get; set; }
    public virtual float Damage { get; set; }
    public virtual float AttackSpeed { get; set; }
    public virtual float AttackRange { get; set; }
    public virtual int AttackCount { get; set; }
    
    public abstract void Initialize(WeaponController controller);
    public abstract void OnUpdate(float deltaTime);
    public abstract void Upgrade();
    public abstract void Abandon();
    public virtual void OnAddOrUpgradeOtherWeapon(WeaponBase changedWeapon){}

    public virtual bool IsEqual(WeaponBase weapon) {
        return Index == weapon.Index && InternalType == weapon.InternalType;
    }
}
