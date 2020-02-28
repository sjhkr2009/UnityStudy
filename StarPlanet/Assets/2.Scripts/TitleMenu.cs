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
    [SerializeField] RectTransform warningWindow;
    [SerializeField] Text warningText;

    bool isPopUpClosing = false;
    bool isWarningActive = false;

    private NowActiveWindow _nowActive;
    public NowActiveWindow nowActive //현재 활성화된 창의 상태. 경고창 활성화 여부 -> 팝업창 출력 -> 불필요한 메뉴 비활성화 순으로 실행한다.
    {
        get => _nowActive;
        set
        {
            switch (value)
            {
                case NowActiveWindow.None:
                    _nowActive = NowActiveWindow.None;

                    isWarningActive = false;
                    if (allPopUpWindow.activeSelf) allPopUpWindow.SetActive(false);
                    break;

                case NowActiveWindow.Sound:
                    _nowActive = NowActiveWindow.Sound;

                    isWarningActive = false;
                    if (!allPopUpWindow.activeSelf) allPopUpWindow.SetActive(true);
                    titleText.text = "SOUND";
                    soundWindowTransform.gameObject.SetActive(true);
                    break;

                case NowActiveWindow.Quit:
                    _nowActive = NowActiveWindow.Quit;

                    isWarningActive = true;
                    if (!allPopUpWindow.activeSelf) allPopUpWindow.SetActive(true);
                    if (soundWindowTransform.gameObject.activeSelf) soundWindowTransform.gameObject.SetActive(false);
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
                ButtonToWarning();
                break;
            case NowActiveWindow.Sound:
                ButtonToOffAllWindow();
                break;
            case NowActiveWindow.Quit:
                ButtonToOffAllWindow();
                break;
            default:
                break;
        }
    }

    void OnPopUp()
    {
        popUpBackgroundColor.color = Color.clear;
        popUpBackgroundColor.DOColor(new Color(0, 0, 0, 0.5f), 0.3f).SetUpdate(true);

        if (isWarningActive)
        {
            popUpBasicTransform.gameObject.SetActive(false);
            warningWindow.gameObject.SetActive(true);
        }
        else if (!isWarningActive)
        {
            popUpBasicTransform.gameObject.SetActive(true);
            warningWindow.gameObject.SetActive(false);
        }
    }

    public void ButtonToPlay()
    {
        if (isPopUpClosing) return;
        SceneManager.LoadScene("Play");
    }

    public void ButtonToOpenSoundMenu()
    {
        if (isPopUpClosing) return;

        nowActive = NowActiveWindow.Sound;
        PopUpWindowOnAnimation(soundWindowTransform);
    }
    public void ButtonToWarning()
    {
        if (isPopUpClosing) return;

        nowActive = NowActiveWindow.Quit;
        PopUpWindowOnAnimation(warningWindow);
        warningText.text = "게임을 종료하시겠습니까?";
    }
    public void ButtonWarningToQuit()
    {
        Application.Quit();
    }
    public void ButtonWarningClose()
    {
        if (isPopUpClosing) return;
        ButtonToOffAllWindow();
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
            case NowActiveWindow.Quit:
                PopUpWindowOffAnimation(warningWindow);
                break;
            default:
                OffAllWindow();
                break;
        }
    }

    void PopUpWindowOnAnimation(RectTransform windowScale)
    {
        if (!isWarningActive)
        {
            popUpBasicTransform.localScale = Vector3.one * 0.5f;
            popUpBasicTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        windowScale.localScale = Vector3.one * 0.5f;
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
