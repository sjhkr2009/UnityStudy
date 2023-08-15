using DG.Tweening;
using UnityEngine;

public class EffectHandler : MonoBehaviour {
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Animator animator;
    [SerializeField] private float destroyTime = 3f;
    private static readonly int Play = Animator.StringToHash("play");

    public static EffectHandler Create(string effectName, Vector2 position) {
        var effect = PoolManager.Get<EffectHandler>(effectName);
        if (effect) {
            effect.transform.position = position;
        } else {
            Debugger.Error($"[EffectHandler.Create] Cannot find effect: {effectName}");
        }

        return effect;
    }

    private void OnEnable() {
        if (particle) particle.Play();
        if (animator) animator.SetTrigger(Play);
        
        DOVirtual.DelayedCall(destroyTime, () => PoolManager.Abandon(gameObject));
    }
}
