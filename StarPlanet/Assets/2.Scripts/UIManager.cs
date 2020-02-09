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
    [BoxGroup("Playing UI")] [SerializeField] Text countdownText;
    [BoxGroup("Playing UI")] [SerializeField] GameObject orbitalRadiusUI;
    [BoxGroup("Playing UI")] [SerializeField] Slider starHpBar;
    [BoxGroup("Playing UI")] [SerializeField] Slider planetHpBar;
    [BoxGroup("Playing UI")] [SerializeField] Text scoreText;

    //[Title("Accelerate")]
    [BoxGroup("Button")] [SerializeField] Image accelBackground;
    [BoxGroup("Button")] [SerializeField] Color accelColorIdle;
    [BoxGroup("Button")] [SerializeField] Color accelColorActive;
    [BoxGroup("Button")] [SerializeField] GameObject accelIconIdle;
    [BoxGroup("Button")] [SerializeField] GameObject accelIconActive;

    [BoxGroup("Pop-Up Window")] [SerializeField] GameObject allPopUpWindow;
    [BoxGroup("Pop-Up Window")] [SerializeField] Text titleText;
    [BoxGroup("Pop-Up Window")] [SerializeField] RectTransform popUpBackground;
    [BoxGroup("Pop-Up Window")] [SerializeField] RectTransform GameoverUI;

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
        yield return new WaitForSeconds(0.3f);

        switch (mode)
        {
            case CloseMode.Resume:
                break;
            case CloseMode.Restart:
                GameManager.Instance.ReStartScene();
                break;
            case CloseMode.GoTitle:
                break;
        }
    }
    public void OnGameStateChanged(GameState gameState)
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
                if (!popUpBackground.gameObject.activeSelf) popUpBackground.gameObject.SetActive(true);
                PopUpWindow(GameoverUI);
                titleText.text = "OPTION";
                break;
            case GameState.GameOver:
                allPopUpWindow.SetActive(true);
                if (!popUpBackground.gameObject.activeSelf) popUpBackground.gameObject.SetActive(true);
                PopUpWindow(GameoverUI);
                titleText.text = "GAME OVER";
                break;
        }
    }
    public void ButtonGameoverToTitle()
    {
        StartCoroutine(OffWindow(GameoverUI));
        StartCoroutine(SceneLoadButton(CloseMode.GoTitle)); //타이틀로 이동
    }
    public void ButtonGameoverToRestart()
    {
        StartCoroutine(OffWindow(GameoverUI));
        StartCoroutine(SceneLoadButton(CloseMode.Restart));
    }
    public void ButtonPauseToResume()
    {
        StartCoroutine(OffWindow(GameoverUI)); //옵션창으로 변경할 것
        StartCoroutine(SceneLoadButton(CloseMode.Resume));
    }
    public void ButtonPauseToTitle()
    {
        StartCoroutine(OffWindow(GameoverUI)); //옵션창으로 변경할 것
        StartCoroutine(SceneLoadButton(CloseMode.GoTitle)); //타이틀로 이동
    }

    void PopUpWindow(RectTransform windowScale) // 팝업 열때 에니메이션. 애니메이션은 배경에만 넣자
    {
        windowScale.gameObject.SetActive(true);
        windowScale.localScale = Vector3.one * 0.5f;
        popUpBackground.localScale = Vector3.one * 0.5f;

        windowScale.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        popUpBackground.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }
    IEnumerator OffWindow(RectTransform windowScale) //메뉴와 버튼을 비활성화한 후 배경을 애니메이션으로 닫기. 닫은 후에 추가 동작 실행.
    {
        if (isPopUpClosing) yield break;

        isPopUpClosing = true;
        windowScale.DOScale(0f, 0.2f).SetEase(Ease.InBack);
        popUpBackground.DOScale(0f, 0.2f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(0.3f);

        windowScale.gameObject.SetActive(false);
        allPopUpWindow.SetActive(false);
        isPopUpClosing = false;
    }
    
}
