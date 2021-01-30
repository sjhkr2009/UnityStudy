using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public float health = 50;

    public void Use(GameObject target)
    {
        LivingEntity livingEntity = target.GetComponent<LivingEntity>();

        if (livingEntity != null)
		{
            livingEntity.RestoreHealth(health);
            UIManager.Instance.UpdateHealthText(livingEntity.Health);
        }

        Destroy(gameObject);
    }
}