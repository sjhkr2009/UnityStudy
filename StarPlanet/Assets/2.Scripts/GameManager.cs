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
    [BoxGroup("Scripts")] [SerializeField] Star star;                   public Star Star => star;
    [BoxGroup("Scripts")] [SerializeField] Planet planet;               public Planet Planet => planet;
    [BoxGroup("Scripts")] [SerializeField] EnemyManager enemyManager;
    [BoxGroup("Scripts")] [SerializeField] SoundManager soundManager;   public SoundManager SoundManager => soundManager;
    [BoxGroup("Scripts")] [SerializeField] ScoreManager scoreManager;
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
        star = FindObjectOfType<Star>();
        planet = FindObjectOfType<Planet>();

        star.EventHpChanged += OnPlayerHpChanged;
        star.EventPlayerDead += OnPlayerDead;

        planet.EventHpChanged += OnPlayerHpChanged;
        planet.EventPlayerDead += OnPlayerDead;

        EventGameStateChanged += star.OnGameStateChanged;

        uiManager.EventCountDownDone += OnCountDownDone;
    }

    void Start()
    {
        if(_instance != null && _instance != this) { Destroy(gameObject);}
    }


    private void OnDestroy()
    {
        star.EventHpChanged -= OnPlayerHpChanged;
        star.EventPlayerDead -= OnPlayerDead;

        planet.EventHpChanged -= OnPlayerHpChanged;
        planet.EventPlayerDead -= OnPlayerDead;

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

    /// <summary>
    /// 플레이어를 선택하여 체력을 변화시킵니다.
    /// </summary>
    /// <param name="isStar">star와 planet 중 체력을 변화시킬 대상을 선택하세요. true면 Star, false면 Planet이 선택됩니다.</param>
    /// <param name="changeLevel">변화시킬 수치를 입력하세요. 양수 값이면 체력이 회복되고, 음수면 체력이 감소합니다.</param>
    public void PlayerHPChange(bool isStar, int changeLevel)
    {
        if (isStar) star.Hp += changeLevel;
        else planet.Hp += changeLevel;
    }

    public void OnPlayerHpChanged(int hp, Player player) 
    {
        if (hp <= 0) player.gameObject.SetActive(false);
    }

    private void OnPlayerDead<T>(T player) where T : Player
    {
        gameState = GameState.GameOver;
    }

    private void OnCountDownDone()
    {
        gameState = GameState.Playing;
    }
}
