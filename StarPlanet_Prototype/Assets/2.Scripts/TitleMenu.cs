using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum WarningMode { Quit, Reset }

public class TitleMenu : MonoBehaviour
{
    [SerializeField] RectTransform titleImage;
    [SerializeField] GameObject mainMenu;
    [SerializeField] Text bestScoreText; 
    [Header("Basic")]
    [SerializeField] GameObject allPopUpWindow;
    [SerializeField] PopUpWindow popUpWindow;
    [SerializeField] RectTransform popUpBasicTransform;
    [SerializeField] Image popUpBackgroundColor;
    [SerializeField] Text titleText;
    [SerializeField] RectTransform popUpTextTransform;
    [SerializeField] Text popUpText;
    [Header("Window")]
    [SerializeField] RectTransform soundWindowTransform;
    [SerializeField] RectTransform warningWindow;
    [SerializeField] RectTransform[] tutorialWindows;
    [SerializeField] Text warningText;

    Vector3 popUpTextOriginPos;
    bool isPopUpClosing = false;
    bool notBackground = false;
    WarningMode _warningMode;

    WarningMode warningMode
    {
        get => _warningMode;
        set
        {
            switch (value)
            {
                case WarningMode.Quit:
                    _warningMode = WarningMode.Quit;
                    warningText.text = "게임을 종료하시겠습니까?";
                    break;
                case WarningMode.Reset:
                    _warningMode = WarningMode.Reset;
                    warningText.text = "모든 데이터가 초기화됩니다. 계속하시겠습니까?";
                    break;
            }
        }
    }

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

                    notBackground = false;
                    if (allPopUpWindow.activeSelf) allPopUpWindow.SetActive(false);
                    break;

                case NowActiveWindow.Sound:
                    _nowActive = NowActiveWindow.Sound;

                    notBackground = false;
                    if(warningWindow.gameObject.activeSelf) warningWindow.gameObject.SetActive(false);
                    foreach (var item in tutorialWindows) item.gameObject.SetActive(false);

                    if (!allPopUpWindow.activeSelf) allPopUpWindow.SetActive(true);
                    titleText.text = "SOUND";
                    soundWindowTransform.gameObject.SetActive(true);
                    break;

                case NowActiveWindow.Warning:
                    _nowActive = NowActiveWindow.Warning;

                    notBackground = true;
                    if (!allPopUpWindow.activeSelf) allPopUpWindow.SetActive(true);
                    if (soundWindowTransform.gameObject.activeSelf) soundWindowTransform.gameObject.SetActive(false);
                    foreach (var item in tutorialWindows) item.gameObject.SetActive(false);

                    if (!warningWindow.gameObject.activeSelf) warningWindow.gameObject.SetActive(true);
                    break;

                case NowActiveWindow.Tutorial:
                    _nowActive = NowActiveWindow.Tutorial;

                    notBackground = true;
                    if (!allPopUpWindow.activeSelf) allPopUpWindow.SetActive(true);
                    if (warningWindow.gameObject.activeSelf) warningWindow.gameObject.SetActive(false);
                    if (soundWindowTransform.gameObject.activeSelf) soundWindowTransform.gameObject.SetActive(false);

                    foreach (var item in tutorialWindows) item.gameObject.SetActive(false);
                    tutorialWindows[0].gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void Awake()
    {
        popUpTextOriginPos = popUpTextTransform.position;
        Debug.Log($"팝업 메시지 위치: {popUpTextOriginPos}");

        titleImage.localScale = Vector3.zero;
        mainMenu.SetActive(false);
        titleImage.DOScale(1f, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.3f, () => { mainMenu.SetActive(true); });
        });
    }

    private void Start()
    {
        if (allPopUpWindow.activeSelf) OffAllWindow();
        bestScoreText.text = $"Best Record: {PlayerPrefs.GetInt("BestScore", 0)}";

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
                ButtonToQuitWarning();
                break;
            case NowActiveWindow.Sound:
                ButtonToOffAllWindow();
                break;
            case NowActiveWindow.Warning:
                ButtonToOffAllWindow();
                break;
            case NowActiveWindow.Tutorial:
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

        if (notBackground)
        {
            popUpBasicTransform.gameObject.SetActive(false);
        }
        else if (!notBackground)
        {
            popUpBasicTransform.gameObject.SetActive(true);
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
    public void ButtonToOpenTutorial()
    {
        if (isPopUpClosing) return;

        nowActive = NowActiveWindow.Tutorial;
        PopUpWindowOnAnimation(tutorialWindows[0]);
    }
    public void ButtonToCloseTutorial()
    {
        if (isPopUpClosing) return;
        ButtonToOffAllWindow();
    }
    public void ButtonToNextTutorial(int currentPageIndex)
    {
        tutorialWindows[currentPageIndex].gameObject.SetActive(false);
        tutorialWindows[currentPageIndex + 1].gameObject.SetActive(true);
        tutorialWindows[currentPageIndex + 1].localScale = Vector3.one;
    }
    public void ButtonToPreviousTutorial(int currentPageIndex)
    {
        tutorialWindows[currentPageIndex].gameObject.SetActive(false);
        tutorialWindows[currentPageIndex - 1].gameObject.SetActive(true);
    }
    public void ButtonToQuitWarning()
    {
        if (isPopUpClosing) return;
        warningMode = WarningMode.Quit;

        nowActive = NowActiveWindow.Warning;
        PopUpWindowOnAnimation(warningWindow);
    }
    public void ButtonToResetWarning()
    {
        if (isPopUpClosing) return;
        warningMode = WarningMode.Reset;

        nowActive = NowActiveWindow.Warning;
        PopUpWindowOnAnimation(warningWindow);
    }
    public void ButtonWarningToYes()
    {
        switch (warningMode)
        {
            case WarningMode.Quit:
                WarningToQuit();
                break;
            case WarningMode.Reset:
                WarningToReset();
                break;
        }
    }
    void WarningToQuit()
    {
        Application.Quit();
    }
    void WarningToReset()
    {
        if (isPopUpClosing) return;
        PlayerPrefs.DeleteAll();
        bestScoreText.text = $"Best Record: {PlayerPrefs.GetInt("BestScore", 0)}";

        ButtonToOffAllWindow();

        PopUpText("데이터가 삭제되었습니다");
    }
    public void ButtonWarningClose()
    {
        if (isPopUpClosing) return;
        ButtonToOffAllWindow();
    }
    public void PopUpText(string message)
    {
        popUpText.gameObject.SetActive(true);
        popUpText.DOFade(0.75f, 0f);
        popUpText.text = message;
        popUpTextTransform.position = popUpTextOriginPos;

        popUpTextTransform.DOMoveY(popUpTextOriginPos.y * 1.2f, 2f).SetEase(Ease.Linear);
        popUpText.DOFade(0f, 2f).OnComplete(() => { popUpText.gameObject.SetActive(false); });
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
            case NowActiveWindow.Warning:
                PopUpWindowOffAnimation(warningWindow);
                break;
            case NowActiveWindow.Tutorial:
                foreach (var page in tutorialWindows)
                {
                    if (page.gameObject.activeSelf)
                    {
                        PopUpWindowOffAnimation(page);
                        return;
                    }
                }
                break;
            default:
                OffAllWindow();
                break;
        }
    }

    void PopUpWindowOnAnimation(RectTransform windowScale)
    {
        if (!notBackground)
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
