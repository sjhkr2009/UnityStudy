using UnityEngine;

public interface IDamagableEntity {
    public GameObject gameObject { get; }
    public AttackResult OnAttacked(IAttackableCollider attacker, Vector2 attackPos);
}
