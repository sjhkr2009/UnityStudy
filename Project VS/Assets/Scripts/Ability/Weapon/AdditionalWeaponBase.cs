using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdditionalWeaponBase : AbilityBase, IWeaponAbility {
    public virtual Transform Transform { get; set; }
    public virtual void OnEveryFrame(float deltaTime) { }

    public abstract void OnHitAnyEnemy(DamageData data, EnemyStatus status);

    public override void Initialize(AbilityController controller) {
        base.Initialize(controller);
        GameBroadcaster.OnHitEnemy += OnHitAnyEnemy;
    }

    public override void Abandon() {
        base.Abandon();
        GameBroadcaster.OnHitEnemy -= OnHitAnyEnemy;
    }
}
