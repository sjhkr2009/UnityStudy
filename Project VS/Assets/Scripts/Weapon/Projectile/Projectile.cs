using UnityEngine;

public class Projectile : GameListenerBehavior, IAttackableCollider, IPoolHandler {
    [TagSelector] public string targetTag;
    
    public float Damage { get; set; }
    
    public virtual bool IsValidTarget(GameObject target) {
        if (string.IsNullOrEmpty(targetTag)) {
            Debugger.Error("[Projectile.IsValidTarget] Target Tag is null!!");
            return false;
        }
        return target.CompareTag(targetTag);
    }

    public virtual void Initialize(ProjectileParam param) {
        Damage = param.damage;
    }

    public void OnInitialize() {
        transform.ResetTransform();
    }

    public void OnRelease() { }
}
