using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class TitleMenu : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] GameObject allPopUpWindow;
    [SerializeField] RectTransform popUpBasicTransform;
    [SerializeField] Text titleText;
    [Header("Window")]
    [SerializeField] RectTransform soundWindowTransform;

    bool isPopUpClosing = false;


    private NowActiveWindow _nowActive;
    public NowActiveWindow nowActive
    {
        get => _nowActive;
        set
        {
            switch (value)
            {
                case NowActiveWindow.None:
                    _nowActive = NowActiveWindow.None;
                    break;

                case NowActiveWindow.Pause:
                    _nowActive = NowActiveWindow.Pause;
                    break;

                case NowActiveWindow.Sound:
                    _nowActive = NowActiveWindow.Sound;
                    if (!allPopUpWindow.activeSelf) allPopUpWindow.SetActive(true);
                    titleText.text = "SOUND";
                    soundWindowTransform.gameObject.SetActive(true);
                    break;

                case NowActiveWindow.Gameover:
                    _nowActive = NowActiveWindow.Gameover;
                    break;
            }
        }
    }

    private void Start()
    {
        if (allPopUpWindow.activeSelf) OffAllWindow();
    }


    public void ButtonToPlay()
    {
        SceneManager.LoadScene("Play");
    }

    public void ButtonToOpenSoundMenu()
    {
        if (isPopUpClosing) return;

        nowActive = NowActiveWindow.Sound;
        PopUpWindowOnAnimation(soundWindowTransform);
    }

    public void ButtonToOffAllWindow()
    {
        if (isPopUpClosing) return;

        switch (nowActive)
        {
            case NowActiveWindow.None:
                if (allPopUpWindow.activeSelf) allPopUpWindow.SetActive(false);
                break;
            case NowActiveWindow.Sound:
                PopUpWindowOffAnimation(soundWindowTransform);
                break;
            default:
                if (allPopUpWindow.activeSelf) allPopUpWindow.SetActive(false);
                break;
        }
    }

    void PopUpWindowOnAnimation(RectTransform windowScale)
    {
        popUpBasicTransform.localScale = Vector3.one * 0.5f;
        windowScale.localScale = Vector3.one * 0.5f;

        popUpBasicTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
        windowScale.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    void PopUpWindowOffAnimation(RectTransform windowScale)
    {
        isPopUpClosing = true;
        windowScale.DOScale(0f, 0.15f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() => { windowScale.gameObject.SetActive(false); });
        popUpBasicTransform.DOScale(0f, 0.15f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(OffAllWindow);
    }

    void OffAllWindow()
    {
        allPopUpWindow.SetActive(false);
        isPopUpClosing = false;
    }
}
