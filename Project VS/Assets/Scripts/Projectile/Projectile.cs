using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : GameListenerBehaviour, IAttackableCollider, IPoolHandler {
    [TagSelector] public string targetTag;
    [SerializeField] private GameObject destroyParticle;
    
    public float Damage { get; set; }
    public AbilityBase ParentAbility { get; protected set; }
    public Action<Vector3> OnAttack { get; protected set; }
    protected IDamagableEntity[] IgnoreTargets { get; set; }

    public virtual bool IsValidTarget(GameObject target) {
        if (string.IsNullOrEmpty(targetTag)) {
            Debugger.Error("[Projectile.IsValidTarget] Target Tag is null!!");
            return false;
        }

        if (IgnoreTargets != null && IgnoreTargets.Any(t => t.gameObject == target)) {
            Debugger.Log($"[Projectile.IsValidTarget] Target is Ignored: {target.name}");
            return false;
        }
        
        return target.CompareTag(targetTag);
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (!IsValidTarget(other.gameObject)) return;

        var damageHandler = other.GetComponent<IDamagableEntity>();
        var attackPos = other.bounds.ClosestPoint(transform.position);
        damageHandler?.OnAttacked(this, attackPos);
        OnAttack?.Invoke(attackPos);
        ShowHitEffect();
    }

    public virtual void Initialize(ProjectileParam param) {
        Damage = param.damage;
        ParentAbility = param.ability;
        IgnoreTargets = param.ignoreTargets;
        OnAttack = param.onAttack;
        if (param.size > 0f) transform.localScale = Vector3.one * param.size;
    }

    public virtual void OnInitialize() {
        transform.ResetTransform();
        IgnoreTargets = null;
    }

    public virtual void OnRelease() {
        ParentAbility = null;
    }

    protected virtual void ShowHitEffect() {
        if (!destroyParticle) return;
        
        var eff = EffectHandler.Create(destroyParticle.name, transform.position);
        if (!eff) Instantiate(destroyParticle, transform.position, Quaternion.identity);
    }
}
