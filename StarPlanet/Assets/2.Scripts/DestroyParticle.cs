using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DestroyParticle : MonoBehaviour
{
    [BoxGroup("Particle Setting"), SerializeField] private float particleDuration = 1f;

    void OnEnable()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
        {
            gameObject.SetActive(false);
            return;
        }
        Invoke(nameof(DurationOut), particleDuration);
    }
    void DurationOut() { gameObject.SetActive(false); }
}
