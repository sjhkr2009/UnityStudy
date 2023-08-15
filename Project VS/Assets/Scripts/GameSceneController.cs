using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : GameListenerBehaviour {
    [SerializeField] private GameObject onDeadUI;
    [SerializeField] private TMP_Text onDeadTimeText;
    [SerializeField] private TMP_Text onDeadCoinText;
    
    private bool clickedLobby = false;

    protected override void Start() {
        onDeadUI.SetActive(false);
        base.Start();
        GameManager.Instance.StartGame();
    }

    public void GoToLobby() {
        if (clickedLobby) return;
        
        SceneManager.LoadScene("LobbyScene");
    }

    void OnEndGame() {
        var score = GameManager.Instance.GetScore();
        UserData.Coin += Mathf.FloorToInt(score);
        
        GameManager.Instance.ClearAll();
    }

    public void OnClickQuit() {
        OnEndGame();
        GoToLobby();
    }

    public void OnClickReplay() {
        OnEndGame();
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void OnClickContinue() {
        GameManager.Instance.ContinueGame();
    }

    public override void OnResumeGame() {
        onDeadUI.SetActive(false);
    }

    public override void OnDeadPlayer() {
        onDeadUI.SetActive(true);
        
        var status = GameManager.Controller.GetCurrentStatus();
        var time = TimeSpan.FromSeconds(status.gameTime);
        onDeadTimeText.text = time.ToString("mm':'ss");
        onDeadCoinText.text = GameManager.Instance.GetScore().ToString();
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        GameManager.Instance.ClearController();
    }
}
