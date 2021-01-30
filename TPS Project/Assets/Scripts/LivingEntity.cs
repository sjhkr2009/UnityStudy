using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float Health { get; protected set; }
    public bool IsDead { get; protected set; }
    
    public event Action OnDeath;
    
    private const float minTimeBetDamaged = 0.1f;
    private float lastDamagedTime;

    protected bool IsInvulnerable => (Time.time < lastDamagedTime + minTimeBetDamaged);
    
    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = startingHealth;
    }

    public virtual bool ApplyDamage(DamageMessage damageMessage)
    {
        if (IsInvulnerable || damageMessage.attacker == gameObject || IsDead)
            return false;

        lastDamagedTime = Time.time;
        Health -= damageMessage.amount;
        
        if (Health <= 0) 
            Die();

        return true;
    }
    
    public virtual void RestoreHealth(float restoreValue)
    {
        if (IsDead)
            return;
        
        Health += restoreValue;
    }
    
    public virtual void Die()
    {
		OnDeath?.Invoke();

		IsDead = true;
    }
}