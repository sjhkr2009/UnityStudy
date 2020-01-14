using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleManager : MonoBehaviour
{
    

    public void DestroyParticle(ParticleSystem particle)
    {
        StartCoroutine(ParticlePlay(particle));
    }

    public void DestroyParticle(ParticleSystem particle, AudioSource audio)
    {
        StartCoroutine(ParticlePlay(particle, audio));
    }

    IEnumerator ParticlePlay(ParticleSystem particle)
    {
        particle.Play();
        yield return new WaitForSeconds(particle.main.duration);

        particle.gameObject.SetActive(false);
    }

    IEnumerator ParticlePlay(ParticleSystem particle, AudioSource audio)
    {
        particle.Play();
        audio.Play();
        yield return new WaitForSeconds(particle.main.duration);

        particle.gameObject.SetActive(false);
    }

    IEnumerator ParticlePlay(ParticleSystem particle, float duration)
    {
        particle.Play();
        yield return new WaitForSeconds(duration);

        particle.gameObject.SetActive(false);
    }

    IEnumerator ParticlePlay(ParticleSystem particle, AudioSource audio, float duration)
    {
        particle.Play();
        audio.Play();
        yield return new WaitForSeconds(duration);

        particle.gameObject.SetActive(false);
    }

}
