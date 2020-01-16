using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public enum GameState { Ready, Playing, Pause, GameOver }

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    public event Action<GameState> EventGameStateChanged = e => { };

    private GameState _gameState;
    public GameState gameState
    {
        get => _gameState;
        set
        {
            switch (value)
            {
                case GameState.Ready:
                    _gameState = GameState.Ready;
                    break;
                case GameState.Playing:
                    _gameState = GameState.Playing;
                    StartCoroutine(enemyManager.EnemySpawn());
                    Time.timeScale = 1f;
                    break;
                case GameState.Pause:
                    _gameState = GameState.Pause;
                    StopAllCoroutines();
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    _gameState = GameState.GameOver;
                    Debug.Log("Game Over");
                    break;
            }
            EventGameStateChanged(_gameState);
        }
    }

    [BoxGroup("Scripts")] [SerializeField] UIManager uiManager;
    [BoxGroup("Scripts")] [SerializeField] Star star;
    public Star Star => star;
    [BoxGroup("Scripts")] [SerializeField] EnemyManager enemyManager;
    [BoxGroup("Scripts")] [SerializeField] SoundManager soundManager;
    public SoundManager SoundManager => soundManager;
    [BoxGroup("Scripts")] public PoolManager poolManager;
    [BoxGroup("Scripts")] public ParticleManager particleManager;

    Vector3 mousePos;
    public event Action<Vector3> EventOnClick;

    private void Awake()
    {
        _instance = this;
        gameState = GameState.Ready;
        uiManager = GetComponent<UIManager>();
        enemyManager = GetComponent<EnemyManager>();
        soundManager = GetComponent<SoundManager>();
        particleManager = GetComponent<ParticleManager>();
    }

    void Start()
    {
        if(_instance != null && _instance != this) { Destroy(gameObject);}

        star.EventHpChanged += OnPlayerHpChanged;
        star.EventPlayerDead += OnPlayerDead;

        EventGameStateChanged += star.OnGameStateChanged;

        uiManager.EventCountDownDone += OnCountDownDone;
    }


    private void OnDestroy()
    {
        star.EventHpChanged -= OnPlayerHpChanged;
        star.EventPlayerDead -= OnPlayerDead;

        EventGameStateChanged -= star.OnGameStateChanged;

        uiManager.EventCountDownDone -= OnCountDownDone;
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

        if (Input.GetMouseButton(0))
        {
            star.TargetRadiusChange(mousePos);
        }
    }


    public void OnPlayerHpChanged(int hp)
    {
        if (hp == 0) gameState = GameState.GameOver;
    }

    private void OnPlayerDead(Star obj)
    {
        gameState = GameState.GameOver;
    }

    private void OnCountDownDone()
    {
        gameState = GameState.Playing;
    }
}
