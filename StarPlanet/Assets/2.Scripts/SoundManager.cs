using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SoundTypeFX { CorrectCol, WrongCol, NormalBomb, HexagonBomb, Healing }

public class SoundManager : MonoBehaviour
{
    public float MasterVolume
    {
        get => PlayerPrefs.GetFloat(nameof(MasterVolume), 1.0f);
        set => PlayerPrefs.SetFloat(nameof(MasterVolume), value);
    }
    public float FXVolume
    {
        get => PlayerPrefs.GetFloat(nameof(FXVolume), 1.0f);
        set => PlayerPrefs.SetFloat(nameof(FXVolume), value);
    }
    public float BGMVolume
    {
        get => PlayerPrefs.GetFloat(nameof(BGMVolume), 1.0f);
        set => PlayerPrefs.SetFloat(nameof(BGMVolume), value);
    }

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
                audio.PlayOneShot(correctCollision, 0.95f * MasterVolume * FXVolume);
                break;
            case SoundTypeFX.WrongCol:
                audio.PlayOneShot(wrongCollision, 0.4f * MasterVolume * FXVolume);
                break;
            case SoundTypeFX.NormalBomb:
                audio.PlayOneShot(normalBombExplosion, 0.7f * MasterVolume * FXVolume);
                break;
            case SoundTypeFX.HexagonBomb:
                audio.PlayOneShot(hexagonBombExplosion, 0.5f * MasterVolume * FXVolume);
                break;
            case SoundTypeFX.Healing:
                audio.PlayOneShot(healing, 0.65f * MasterVolume * FXVolume);
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
