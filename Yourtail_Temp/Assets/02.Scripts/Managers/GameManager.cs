using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;

public enum GameState
{
    Idle,
    Order,
    SelectBase,
    SelectSub,
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
                case GameState.Order:
                    MainInOrder();
                    break;
                case GameState.SelectBase:
                    MainInSelectBase();
                    break;
                case GameState.SelectSub:
                    MainInSelectSub();
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
        GameState = GameState.Idle;
        Sound.Init();
        _data.LoadFromPlayerPrefs();

        Input.InputStateChange -= StateChange;
        //Input.InputNextState -= InNextState;
        Input.InputEscape -= OnEscape;
        Input.InputRetryCocktail -= OnRetryCocktail;
        OnGameStateChange -= Data.OnGameStateChange;

        Input.InputStateChange += StateChange;
        //Input.InputNextState += InNextState;
        Input.InputEscape += OnEscape;
        Input.InputRetryCocktail += OnRetryCocktail;
        OnGameStateChange += Data.OnGameStateChange;
    }
    private void OnDestroy()
    {
        _data.SaveToPlayerPrefs();
        
        Input.InputStateChange -= StateChange;
        //Input.InputNextState -= InNextState;
        Input.InputEscape -= OnEscape;
        Input.InputRetryCocktail -= OnRetryCocktail;
        OnGameStateChange -= Data.OnGameStateChange;

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
    void MainInOrder()
    {
        Data.CurrentCocktail = new Cocktail();
    }

    void MainInSelectBase()
    {
        UI.ClosePopupUI<SelectSubMaterialUI>();
        UI.OpenPopupUI<SelectBaseMaterialUI>();
    }

    void MainInSelectSub()
    {
        UI.ClosePopupUI<SelectBaseMaterialUI>();
        UI.OpenPopupUI<SelectSubMaterialUI>();
    }
    void MainInCombine()
    {
        UI.ClosePopupUI();
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
        if (UI.TryClosePopupUI<MaterialInfoWindow>()) return;
        if (UI.TryClosePopupUI<CheckBeforeShake>()) return;
        if (UI.TryClosePopupUI<OrderInfoWindow>()) return;
        if (UI.TryClosePopupUI<OrderBubble>()) return;

        //추후 옵션창을 추가하게 된다면, 실패 시 옵션창을 띄울 것.
    }
    void OnRetryCocktail()
    {
        UI.ClosePopupUI<MakeCocktailUI>();
        Data.Retry();
        GameState = GameState.SelectBase;
    }
    
}
