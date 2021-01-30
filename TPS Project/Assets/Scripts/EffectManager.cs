using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<EffectManager>();
            return m_Instance;
        }
    }

    public enum EffectType
    {
        Common,
        Flesh
    }
    
    public ParticleSystem commonHitEffectPrefab;
    public ParticleSystem fleshHitEffectPrefab;
    
    public void PlayHitEffect(Vector3 pos, Vector3 normal, Transform parent = null, EffectType effectType = EffectType.Common)
    {
        ParticleSystem targetPrefab;
		switch (effectType)
		{
			case EffectType.Common:
                targetPrefab = commonHitEffectPrefab;
				break;
			case EffectType.Flesh:
                targetPrefab = fleshHitEffectPrefab;
				break;
			default:
                Debug.Log("EffectManager.PlayHitEffect() : Effect Type is null.");
				return;
		}

        ParticleSystem effect = Instantiate(targetPrefab, pos, Quaternion.LookRotation(normal), parent);
        effect.Play();
	}
}