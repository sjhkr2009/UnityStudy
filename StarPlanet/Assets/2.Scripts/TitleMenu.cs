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
    [SerializeField] PopUpWindow popUpWindow;
    [SerializeField] RectTransform popUpBasicTransform;
    [SerializeField] Image popUpBackgroundColor;
    [SerializeField] Text titleText;
    [Header("Window")]
    [SerializeField] RectTransform soundWindowTransform;
    [SerializeField] GameObject warningWindow;

    bool isPopUpClosing = false;
    bool isWarningActive = false;

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

        popUpWindow.EventOnPopUpOpen += OnPopUp;
    }
    private void OnDestroy()
    {
        popUpWindow.EventOnPopUpOpen -= OnPopUp;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Escape();
    }

    public void Escape()
    {
        switch (nowActive)
        {
            case NowActiveWindow.None:
                break;
            case NowActiveWindow.Sound:
                ButtonToOffAllWindow();
                break;
            default:
                break;
        }
    }

    void OnPopUp() //팝업창을 처음 열 때 공통적으로 처리할 부분. UI를 제외한 게임화면을 50% 검게 처리하고 경고창이 활성화되어 있다면 경고창을 꺼 준다. 
    {
        warningWindow.SetActive(false);
        isWarningActive = false;

        popUpBackgroundColor.color = Color.clear;
        popUpBackgroundColor.DOColor(new Color(0, 0, 0, 0.5f), 0.3f).SetUpdate(true);

        if (!popUpBasicTransform.gameObject.activeSelf) popUpBasicTransform.gameObject.SetActive(true);
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
                nowActive = NowActiveWindow.None;
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
        nowActive = NowActiveWindow.None;
        isPopUpClosing = false;
    }
}
