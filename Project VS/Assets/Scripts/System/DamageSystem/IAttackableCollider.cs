using UnityEngine;

public interface IAttackableCollider {
    public Transform transform { get; }
    public AbilityBase ParentAbility { get; }
    public float Damage { get; }
}
