using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class EnemyController : EnemyControllerBase, IRepositionTarget {
    public Rigidbody2D target;

    public override void OnInitialize() {
        base.OnInitialize();
        SetTarget();
    }

    void SetTarget() {
        if (moveStrategy is ITargetTracker tracker) {
            if (!target) target = GlobalCachedData.Player.GetComponent<Rigidbody2D>();
            tracker.SetTarget(target);
        }
    }

    public virtual void Reposition(Transform pivotTransform) {
        if (StatusHandler.IsDead) return;
        
        var playerDir = GlobalCachedData.Player.GetStatusHandler.InputVector;
        
        var randomVector = CustomUtility.GetRandomVector(-3f, 3f);
        var moveDelta = (playerDir * Define.EnvironmentSetting.TileMapSize) + randomVector;
        
        transform.Translate(moveDelta);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(Define.Tag.Projectile)) return;

        StatusHandler.Hp -= other.GetComponent<Projectile>().damage;
        if (StatusHandler.Hp <= 0) Dead();
    }

    void Dead() {
        StatusHandler.IsDead = true;
        PoolManager.Abandon(gameObject);
    }
}
