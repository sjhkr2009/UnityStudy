using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;
using DG.Tweening;
public enum NowActiveWindow { None, Pause, Sound, Gameover, Warning, Tutorial }

public class UIManager : MonoBehaviour
{
    //상태창
    [BoxGroup("Playing UI")] [SerializeField] Text countdownText;
    [BoxGroup("Playing UI")] [SerializeField] GameObject orbitalRadiusUI;
    [BoxGroup("Playing UI")] [SerializeField] Slider starHpBar;
    [BoxGroup("Playing UI")] [SerializeField] Slider planetHpBar;
    [BoxGroup("Playing UI")] [SerializeField] Text scoreText;
    [BoxGroup("Playing UI")] [SerializeField] Text bestScoreText;
    [BoxGroup("Playing UI")] [SerializeField] Text timeText;

    //가속 버튼 관련
    [BoxGroup("Button")] [SerializeField] Image accelBackground;
    [BoxGroup("Button")] [SerializeField] Color accelColorIdle;
    [BoxGroup("Button")] [SerializeField] Color accelColorActive;
    [BoxGroup("Button")] [SerializeField] GameObject accelIconIdle;
    [BoxGroup("Button")] [SerializeField] GameObject accelIconActive;

    //피버 게이지 관련
    [BoxGroup("Fever")] [SerializeField] ParticleSystem feverEffect;
    [BoxGroup("Fever")] [SerializeField] Image feverFillArea;
    [BoxGroup("Fever")] [SerializeField] Image feverEdge;

    //팝업창 관련 - 공통
    [BoxGroup("Pop-Up Window")] [SerializeField] GameObject allPopUpWindow;
    [BoxGroup("Pop-Up Window")] [SerializeField] PopUpWindow popUpWindow;
    [BoxGroup("Pop-Up Window")] [SerializeField] Text titleText;
    [BoxGroup("Pop-Up Window")] [SerializeField] Image popUpBackgroundColor;
    [BoxGroup("Pop-Up Window")] [SerializeField] RectTransform popUpBasicTransform;
    [BoxGroup("Pop-Up Window")] [SerializeField] RectTransform gameoverTransform;
    [BoxGroup("Pop-Up Window")] [SerializeField] RectTransform pauseTransform;
    [BoxGroup("Pop-Up Window")] [SerializeField] RectTransform soundTransform;
    [BoxGroup("Pop-Up Window")] [SerializeField] GameObject warningWindow;
    [BoxGroup("Pop-Up Window")] [SerializeField] WarningText warningText;
    [BoxGroup("Pop-Up Window")] [SerializeField] Text totalScoreText;
    [BoxGroup("Pop-Up Window")] [SerializeField] Text totalBestScoreText;
    [BoxGroup("Pop-Up Window")] [SerializeField] Text totalTimeText;

    [BoxGroup("Object")] [SerializeField] Star star;
    [BoxGroup("Object")] [SerializeField] Planet planet;

    ScoreManager scoreManager;
    SoundManager soundManager;

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
                    if (gameoverTransform.gameObject.activeSelf) gameoverTransform.gameObject.SetActive(false);
                    if (soundTransform.gameObject.activeSelf) soundTransform.gameObject.SetActive(false);

                    titleText.text = "OPTION";
                    pauseTransform.gameObject.SetActive(true);
                    break;

                case NowActiveWindow.Sound:
                    _nowActive = NowActiveWindow.Sound;
                    if (gameoverTransform.gameObject.activeSelf) gameoverTransform.gameObject.SetActive(false);
                    if (pauseTransform.gameObject.activeSelf) pauseTransform.gameObject.SetActive(false);

                    titleText.text = "SOUND";
                    soundTransform.gameObject.SetActive(true);
                    break;

                case NowActiveWindow.Gameover:
                    _nowActive = NowActiveWindow.Gameover;
                    if (pauseTransform.gameObject.activeSelf) pauseTransform.gameObject.SetActive(false);
                    if (soundTransform.gameObject.activeSelf) soundTransform.gameObject.SetActive(false);

                    titleText.text = "GAME OVER";
                    gameoverTransform.gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void Awake()
    {
        if (popUpWindow == null) popUpWindow = allPopUpWindow.GetComponent<PopUpWindow>();
        if (warningText == null) warningText = FindObjectOfType<WarningText>();

        star.EventRadiusChange += RadiusChange;
        star.EventHpChanged += OnPlayerHpChanged;
        star.EventMaxHpChanged += OnPlayerMaxHpChanged;

        planet.EventHpChanged += OnPlayerHpChanged;
        planet.EventMaxHpChanged += OnPlayerMaxHpChanged;

        popUpWindow.EventOnPopUpOpen += OnPopUp;
        warningText.EventOnTextEnable += WarningTextSetting;

        accelIconActive.SetActive(false);
        allPopUpWindow.SetActive(false);
        countdownText.gameObject.SetActive(false);
        FeverGaugeReset();
    }

