using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DestroyParticle : MonoBehaviour
{
    ParticleManager particleManager;
    [BoxGroup("Particle Setting")] [SerializeField] ParticleSystem particleEffect;
    [BoxGroup("Particle Setting")] [SerializeField] bool hasAudio;
    [BoxGroup("Particle Setting")] [ShowIf(nameof(hasAudio))] [SerializeField] AudioClip audioOnSuccess;
    [BoxGroup("Particle Setting")] [ShowIf(nameof(hasAudio))] [SerializeField] AudioClip audioOnFail;

    [SerializeField] AudioSource audioSource;

    float durationTime = 0f;

    void OnEnable()
    {
        durationTime = 0f;
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            particleManager = GameManager.Instance.particleManager;
            particleManager.DestroyParticle(particleEffect);
        }
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            durationTime += Time.deltaTime;
        }
        if(durationTime > 10f)
        {
            gameObject.SetActive(false);
        }
    }
}
