using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponCollider {
    public bool IsValidTarget(GameObject target);
    public float Damage { get; }
}
