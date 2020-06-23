using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using TMPro;

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

    #region Current Info
    [ShowInInspector] private List<BaseMaterials> currentBaseMaterials = new List<BaseMaterials>();
    [ShowInInspector] private List<SubMaterials> currentSubMaterials = new List<SubMaterials>();
    
    void AddCurrentBase(BaseMaterials item)
    {
        currentBaseMaterials.Add(item);
        OnSetBaseMaterial(item);
    }
    void RemoveCurrentBase(BaseMaterials item)
    {
        currentBaseMaterials.Remove(item);
        OnRemoveBaseMaterial(item);
    }

    void AddCurrentSub(SubMaterials item)
    {
        currentSubMaterials.Add(item);
        OnSetSubMaterial(item);
    }
    void RemoveCurrentSub(SubMaterials item)
    {
        currentSubMaterials.Remove(item);
        OnRemoveSubMaterial(item);
    }
    public Action<BaseMaterials> OnSetBaseMaterial = n => { };
    public Action<BaseMaterials> OnRemoveBaseMaterial = n => { };
    public Action<SubMaterials> OnSetSubMaterial = n => { };
    public Action<SubMaterials> OnRemoveSubMaterial = n => { };

    #endregion
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

    void SelectMaterial(int code)
    {
        if(GameState == GameState.SelectBase)
        {
            BaseMaterials value = Data.BaseMaterialData["B" + code.ToString()];
            if (currentBaseMaterials.Contains(value)) RemoveCurrentBase(value);
            else AddCurrentBase(value);
        }
        else if(GameState == GameState.SelectSub)
        {
            SubMaterials value = Data.SubMaterialData["S" + code.ToString()];
            if (currentSubMaterials.Contains(value)) RemoveCurrentSub(value);
            else AddCurrentSub(value);
        }
    }

    private void Start()
    {
        GameState = GameState.Idle;

        //이벤트는 파괴 시 삭제할 것.
        Input.InputMaterialSelect -= SelectMaterial;
        Input.InputStateChange -= StateChange;
        Input.InputNextState -= InNextState;

        Input.InputMaterialSelect += SelectMaterial;
        Input.InputStateChange += StateChange;
        Input.InputNextState += InNextState;
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
        UI.ClosePopupUI();
        UI.OpenPopupUI<SelectBaseMaterialUI>();
    }

    void MainInSelectSub()
    {
        Debug.Log("부재료 선택");
        UI.ClosePopupUI();
        UI.OpenPopupUI<SelectSubMaterialUI>();
    }
    void MainInCombine()
    {
        if (currentSubMaterials.Count == 0) currentSubMaterials.Add(new SubMaterials());
        Data.CurrentCocktail = Data.MakeCocktail(currentBaseMaterials, currentSubMaterials); //해당 스크립트에서 이벤트로 처리하기
        Debug.Log($"조합: {Data.CurrentCocktail.cocktailName} 칵테일");
    }
    void MainInSetCocktail()
    {
        Data.CurrentCorrectCheck();

        currentBaseMaterials.Clear();
        currentSubMaterials.Clear();
        UI.CloseAllPopup();
    }

    //임시 진행 버튼
    void InNextState()
    {
        GameState = (GameState)(((int)GameState + 1) % 6);
    }
}
