using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class Projectile : MonoBehaviour, IAttackableCollider {
    [TagSelector] public string targetTag;
    
    public virtual bool IsValidTarget(GameObject target) {
        if (string.IsNullOrEmpty(targetTag)) {
            Debugger.Error("[Projectile.IsValidTarget] Target Tag is null!!");
            return false;
        }
        return target.CompareTag(targetTag);
    }

    public float Damage { get; set; }

    public virtual void Initialize(ProjectileParam param) {
        Damage = param.damage;
    }
}
