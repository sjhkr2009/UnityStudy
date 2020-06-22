using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum GameState { Ready, Playing, Pause, GameOver, StageClear }

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GameManager>();
            if (_instance == null)
            {
                GameObject container = new GameObject("GameManager");
                _instance = container.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    private GameState _gameState;
    public GameState gameState
    {
        get => _gameState;
        set
        {
            switch (value)
            {
                case GameState.Ready:
                    _gameState = value;
                    EventOnReady();
                    break;
                case GameState.Playing:
                    _gameState = value;
                    EventOnPlaying();
                    Time.timeScale = 1f;
                    break;
                case GameState.Pause:
                    _gameState = value;
                    EventOnPause();
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    _gameState = value;
                    EventOnGameOver();
                    break;
                case GameState.StageClear:
                    _gameState = value;
                    EventOnStageClear();
                    break;
            }
        }
    }

    public event Action EventOnReady = () => { };
    public event Action EventOnPlaying = () => { };
    public event Action EventOnPause = () => { };
    public event Action EventOnGameOver = () => { Debug.Log("Game Over"); };
    public event Action EventOnStageClear = () => { };

    [SerializeField] [BoxGroup("Scripts")] Player player;
    [SerializeField] [BoxGroup("Scripts")] JoyStickController joyStick;
    [SerializeField] [BoxGroup("Scripts")] LaserManager laserManager;
    [SerializeField] [BoxGroup("Scripts")] UiManager uiManager;

    private void Awake()
    {
        _instance = this;
        if(player == null) player = FindObjectOfType<Player>();
        if (joyStick == null) joyStick = FindObjectOfType<JoyStickController>();
        if (laserManager == null) laserManager = GetComponent<LaserManager>();
        if (uiManager == null) uiManager = GetComponent<UiManager>();

        gameState = GameState.Ready;
    }
    void Start()
    {
        player.EventOnHpChanged += GameoverCheck;

        joyStick.EventOnDrag += player.PlayerMove;

        laserManager.EventInHacking += uiManager.OnHacking;
        laserManager.EventOutHacking += uiManager.ExitHacking;
        laserManager.EventOnLaserHit += OnLaserHit;

        EventOnReady += uiManager.StartReadyCountdown;
        EventOnPause += uiManager.OnPause;
        EventOnPlaying += uiManager.ExitPause;
    }

    private void OnDestroy()
    {
        player.EventOnHpChanged -= GameoverCheck;

        joyStick.EventOnDrag -= player.PlayerMove;

        laserManager.EventInHacking -= uiManager.OnHacking;
        laserManager.EventOutHacking -= uiManager.ExitHacking;
    }


    void GameoverCheck(float playerHp)
    {
        if(playerHp <= 0f)
        {
            if (gameState == GameState.Playing) gameState = GameState.GameOver;
        }
    }

    void OnLaserHit(RaycastHit2D hit)
    {
        Debug.Log($"{hit.collider.gameObject.name}에 적중함");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
            if (gameState == GameState.Playing) gameState = GameState.Pause;
        }
        else
        {
            if (gameState != GameState.Pause) Time.timeScale = 1f;
        }
    }
}
