using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleDurationSetting : MonoBehaviour
{
    [BoxGroup("Particle Duration"), SerializeField] private float particleDuration = 1f;

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

    private void OnDestroy()
    {
        CancelInvoke();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
