using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public enum SoundTypeFX { CorrectCol, WrongCol, NormalBomb, HexagonBomb, Healing, Booster, ButtonClickLong, ButtonClickNormal, ButtonClickShort }

public class SoundManager : MonoBehaviour
{
    public float MasterVolume
    {
        get => PlayerPrefs.GetFloat(nameof(MasterVolume), 1.0f);
        set => PlayerPrefs.SetFloat(nameof(MasterVolume), value);
    }
    public float FXVolume
    {
        get => PlayerPrefs.GetFloat(nameof(FXVolume), 0.5f);
        set => PlayerPrefs.SetFloat(nameof(FXVolume), value);
    }
    public float BGMVolume
    {
        get => PlayerPrefs.GetFloat(nameof(BGMVolume), 0.5f);
        set => PlayerPrefs.SetFloat(nameof(BGMVolume), value);
    }

    [BoxGroup("BGM")] [SerializeField] AudioSource BGMPlayer;

    [BoxGroup("FX")] [SerializeField] AudioSource testFXPlayer;
    [BoxGroup("FX")] [SerializeField] AudioClip testFXSound;
    [Header("Sound Sources")]
    [BoxGroup("FX")] [SerializeField] AudioClip correctCollision;
    [BoxGroup("FX")] [SerializeField] AudioClip wrongCollision;
    [BoxGroup("FX")] [SerializeField] AudioClip normalBombExplosion;
    [BoxGroup("FX")] [SerializeField] AudioClip hexagonBombExplosion;
    [BoxGroup("FX")] [SerializeField] AudioClip healing;
    [BoxGroup("FX")] [SerializeField] AudioClip booster;
    [BoxGroup("FX")] [SerializeField] AudioClip buttonClickLong;
    [BoxGroup("FX")] [SerializeField] AudioClip buttonClickNormal;
    [BoxGroup("FX")] [SerializeField] AudioClip buttonClickShort;

    [BoxGroup("Info"), SerializeField, ReadOnly] float masterVolume;
    [BoxGroup("Info"), SerializeField, ReadOnly] float fxVolume;
    [BoxGroup("Info"), SerializeField, ReadOnly] float bgmVolume;

    List<AudioSource> audioFXPlayers = new List<AudioSource>();

    private void Start()
    {
        audioFXPlayers = GameManager.Instance.PoolManager.audioFXList;

        masterVolume = MasterVolume;
        bgmVolume = BGMVolume;
        fxVolume = FXVolume;
        PlayBGM();
    }

    void PlayBGM()
    {
        BGMPlayer.volume = MasterVolume * BGMVolume;
        BGMPlayer.Play();
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
                audio.PlayOneShot(healing, 0.7f * MasterVolume * FXVolume);
                break;
            case SoundTypeFX.Booster:
                audio.PlayOneShot(booster, 0.4f * MasterVolume * FXVolume);
                break;
            case SoundTypeFX.ButtonClickLong:
                audio.PlayOneShot(buttonClickLong, 0.7f * MasterVolume * FXVolume);
                break;
            case SoundTypeFX.ButtonClickNormal:
                audio.PlayOneShot(buttonClickNormal, 0.8f * MasterVolume * FXVolume);
                break;
            case SoundTypeFX.ButtonClickShort:
                audio.PlayOneShot(buttonClickShort, 0.5f * MasterVolume * FXVolume);
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
    public void MasterVolumeChange(Slider volumeSlider)
    {
        MasterVolume = volumeSlider.value;
        masterVolume = PlayerPrefs.GetFloat(nameof(MasterVolume)); //모니터링용
        BGMPlayer.volume = MasterVolume * BGMVolume;
    } 
    public void BGMVolumeChange(Slider volumeSlider)
    {
        BGMVolume = volumeSlider.value;
        bgmVolume = PlayerPrefs.GetFloat(nameof(BGMVolume));
        BGMPlayer.volume = MasterVolume * BGMVolume;
    }
    public void FXVolumeChange(Slider volumeSlider)
    {
        FXVolume = volumeSlider.value;
        fxVolume = PlayerPrefs.GetFloat(nameof(FXVolume));
    }

}
