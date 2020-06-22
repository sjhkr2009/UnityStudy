using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class TitleSoundMenu : MonoBehaviour
{
    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioSource fxPlayer;
    [BoxGroup("Sound Sources"), SerializeField] AudioClip testFXSound;

    [BoxGroup("Info"), SerializeField, ReadOnly] float masterVolume;
    [BoxGroup("Info"), SerializeField, ReadOnly] float bgmVolume;
    [BoxGroup("Info"), SerializeField, ReadOnly] float fxVolume;

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

    private void Start()
    {
        PlayBGM();
    }

    void PlayBGM()
    {
        bgmPlayer.volume = MasterVolume * BGMVolume;
        bgmPlayer.Play();
    }

    public void MasterVolumeChange(Slider volumeSlider)
    {
        MasterVolume = volumeSlider.value;
        masterVolume = PlayerPrefs.GetFloat(nameof(MasterVolume)); //모니터링용
        bgmPlayer.volume = MasterVolume * BGMVolume;
    }
    public void BGMVolumeChange(Slider volumeSlider)
    {
        BGMVolume = volumeSlider.value;
        bgmVolume = PlayerPrefs.GetFloat(nameof(BGMVolume)); //모니터링용
        bgmPlayer.volume = MasterVolume * BGMVolume;
    }
    public void FXVolumeChange(Slider volumeSlider)
    {
        FXVolume = volumeSlider.value;
        fxVolume = PlayerPrefs.GetFloat(nameof(FXVolume));
    }
    public void TestFXPlay() { fxPlayer.PlayOneShot(testFXSound, MasterVolume * FXVolume); }
}
