using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SoundTypeFX { CorrectCol, WrongCol }

public class SoundManager : MonoBehaviour
{
    //[BoxGroup("BGM")] [SerializeField] AudioSource audioSource;

    [BoxGroup("Enemy Destroy")] [SerializeField] AudioClip correctCollision;
    [BoxGroup("Enemy Destroy")] [SerializeField] AudioClip wrongCollision;

    List<AudioSource> audioFXSources = new List<AudioSource>();

    public void MakeAudioList(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            audioFXSources.Add(gameObjects[i].GetComponent<AudioSource>());
        }
    }

    public void PlayFXSound(SoundTypeFX soundType)
    {
        AudioSource audio = FindAudioFXPlayer();

        switch (soundType)
        {
            case SoundTypeFX.CorrectCol:
                audio.clip = correctCollision;
                audio.volume = 0.7f;
                audio.Play();
                break;
            case SoundTypeFX.WrongCol:
                audio.clip = wrongCollision;
                audio.volume = 0.4f;
                audio.Play();
                break;
            
        }
    }

    AudioSource FindAudioFXPlayer()
    {
        AudioSource container = audioFXSources[0];
        for (int i = 0; i < audioFXSources.Count; i++)
        {
            if (!audioFXSources[i].isPlaying)
            {
                container = audioFXSources[i];
                break;
            }
        }
        container.gameObject.SetActive(true);
        return container;
    }
}
