using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleManager : MonoBehaviour
{
    //GameManager.Instance.particleManager를 각 파티클에서 불러옴
    
    public IEnumerator ParticlePlay(ParticleSystem particle)
    {
        particle.Play();
        yield return new WaitForSeconds(particle.main.duration);

        particle.gameObject.SetActive(false);
    }

    public IEnumerator ParticlePlay(ParticleSystem particle, AudioSource audio)
    {
        particle.Play();
        audio.Play();
        yield return new WaitForSeconds(particle.main.duration);

        particle.gameObject.SetActive(false);
    }

    public IEnumerator ParticlePlay(ParticleSystem particle, float duration)
    {
        particle.Play();
        yield return new WaitForSeconds(duration);

        particle.gameObject.SetActive(false);
    }

    public IEnumerator ParticlePlay(ParticleSystem particle, AudioSource audio, float duration)
    {
        particle.Play();
        audio.Play();
        yield return new WaitForSeconds(duration);

        particle.gameObject.SetActive(false);
    }

}
