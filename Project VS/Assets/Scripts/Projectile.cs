using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageGetter {
    public float Damage { get; set; }

    public virtual void Initialize(ProjectileParam param) {
        Damage = param.damage;
    }
}
