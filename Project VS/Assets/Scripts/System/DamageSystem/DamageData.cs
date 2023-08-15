using UnityEngine;

public struct DamageData {
    public IAttackableCollider attacker;
    public IDamagableEntity hitEntity;
    public AbilityBase hitAbility;
    public Vector2 attackPos;
    public Vector2 attackDirection;
    public float damage;

    public DamageData(IAttackableCollider attacker, IDamagableEntity hitEntity, Vector2 attackPos) {
        this.hitEntity = hitEntity;
        this.attackPos = attackPos;
        this.attacker = attacker;
        attackDirection = attacker.transform.up;
        this.damage = attacker.Damage;
        hitAbility = attacker.ParentAbility;
    }
}
