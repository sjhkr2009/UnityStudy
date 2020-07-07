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

        if (Data == null)
        {
            Data = GetComponent<DataManager>();
            if (Data == null) Data = gameObject.AddComponent<DataManager>();
        }
        //if (Input == null)
        //{
        //    Input = GetComponent<InputManager>();
        //    if (Input == null) Input = gameObject.AddComponent<InputManager>();
        //}
    }


    // Get Manager Class
    public static DataManager Data { get; private set; }
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


    private void Start()
    {
        GameState = GameState.Idle;
        Sound.Init();

        Input.InputStateChange -= StateChange;
        Input.InputNextState -= InNextState;
        Input.InputEscape -= OnEscape;
        OnGameStateChange -= Data.OnGameStateChange;

        Input.InputStateChange += StateChange;
        Input.InputNextState += InNextState;
        Input.InputEscape += OnEscape;
        OnGameStateChange += Data.OnGameStateChange;
    }
    private void OnDestroy()
    {
        Input.InputStateChange -= StateChange;
        Input.InputNextState -= InNextState;
        Input.InputEscape -= OnEscape;
        OnGameStateChange -= Data.OnGameStateChange;
    }

    private void Update()
    {
        Input.OnUpdate();
    }

    void MainInOrder()
    {
        Data.CurrentCocktail = new Cocktail();
    }

    void MainInSelectBase()
    {
        Debug.Log("베이스 재료 선택");
        UI.ClosePopupUI<SelectSubMaterialUI>();
        UI.OpenPopupUI<SelectBaseMaterialUI>();
    }

    void MainInSelectSub()
    {
        Debug.Log("부재료 선택");
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
        DOVirtual.DelayedCall(0.65f, () => { GameState = GameState.Idle; });
    }

    //임시 진행 버튼
    void InNextState()
    {
        GameState = (GameState)(((int)GameState + 1) % 6);
    }
    void OnEscape()
    {
        UI.TryClosePopupUI<MaterialInfoWindow>(); //추후 실패 시 옵션창을 띄울 것.
    }
}
