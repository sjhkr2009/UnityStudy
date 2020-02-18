using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using DG.Tweening;

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

    [SerializeField, ReadOnly] private GameState _gameState;
    public GameState gameState
    {
        get => _gameState;
        set
        {
            switch (value)
            {
                case GameState.Ready:
                    _gameState = GameState.Ready;
                    Time.timeScale = 0f;
                    break;
                case GameState.Playing:
                    _gameState = GameState.Playing;
                    Time.timeScale = 1f;
                    break;
                case GameState.Pause:
                    _gameState = GameState.Pause;
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    _gameState = GameState.GameOver;
                    break;
            }
            EventGameStateChanged(_gameState);
        }
    }

    [BoxGroup("Screen")] public float screenHorizontal = 9f, screenVertical = 16f;

    [BoxGroup("Scripts")] [SerializeField] Star star;                   public Star Star => star;
    [BoxGroup("Scripts")] [SerializeField] Planet planet;               public Planet Planet => planet;
    [BoxGroup("Scripts")] [SerializeField] UIManager uiManager;         public UIManager UiManager => uiManager;
    [BoxGroup("Scripts")] [SerializeField] EnemyManager enemyManager;   public EnemyManager EnemyManager => enemyManager;
    [BoxGroup("Scripts")] [SerializeField] SoundManager soundManager;   public SoundManager SoundManager => soundManager;
    [BoxGroup("Scripts")] [SerializeField] ScoreManager scoreManager;   public ScoreManager ScoreManager => scoreManager;
    [BoxGroup("Scripts")] [SerializeField] PoolManager poolManager;     public PoolManager PoolManager => poolManager;
    [BoxGroup("Scripts")] [SerializeField] ParticleManager particleManager; public ParticleManager ParticleManager => particleManager;
    [BoxGroup("Scripts")] [SerializeField] ItemManager itemManager;     public ItemManager ItemManager => itemManager;
    [BoxGroup("Scripts")] [SerializeField] FeverManager feverManager;   public FeverManager FeverManager => feverManager;
    [BoxGroup("Scripts")] [SerializeField] TimeManager timeManager;     public TimeManager TimeManager => timeManager;

    Vector3 mousePos;
    public event Action<Vector3> EventOnTouchScreen = n => { };

    private void Awake()
    {
        _instance = this;

        if (uiManager == null) uiManager = GetComponent<UIManager>();
        if (enemyManager == null) enemyManager = GetComponent<EnemyManager>();
        if (soundManager == null) soundManager = GetComponent<SoundManager>();
        if (scoreManager == null) scoreManager = GetComponent<ScoreManager>();
        if (poolManager == null) poolManager = GetComponent<PoolManager>();
        if (particleManager == null) particleManager = GetComponent<ParticleManager>();
        if (feverManager == null) feverManager = GetComponent<FeverManager>();
        if (timeManager == null) timeManager = GetComponent<TimeManager>();
        if (star == null) star = FindObjectOfType<Star>();
        if (planet == null) planet = FindObjectOfType<Planet>();

        star.EventHpChanged += OnPlayerHpChanged;
        star.EventPlayerDead += OnPlayerDead;

        planet.EventHpChanged += OnPlayerHpChanged;
        planet.EventPlayerDead += OnPlayerDead;

        EventOnTouchScreen += star.TargetRadiusChange;

        EventGameStateChanged += star.OnGameStateChanged;
        EventGameStateChanged += uiManager.OnGameStateChanged;

        scoreManager.EventOnScoreChanged += uiManager.ScoreTextChange;
        scoreManager.EventOnGameOver += uiManager.OnGameOverScorePrint;

        feverManager.EventOnFeverTime += star.OnFeverTime;
        feverManager.EventExitFeverTime += star.ExitFeverTime;

        timeManager.EventPerOneSecond += feverManager.GetFeverCountPerSecond;


        gameState = GameState.Ready;
        DOVirtual.DelayedCall(4f, () =>
        {
            if (gameState == GameState.Ready)
            {
                gameState = GameState.Playing;
                Debug.Log("강제 시작");
            }
        });
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

        EventOnTouchScreen -= star.TargetRadiusChange;

        EventGameStateChanged -= star.OnGameStateChanged;
        EventGameStateChanged -= uiManager.OnGameStateChanged;

        scoreManager.EventOnScoreChanged -= uiManager.ScoreTextChange;
        scoreManager.EventOnGameOver -= uiManager.OnGameOverScorePrint;

        feverManager.EventOnFeverTime -= star.OnFeverTime;
        feverManager.EventExitFeverTime -= star.ExitFeverTime;

        timeManager.EventPerOneSecond -= feverManager.GetFeverCountPerSecond;
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (gameState)
            {
                case GameState.Ready:
                    break;
                case GameState.Playing:
                    gameState = GameState.Pause;
                    break;
                case GameState.Pause:
                    uiManager.Escape();
                    break;
                case GameState.GameOver:
                    uiManager.Escape();
                    break;
            }
        }
    }

    public void OnClickScreen()
    {
        if (gameState == GameState.Playing) EventOnTouchScreen(mousePos);
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
        if(gameState == GameState.Playing) gameState = GameState.GameOver;
    }

    /// <summary>
    /// Scene을 닫기 전에 호출하는 함수입니다. 오브젝트 풀링으로 생성된 모든 오브젝트의 이벤트를 초기화하고, 팝업창을 닫으며 TimeScale을 기본값으로 되돌립니다. 
    /// </summary>
    void SceneReset()
    {
        itemManager.AllItemEventReset();
        enemyManager.AllEnemyEventReset();
        uiManager.OffAllWindow();
        Time.timeScale = 1f;
    }

    public void ReStartScene()
    {
        SceneReset();
        SceneManager.LoadScene("Play");
    }

    public void LoadTitleScene()
    {
        SceneReset();
        SceneManager.LoadScene("Title");
    }
}
