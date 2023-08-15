using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/** 콜라이더 범위를 일정 주기로 공격하는 오브젝트. 편의상 투사체로 간주하며 lifeTime에 의해서만 소멸된다. */
public class PeriodicAttackProjectile : Projectile {
    private float delayTime;
    private float attackDuration;
    private float attackInterval;
    private float lifeTime;
    private float elapsedTimeFromSpawned;
    private bool isActive;

    private HashSet<GameObject> CheckedObjects { get; } = new HashSet<GameObject>();


    /// <param name="speed">value means start attack delay</param>
    public override void Initialize(ProjectileParam param) {
        base.Initialize(param);
        
        transform.position = param.startPoint;
        transform.localScale = Vector3.one * param.range;
        lifeTime = param.lifeTime;
        delayTime = param.speed;
        attackDuration = param.attackDurationTime;
        attackInterval = param.attackInterval;

        if (Mathf.Approximately(delayTime, 0f)) isActive = true;

        if (lifeTime <= 0f) {
            Debugger.Error($"[AttackOnceProjectile.Initialize] Param is invalid! (LifeTime: {lifeTime})");
            lifeTime = 10f;
        }
    }

    public override bool IsValidTarget(GameObject target) {
        if (!isActive) return false;
        if (CheckedObjects.Contains(target)) return false;
        
        var isValid = base.IsValidTarget(target);
        if (isValid) {
            CheckedObjects.Add(target);
            if (attackInterval > 0f) {
                DOVirtual.DelayedCall(attackInterval, () => {
                    if (isActive) CheckedObjects.Remove(target);
                });
            }
        }
        return isValid;
    }
    
    protected override void OnTriggerEnter2D(Collider2D other) { }

    private void OnTriggerStay2D(Collider2D other) {
        if (!IsValidTarget(other.gameObject)) return;

        var damageHandler = other.GetComponent<IDamagableEntity>();
        var attackPos = other.bounds.ClosestPoint(transform.position);
        damageHandler?.OnAttacked(this, attackPos);
        OnAttack?.Invoke(attackPos);
    }

    private void Update() {
        if (GameManager.IsPause) return;

        elapsedTimeFromSpawned += Time.deltaTime;
        // 시작 딜레이가 지나고 활성화되며, 시작 딜레이로부터 지속시간만큼 지나고 비활성화된다
        isActive = delayTime <= elapsedTimeFromSpawned && elapsedTimeFromSpawned <= (delayTime + attackDuration);
        
        if (elapsedTimeFromSpawned > lifeTime) {
            PoolManager.Abandon(gameObject);
        }
    }

    public override void OnInitialize() {
        base.OnInitialize();
        elapsedTimeFromSpawned = 0f;
        isActive = false;
    }

    public override void OnRelease() {
        isActive = false;
        CheckedObjects.Clear();
        base.OnRelease();
    }
}
