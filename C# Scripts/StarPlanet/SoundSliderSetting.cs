using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderSetting : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider bgmVolumeSlider;
    [SerializeField] Slider fxVolumeSlider;

    private void OnEnable()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat(nameof(GameManager.Instance.SoundManager.MasterVolume), 1.0f);
        bgmVolumeSlider.value = PlayerPrefs.GetFloat(nameof(GameManager.Instance.SoundManager.BGMVolume), 0.5f);
        fxVolumeSlider.value = PlayerPrefs.GetFloat(nameof(GameManager.Instance.SoundManager.FXVolume), 0.5f);
    }
}
