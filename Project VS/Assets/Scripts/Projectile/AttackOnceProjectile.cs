using System.Collections.Generic;
using UnityEngine;

/** 소환된 직후 콜라이더 범위를 1회 공격하는 오브젝트. 편의상 투사체로 간주하며 lifeTime에 의해서만 소멸된다. */
public class AttackOnceProjectile : Projectile {
    private float delayTime;
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
        if (isValid) CheckedObjects.Add(target);
        return isValid;
    }

    private void Update() {
        if (GameManager.IsPause) return;

        elapsedTimeFromSpawned += Time.deltaTime;
        if (!isActive) {
            isActive = delayTime <= elapsedTimeFromSpawned;
        }
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
