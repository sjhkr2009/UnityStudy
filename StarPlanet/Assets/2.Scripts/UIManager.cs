using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;
using DG.Tweening;

enum CloseMode { Resume, Restart, GoTitle }

public class UIManager : MonoBehaviour
{
    //상태창
    [BoxGroup("Playing UI")] [SerializeField] Text countdownText;
    [BoxGroup("Playing UI")] [SerializeField] GameObject orbitalRadiusUI;
    [BoxGroup("Playing UI")] [SerializeField] Slider starHpBar;
    [BoxGroup("Playing UI")] [SerializeField] Slider planetHpBar;
    [BoxGroup("Playing UI")] [SerializeField] Text scoreText;

    //가속 버튼 관련
    [BoxGroup("Button")] [SerializeField] Image accelBackground;
    [BoxGroup("Button")] [SerializeField] Color accelColorIdle;
    [BoxGroup("Button")] [SerializeField] Color accelColorActive;
    [BoxGroup("Button")] [SerializeField] GameObject accelIconIdle;
    [BoxGroup("Button")] [SerializeField] GameObject accelIconActive;

    //팝업창 관련 - 공통
    [BoxGroup("Pop-Up Window")] [SerializeField] GameObject allPopUpWindow;
    [BoxGroup("Pop-Up Window")] [SerializeField] GameObject popUpBackground;
    [BoxGroup("Pop-Up Window")] [SerializeField] RectTransform backgroundTransform;
    [BoxGroup("Pop-Up Window")] [SerializeField] GameObject gameoverUI;
    [BoxGroup("Pop-Up Window")] [SerializeField] RectTransform gameoverTransform;
    [BoxGroup("Pop-Up Window")] [SerializeField] GameObject pauseUI;
    [BoxGroup("Pop-Up Window")] [SerializeField] RectTransform pauseTransform;

    [BoxGroup("Object")] [SerializeField] Star star;
    [BoxGroup("Object")] [SerializeField] Planet planet;

    public event Action EventCountDownDone = () => { };

    bool isPopUpClosing = false;

    private void Awake()
    {
        StartCoroutine(CountdownToPlay());

        star.EventRadiusChange += RadiusChange;
        star.EventHpChanged += OnPlayerHpChanged;
        star.EventMaxHpChanged += OnPlayerMaxHpChanged;

        planet.EventHpChanged += OnPlayerHpChanged;
        planet.EventMaxHpChanged += OnPlayerMaxHpChanged;

        accelIconActive.SetActive(false);
        allPopUpWindow.SetActive(false);
    }

    private void Start()
    {
        scoreText.text = "점수: 0";
    }

    private void OnDestroy()
    {
        star.EventRadiusChange -= RadiusChange;
        star.EventHpChanged -= OnPlayerHpChanged;
        star.EventMaxHpChanged -= OnPlayerMaxHpChanged;

        planet.EventHpChanged -= OnPlayerHpChanged;
        planet.EventMaxHpChanged -= OnPlayerMaxHpChanged;
    }

    void RadiusChange(float radius)
    {
        orbitalRadiusUI.transform.localScale = Vector3.one * radius * 2;
    }

    IEnumerator CountdownToPlay()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        EventCountDownDone();
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
        scoreText.text = $"점수: {value.ToString()}";
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

    //팝업창 관련 설정
    IEnumerator SceneLoadButton(CloseMode mode) //버튼에 따른 동작 설정
    {
        yield return new WaitForSeconds(0.25f);

        switch (mode)
        {
            case CloseMode.Resume:
                GameManager.Instance.gameState = GameState.Playing;
                break;
            case CloseMode.Restart:
                GameManager.Instance.ReStartScene();
                break;
            case CloseMode.GoTitle:
                break;
        }
    }
    public void OnGameStateChanged(GameState gameState) //게임 상태 변화에 따른 동작 설정
    {
        switch (gameState)
        {
            case GameState.Ready:
                if (allPopUpWindow.activeSelf) allPopUpWindow.SetActive(false);
                break;
            case GameState.Playing:
                if (allPopUpWindow.activeSelf) allPopUpWindow.SetActive(false);
                break;
            case GameState.Pause:
                allPopUpWindow.SetActive(true);
                pauseUI.SetActive(true);
                break;
            case GameState.GameOver:
                allPopUpWindow.SetActive(true);
                gameoverUI.SetActive(true);
                break;
        }
    }
    public void ButtonGameoverToTitle()
    {
        if (isPopUpClosing) return;
        OffWindowAnimation(gameoverTransform);
        Invoke(nameof(OffAllWindow), 0.2f);
        StartCoroutine(SceneLoadButton(CloseMode.GoTitle)); //타이틀로 이동
    }
    public void ButtonGameoverToRestart()
    {
        if (isPopUpClosing) return;
        OffWindowAnimation(gameoverTransform);
        Invoke(nameof(OffAllWindow), 0.2f);
        StartCoroutine(SceneLoadButton(CloseMode.Restart)); //씬 재시작
    }
    public void ButtonPauseToResume()
    {
        if (isPopUpClosing) return;
        OffWindowAnimation(pauseTransform);
        Invoke(nameof(OffAllWindow), 0.2f);
        StartCoroutine(SceneLoadButton(CloseMode.Resume));
    }
    public void ButtonPauseToTitle()
    {
        if (isPopUpClosing) return;
        OffWindowAnimation(pauseTransform);
        Invoke(nameof(OffAllWindow), 0.2f);
        StartCoroutine(SceneLoadButton(CloseMode.GoTitle)); //타이틀로 이동
    }

    void OffWindowAnimation(RectTransform windowScale) //팝업 닫을 때 애니메이션. 비활성화 기능은 별도.
    {
        isPopUpClosing = true;
        windowScale.DOScale(0f, 0.15f).SetEase(Ease.InBack);
        backgroundTransform.DOScale(0f, 0.15f).SetEase(Ease.InBack);
    }
    void OffAllWindow() //팝업창 비활성화. 창 닫기 애니메이션 이후 실행.
    {
        allPopUpWindow.SetActive(false);
        isPopUpClosing = false;
    }
}
