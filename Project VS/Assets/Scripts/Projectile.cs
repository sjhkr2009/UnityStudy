using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class Projectile : MonoBehaviour, IWeaponCollider {
    [TagSelector] public string targetTag = Define.Tag.Enemy;
    
    public virtual bool IsValidTarget(GameObject target) {
        return target.CompareTag(targetTag);
    }

    public float Damage { get; set; }

    public virtual void Initialize(ProjectileParam param) {
        Damage = param.damage;
    }
}
