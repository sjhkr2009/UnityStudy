using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SoundTypeFX { CorrectCol, WrongCol, NormalBomb, HexagonBomb }

public class SoundManager : MonoBehaviour
{
    //[BoxGroup("BGM")] [SerializeField] AudioSource audioSource;

    [BoxGroup("FX")] [SerializeField] AudioClip correctCollision;
    [BoxGroup("FX")] [SerializeField] AudioClip wrongCollision;
    [BoxGroup("FX")] [SerializeField] AudioClip normalBombExplosion;
    [BoxGroup("FX")] [SerializeField] AudioClip hexagonBombExplosion;

    List<AudioSource> audioFXPlayers = new List<AudioSource>();


    private void Start()
    {
        audioFXPlayers = GameManager.Instance.PoolManager.audioFXList;
    }

    public void PlayFXSound(SoundTypeFX soundType)
    {
        AudioSource audio = FindAudioFXPlayer();

        switch (soundType)
        {
            case SoundTypeFX.CorrectCol:
                audio.PlayOneShot(correctCollision, 0.7f);
                break;
            case SoundTypeFX.WrongCol:
                audio.PlayOneShot(wrongCollision, 0.4f);
                break;
            case SoundTypeFX.NormalBomb:
                audio.PlayOneShot(normalBombExplosion, 0.4f);
                break;
            case SoundTypeFX.HexagonBomb:
                audio.PlayOneShot(hexagonBombExplosion, 0.5f);
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