    private void OnDestroy()
    {
        star.EventRadiusChange -= RadiusChange;
        star.EventHpChanged -= OnPlayerHpChanged;
        star.EventMaxHpChanged -= OnPlayerMaxHpChanged;

        planet.EventHpChanged -= OnPlayerHpChanged;
        planet.EventMaxHpChanged -= OnPlayerMaxHpChanged;

        popUpWindow.EventOnPopUpOpen -= OnPopUp;
        warningText.EventOnTextEnable -= WarningTextSetting;
    }

    private void Start()
    {
        scoreManager = GameManager.Instance.ScoreManager;
        soundManager = GameManager.Instance.SoundManager;
        scoreText.text = "Score";
        bestScoreText.text = "Best Record";
        timeText.text = "0.00";

    }

    void RadiusChange(float radius)
    {
        orbitalRadiusUI.transform.localScale = Vector3.one * radius * 2;
    }

    void CountdownTextChange(int count)
    {
        if(count == 0)
        {
            countdownText.text = "Start!";
            countdownText.DOFade(1f, 0f).SetUpdate(true);
            countdownText.transform.localScale = Vector3.one * 1.5f;
            DOVirtual.DelayedCall(0.75f, () => { countdownText.gameObject.SetActive(false); GameManager.Instance.gameState = GameState.Playing; });

            return;
        }
        if (!countdownText.gameObject.activeSelf) countdownText.gameObject.SetActive(true);

        countdownText.text = count.ToString();
        countdownText.transform.localScale = Vector3.one * 3f;
        countdownText.transform.DOScale(1f, 1f).SetEase(Ease.OutCirc).SetUpdate(true);
        countdownText.DOFade(1f, 0f).SetUpdate(true);
        countdownText.DOFade(0f, 1f).SetEase(Ease.InCirc).SetUpdate(true)
            .OnComplete(() =>
            {
                if(count > 0) CountdownTextChange(count - 1);
            });
    }

