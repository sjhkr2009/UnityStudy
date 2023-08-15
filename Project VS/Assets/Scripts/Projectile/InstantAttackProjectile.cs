using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/** 직선상의 적을 즉시 히트처리하는 오브젝트. Initialize와 동시에 히트처리하며 콜라이더에 의한 동작은 하지 않는다. */
public class InstantAttackProjectile : Projectile {
    [SerializeField] private GameObject particleByDistance;
    
    private float lifeTime;
    private float elapsedTimeFromSpawned;
    
    public override void Initialize(ProjectileParam param) {
        base.Initialize(param);
        
        particleByDistance.SetActive(false);
        transform.position = param.startPoint;
        lifeTime = param.lifeTime;

        if (lifeTime <= 0f) {
            Debugger.Error($"[AttackOnceProjectile.Initialize] Param is invalid! (LifeTime: {lifeTime})");
            lifeTime = 1f;
        }
        
        DOVirtual.DelayedCall(0.01f, () => {
            transform.position = param.startPoint;
            particleByDistance.SetActive(true);
            transform.DOMove(param.startPoint + (param.direction * param.range), 0.2f);
            Physics2D.RaycastAll(param.startPoint, param.direction, param.range)
                .ForEach(h => Attack(h.collider));
        });
    }

    protected override void OnTriggerEnter2D(Collider2D other) { }

    private void Attack(Collider2D other) {
        if (!IsValidTarget(other.gameObject)) return;

        var damageHandler = other.GetComponent<IDamagableEntity>();
        var attackPos = other.bounds.ClosestPoint(transform.position);
        damageHandler?.OnAttacked(this, attackPos);
        OnAttack?.Invoke(attackPos);
    }

    private void Update() {
        if (GameManager.IsPause) return;

        elapsedTimeFromSpawned += Time.deltaTime;
        
        if (elapsedTimeFromSpawned > lifeTime) {
            PoolManager.Abandon(gameObject);
        }
    }

    public override void OnInitialize() {
        base.OnInitialize();
        elapsedTimeFromSpawned = 0f;
    }
}
