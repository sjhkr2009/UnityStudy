using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackableCollider {
    public bool IsValidTarget(GameObject target);
    public float Damage { get; }
}
