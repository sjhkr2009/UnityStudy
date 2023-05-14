using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : ItemBase {
    public abstract Transform Transform { get; set; }
    public virtual float Damage { get; set; }
    public virtual float AttackSpeed { get; set; }
    public virtual float AttackRange { get; set; }
    public virtual int AttackCount { get; set; }
}
