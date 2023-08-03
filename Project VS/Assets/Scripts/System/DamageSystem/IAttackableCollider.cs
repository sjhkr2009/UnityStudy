using UnityEngine;

public interface IAttackableCollider {
    public Collider2D GetCollider { get; }
    public DamageData GetDamageData();
}
