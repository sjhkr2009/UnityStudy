using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : ItemBase {
    public abstract Transform Transform { get; set; }
    public virtual float Damage { get; set; }
    public virtual float AttackInterval { get; set; }
    public virtual float ObjectSpeed { get; set; }
    public virtual float AttackRange { get; set; }
    public virtual int AttackCount { get; set; }
}