    // 피버타임 관련 효과
    public void FeverGaugeFill(float value)
    {
        feverFillArea.fillAmount = value;
        feverEdge.color = new Color(1f, 1f, 1f, value * 0.66f);
    }
    public void OnFeverTime()
    {
        feverEffect.gameObject.SetActive(true);
        feverEffect.Play();
        float duration = GameManager.Instance.FeverManager.feverDuration;

        feverFillArea.DOFillAmount(0f, duration * 0.8f).SetEase(Ease.Linear);
        DOVirtual.DelayedCall(duration * 0.7f, () =>
        {
            feverEdge.DOFade(0f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        });
    }
    public void FeverGaugeReset()
    {
        feverFillArea.fillAmount = 0f;
        feverEdge.DOKill();
        feverEdge.color = new Color(1f, 1f, 1f, 0f);
        feverEffect.gameObject.SetActive(false);
    }

    public void OnPlayerHpChanged(int value, Player player)
    {
        if (player.playerType == PlayerType.Star) starHpBar.value = value;
        else if (player.playerType == PlayerType.Planet) planetHpBar.value = value;
    }

    public void OnPlayerMaxHpChanged(int value, Player player)
    {
        if (player.playerType == PlayerType.Star) starHpBar.maxValue = value;
        else if (player.playerType == PlayerType.Planet) planetHpBar.maxValue = value;
    }

    public void ScoreTextChange(int value)
    {
        scoreText.text = $"{value.ToString()}";
        if(scoreManager.isCurrentScoreBest) bestScoreText.text = $"{value.ToString()}";
    }
    public void TimeTextChange(float time)
    {
        timeText.text = $"{time.ToString("0.00")}";
    }

    public void OnAccelerateClick()
    {
        accelBackground.color = accelColorActive;
        accelIconIdle.SetActive(false);
        accelIconActive.SetActive(true);

        star.Accelerate();
        GameManager.Instance.SoundManager.PlayFXSound(SoundTypeFX.Booster);
    }
    public void ExitAccelerateClick()
    {
        accelBackground.color = accelColorIdle;
        accelIconActive.SetActive(false);
        accelIconIdle.SetActive(true);

        star.CancelAccelerate();
    }

    public void OnGameOverScorePrint(int score, bool isBestScore)
    {
        totalScoreText.text = score.ToString();
        totalTimeText.text = timeText.text;

        if (isBestScore)
        {
            totalBestScoreText.text = score.ToString();
            //기록 갱신 표시
        }
        else
        {
            totalBestScoreText.text = bestScoreText.text;
        }
    }


    //팝업창 관련 설정
    public void OnGameStateChanged(GameState gameState) //게임 상태 변화에 따른 동작 설정
    {
        switch (gameState)
        {
            case GameState.Ready:
                if (allPopUpWindow.activeSelf) allPopUpWindow.SetActive(false);
                DOVirtual.DelayedCall(0.75f, () => { CountdownTextChange(3); }, true);
                break;
            case GameState.Playing:
                if (allPopUpWindow.activeSelf) allPopUpWindow.SetActive(false);
                if (countdownText.gameObject.activeSelf) countdownText.gameObject.SetActive(false);
                if(scoreManager.Score == 0)
                {
                    scoreText.text = "0";
                    bestScoreText.text = scoreManager.BestScore.ToString();
                }
                break;
            case GameState.Pause:
                allPopUpWindow.SetActive(true);
                nowActive = NowActiveWindow.Pause;
                PopUpWindowOnAnimation(pauseTransform);
                break;
            case GameState.GameOver:
                allPopUpWindow.SetActive(true);
                nowActive = NowActiveWindow.Gameover;
                PopUpWindowOnAnimation(gameoverTransform);
                break;
        }
    }

    /// <summary>
    /// 팝업창이 띄워진 상태에서 Esc 또는 스마트폰의 뒤로가기 버튼을 누를 때의 동작입니다. 현재 가장 위에 띄워져 있는 팝업창에 따라 동작을 수행합니다.
    /// 일시정지 상태: 창을 닫고 플레이 모드로 돌아갑니다.
    /// </summary>
    public void Escape()
    {
        if (isWarningActive)
        {
            isWarningActive = false;
            warningWindow.SetActive(false);
            return;
        }

        switch (nowActive)
        {
            case NowActiveWindow.None:
                break;
            case NowActiveWindow.Pause:
                ButtonPauseToResume();
                break;
            case NowActiveWindow.Sound:
                ButtonSoundToPause();
                break;
            case NowActiveWindow.Gameover:
                OpenWarningWindow();
                break;
        }
    }

    public void ButtonGameoverToTitle()
    {
        if (isPopUpClosing) return;
        soundManager.PlayFXSound(SoundTypeFX.ButtonClickLong);
        PopUpWindowOffAnimation(gameoverTransform);
        DOVirtual.DelayedCall(0.35f, GameManager.Instance.LoadTitleScene, true);
    }
    public void ButtonGameoverToRestart()
    {
        if (isPopUpClosing) return;
        soundManager.PlayFXSound(SoundTypeFX.ButtonClickLong);
        PopUpWindowOffAnimation(gameoverTransform);
        DOVirtual.DelayedCall(0.2f, GameManager.Instance.ReStartScene, true);
    }
    public void ButtonPauseToResume()
    {
        if (isPopUpClosing) return;
        soundManager.PlayFXSound(SoundTypeFX.ButtonClickNormal);
        PopUpWindowOffAnimation(pauseTransform);
        DOVirtual.DelayedCall(0.2f, () => { GameManager.Instance.gameState = GameState.Playing; }, true);
    }
    public void ButtonPauseToTitle()
    {
        if (isPopUpClosing) return;
        soundManager.PlayFXSound(SoundTypeFX.ButtonClickShort);
        OpenWarningWindow();
    }

    public void ButtonPauseToSound()
    {
        if (isPopUpClosing) return;
        soundManager.PlayFXSound(SoundTypeFX.ButtonClickNormal);
        nowActive = NowActiveWindow.Sound;
    }
    public void ButtonSoundToPause()
    {
        if (isPopUpClosing) return;
        soundManager.PlayFXSound(SoundTypeFX.ButtonClickNormal);
        nowActive = NowActiveWindow.Pause;
    }
    public void ButtonWarningClose()
    {
        if (isPopUpClosing) return;
        warningWindow.SetActive(false);
        isWarningActive = false;
    }
    public void ButtonWarningToTitle()
    {
        if (isPopUpClosing) return;
        soundManager.PlayFXSound(SoundTypeFX.ButtonClickLong);
        warningWindow.SetActive(false);
        isWarningActive = false;

        isPopUpClosing = true;
        if (gameoverTransform.gameObject.activeSelf) gameoverTransform.gameObject.SetActive(false);
        if (pauseTransform.gameObject.activeSelf) pauseTransform.gameObject.SetActive(false);
        popUpBasicTransform.DOScale(0f, 0.15f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(OffAllWindow);

        DOVirtual.DelayedCall(0.2f, GameManager.Instance.LoadTitleScene, true);
    }

    void OpenWarningWindow()
    {
        warningWindow.SetActive(true);
        isWarningActive = true;
    }

    void OnPopUp()
    {
        warningWindow.SetActive(false);
        isWarningActive = false;

        popUpBackgroundColor.color = Color.clear;
        popUpBackgroundColor.DOColor(new Color(0, 0, 0, 0.5f), 0.3f).SetUpdate(true);

        if (!popUpBasicTransform.gameObject.activeSelf) popUpBasicTransform.gameObject.SetActive(true);
    }

    void WarningTextSetting(Text text)
    {
        if (GameManager.Instance.gameState == GameState.Pause) text.text = "정말 게임을 중단하고 처음 화면으로 돌아갑니까?";
        else if (GameManager.Instance.gameState == GameState.GameOver) text.text = "처음 화면으로 돌아가시겠습니까?";
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
    public void OffAllWindow()
    {
        allPopUpWindow.SetActive(false);
        isPopUpClosing = false;
        nowActive = NowActiveWindow.None;
    }
}
