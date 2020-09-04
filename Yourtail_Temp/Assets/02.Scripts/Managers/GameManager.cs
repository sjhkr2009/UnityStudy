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
    //Singleton
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        _data.Init();
    }


    // Get Manager Class
    DataManager _data = new DataManager();
    public static DataManager Data => Instance._data;
    InputManager _input = new InputManager();
    public static InputManager Input => Instance._input;
    ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource => Instance._resource;
    UIManager _ui = new UIManager();
    public static UIManager UI => Instance._ui;
    SoundManager _sound = new SoundManager();
    public static SoundManager Sound => Instance._sound;

    public Action<GameState> OnGameStateChange = n => { };
    [SerializeField, ReadOnly] private GameState _gameState = GameState.Idle;
    public GameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
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
            OnGameStateChange(value);
        }
    }
    void StateChange(GameState state) { GameState = state; }
    public bool ignoreOnMouse = false;

    private void Start()
    {
        _data.LoadFromPlayerPrefs();

        GameState = GameState.Idle;
        Sound.Init();

        Input.InputStateChange -= StateChange;
        Input.InputEscape -= OnEscape;
        Input.InputRetryCocktail -= OnRetryCocktail;
        OnGameStateChange -= Data.OnGameStateChange;

        Input.InputStateChange += StateChange;
        Input.InputEscape += OnEscape;
        Input.InputRetryCocktail += OnRetryCocktail;
        OnGameStateChange += Data.OnGameStateChange;

        //temp
        Sound.Play(Define.SoundType.BGM, "jazz_bar", 0.07f);
    }
    private void OnDestroy()
    {
        _data.SaveToPlayerPrefs();
        
        Input.InputStateChange -= StateChange;
        //Input.InputNextState -= InNextState;
        Input.InputEscape -= OnEscape;
        Input.InputRetryCocktail -= OnRetryCocktail;
        OnGameStateChange -= Data.OnGameStateChange;

        Input.Clear();

        DOTween.Clear();
    }

    private void Update()
    {
        Input.OnUpdate();
    }

    void MainInIdle()
    {
        UI.CloseAllPopup();
    }

    void MainInSelect()
    {
        UI.CloseAllPopup();
        UI.OpenSceneUI<SelectMaterialUI>();
    }
    void MainInCombine()
    {
        UI.CloseAllPopup();
        UI.CloseSceneUI<SelectMaterialUI>();
        UI.OpenPopupUI<MakeCocktailUI>();
    }
    void MainInSetCocktail()
    {
        UI.CloseAllPopup();
        UI.OpenPopupUI<SetCocktailUI>();
    }

    public void SetDialog()
    {
        if (Data.CurrentCustomer == Data.CustomerNameData[new Eagle().Name])
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
        Data.ResetSelected();
        GameState = GameState.Select;
    }
    
    public void QuitApp()
    {
        if(!Application.isEditor)
            _input.Clear();

        _data.SaveToPlayerPrefs();
        Application.Quit();
    }
}
