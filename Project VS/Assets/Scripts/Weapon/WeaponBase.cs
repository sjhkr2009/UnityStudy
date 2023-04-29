using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase {
    public abstract WeaponIndex Index { get; }
    public abstract Transform Transform { get; set; }
    
    public virtual int Level { get; set; }
    public virtual float Damage { get; set; }
    public virtual float AttackSpeed { get; set; }
    public virtual float AttackRange { get; set; }
    public virtual int AttackCount { get; set; }
    
    public abstract void Initialize(WeaponController controller);
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnUpgrade();
    public abstract void Abandon();
}
