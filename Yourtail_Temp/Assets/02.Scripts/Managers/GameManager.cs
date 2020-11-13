using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using DG.Tweening;

public enum GameState
{
    Idle,
    Select,
    Combine,
    SetCocktail
}

public class GameManager : MonoBehaviour
{
    public float PlayTime { get; private set; }
    
    //Singleton
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        _game.Init();
    }


    // Get Manager Class
    GameManagerEx _game = new GameManagerEx();
    public static GameManagerEx Game => Instance._game;
    InputManager _input = new InputManager();
    public static InputManager Input => Instance._input;
    ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource => Instance._resource;
    UIManager _ui = new UIManager();
    public static UIManager UI => Instance._ui;
    SoundManager _sound = new SoundManager();
    public static SoundManager Sound => Instance._sound;

    BirdStories _dialog = new BirdStories();
    public static BirdStories Dialog => Instance._dialog;

    public Action<GameState> OnGameStateEnter = n => { };
    public Action<GameState> OnGameStateQuit = n => { };
    [SerializeField, ReadOnly] private GameState _gameState = GameState.Idle;
    public GameState GameState
    {
        get => _gameState;
        set
        {
            OnGameStateQuit(_gameState);
            _gameState = value;
            UI.CloseAllPopup();
            switch (value)
            {
                case GameState.Idle:
                    MainInIdle();
                    break;
                case GameState.Select:
                    MainInSelect();
                    break;
                case GameState.Combine:
                    MainInCombine();
                    break;
                case GameState.SetCocktail:
                    MainInSetCocktail();
                    break;
            }
            OnGameStateEnter(value);
        }
    }
    void StateChange(GameState state) { GameState = state; }
    public bool ignoreOnMouse = false;

    private void Start()
    {
        _game.LoadFromPlayerPrefs();
        _dialog.DialogSetting();

        Sound.Init();
        UI.Init();

        Input.InputStateChange -= StateChange;
        Input.InputEscape -= OnEscape;
        Input.InputRetryCocktail -= OnRetryCocktail;

        Input.InputStateChange += StateChange;
        Input.InputEscape += OnEscape;
        Input.InputRetryCocktail += OnRetryCocktail;

        PlayTime = 0f;

        //temp
        Sound.Play(Define.SoundType.BGM, "jazz_bar", 0.07f);

        if (GameState != GameState.Idle)
            DOVirtual.DelayedCall(0.1f, () => { GameState = GameState.Idle; });
    }
    private void OnDestroy()
    {
        _game.SaveToPlayerPrefs();
        
        Input.InputStateChange -= StateChange;
        Input.InputEscape -= OnEscape;
        Input.InputRetryCocktail -= OnRetryCocktail;

        Input.Clear();

        DOTween.Clear();
    }

    private void Update()
    {
        Input.OnUpdate();
        PlayTime += Time.deltaTime;
    }

    void MainInIdle()
    {
        UI.CloseSceneUI<SelectMaterialUI>();
    }

    void MainInSelect()
    {
        UI.OpenSceneUI<SelectMaterialUI>();
    }
    void MainInCombine()
    {
        UI.OpenPopupUI<MakeCocktailUI>();
    }
    void MainInSetCocktail()
    {
        UI.CloseSceneUI<SelectMaterialUI>();
        UI.OpenPopupUI<CocktailReactionUI>();
    }
    
    public void SetDialog()
    {
        Game.DeleteCustomer();

        if (Game.levelUp)
            UI.OpenPopupUI<DialogUI>();
        else
            GameState = GameState.Idle;
    }

    void OnEscape()
    {
        if (UI.TryClosePopupUI<WarningUI>()) return;

        UI.CurrentWarningType = Define.WarningType.QuitApp;
        UI.OpenPopupUI<WarningUI>();
    }
    void OnRetryCocktail()
    {
        UI.ClosePopupUI<MakeCocktailUI>();
        Game.ResetSelected();
        GameState = GameState.Select;
    }
    
    public void QuitApp()
    {
        if(!Application.isEditor)
            _input.Clear();

        _game.SaveToPlayerPrefs();
        Application.Quit();
    }
}
