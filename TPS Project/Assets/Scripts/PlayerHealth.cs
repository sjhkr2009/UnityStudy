using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator animator;
    private AudioSource playerAudioPlayer;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemPickupClip;

    private void Awake()
    {
        playerAudioPlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateUI();
    }
    
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateHealthText(IsDead ? 0f : Health);
    }
    
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) 
            return false;

        EffectManager.Instance.PlayHitEffect(
            damageMessage.hitPoint,
            damageMessage.hitNormal,
            transform,
            EffectManager.EffectType.Flesh
        );

        playerAudioPlayer.PlayOneShot(hitClip);
        UpdateUI();
        
        return true;
    }
    
    public override void Die()
    {
        base.Die();

        playerAudioPlayer.PlayOneShot(deathClip);
        animator.SetTrigger("Die");
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsDead)
            return;

        IItem item = other.GetComponent<IItem>();
        if (item != null)
        {
            item.Use(gameObject);
            playerAudioPlayer.PlayOneShot(itemPickupClip);
        }
    }
}