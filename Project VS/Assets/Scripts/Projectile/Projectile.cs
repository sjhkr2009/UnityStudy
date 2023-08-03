using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GameListenerBehavior, IAttackableCollider, IPoolHandler {
    [TagSelector] public string targetTag;
    public LayerMask layerMask;
    
    public float Damage { get; set; }

    private Collider2D _collider;
    public Collider2D GetCollider => _collider;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
    }

    public virtual bool IsValidTarget(GameObject target) {
        if (string.IsNullOrEmpty(targetTag)) {
            Debugger.Error("[Projectile.IsValidTarget] Target Tag is null!!");
            return false;
        }
        return target.CompareTag(targetTag);
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (!IsValidTarget(other.gameObject)) return;

        var damageHandler = other.GetComponent<IDamagableEntity>();
        damageHandler?.OnAttacked(this);
    }

    public DamageData GetDamageData() {
        return new DamageData(Damage);
    }

    public virtual void Initialize(ProjectileParam param) {
        Damage = param.damage;
    }

    public virtual void OnInitialize() {
        transform.ResetTransform();
    }

    public virtual void OnRelease() { }
}
