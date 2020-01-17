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

    List<AudioSource> audioFXPlayers = new List<AudioSource>();


    private void Start()
    {
        audioFXPlayers = GameManager.Instance.poolManager.audioFXList;
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
        AudioSource container = audioFXPlayers[0];
        for (int i = 0; i < audioFXPlayers.Count; i++)
        {
            if (!audioFXPlayers[i].isPlaying)
            {
                container = audioFXPlayers[i];
                break;
            }
        }
        if(!container.gameObject.activeSelf) container.gameObject.SetActive(true);
        return container;
    }
}
