using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SoundTypeFX { CorrectCol, WrongCol, NormalBomb, HexagonBomb, Healing }

public class SoundManager : MonoBehaviour
{
    //[BoxGroup("BGM")] [SerializeField] AudioSource audioSource;

    [BoxGroup("FX")] [SerializeField] AudioClip correctCollision;
    [BoxGroup("FX")] [SerializeField] AudioClip wrongCollision;
    [BoxGroup("FX")] [SerializeField] AudioClip normalBombExplosion;
    [BoxGroup("FX")] [SerializeField] AudioClip hexagonBombExplosion;
    [BoxGroup("FX")] [SerializeField] AudioClip healing;

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
                audio.PlayOneShot(correctCollision, 0.85f);
                break;
            case SoundTypeFX.WrongCol:
                audio.PlayOneShot(wrongCollision, 0.3f);
                break;
            case SoundTypeFX.NormalBomb:
                audio.PlayOneShot(normalBombExplosion, 0.5f);
                break;
            case SoundTypeFX.HexagonBomb:
                audio.PlayOneShot(hexagonBombExplosion, 0.35f);
                break;
            case SoundTypeFX.Healing:
                audio.PlayOneShot(healing, 0.5f);
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
                Debug.Log($"{i}번째 오디오 활성화");
                break;
            }
        }
        if(!container.gameObject.activeSelf) container.gameObject.SetActive(true);
        return container;
    }
}
