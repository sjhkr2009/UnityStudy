using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public enum GameState
{
    None,
    Order,
    SelectBase,
    SelectSub,
    Combine,
    SetCocktail
}

public class GameManager : MonoBehaviour
{
    // Singleton
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
        if (Input == null)
        {
            Input = GetComponent<InputManager>();
            if (Input == null) Input = gameObject.AddComponent<InputManager>();
        }
    }

    // Get Manager Class
    public static DataManager Data { get; private set; }
    public static InputManager Input { get; private set; }

    #region Current Info
    [ShowInInspector] private Customers _currentCustomer { get; set; }
    [ShowInInspector] private Order _currentOrder { get; set; }
    [ShowInInspector] private List<BaseMaterials> currentBaseMaterials = new List<BaseMaterials>();
    [ShowInInspector] private List<SubMaterials> currentSubMaterials = new List<SubMaterials>();
    [ShowInInspector] private Cocktail _currentCocktail { get; set; }

    public Customers CurrentCustomer
    {
        get => _currentCustomer;
        set
        {
            _currentCustomer = value;
            OnSetCustomer(value);
        }
    }
    public Order CurrentOrder
    {
        get => _currentOrder;
        set
        {
            _currentOrder = value;
            OnSetOrder(value);
        }
    }
    public Cocktail CurrentCocktail
    {
        get => _currentCocktail;
        set
        {
            _currentCocktail = value;
            OnSetCocktail(value);
        }
    }
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

    public Action<GameState> OnGameStateChange = n => { };
    public Action<Customers> OnSetCustomer = n => { };
    public Action<Order> OnSetOrder = n => { };
    public Action<BaseMaterials> OnSetBaseMaterial = n => { };
    public Action<BaseMaterials> OnRemoveBaseMaterial = n => { };
    public Action<SubMaterials> OnSetSubMaterial = n => { };
    public Action<SubMaterials> OnRemoveSubMaterial = n => { };
    public Action<Cocktail> OnSetCocktail = n => { };

    #endregion

    [SerializeField, ReadOnly] private GameState _gameState = GameState.None;
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
        GameState = GameState.None;

        //이벤트는 파괴 시 삭제할 것.
        Input.InputMaterialSelect += SelectMaterial;
        Input.InputStateChange += StateChange;
        Input.InputNextState += InNextState;
    }

    void MainInOrder()
    {
        CurrentCocktail = new Cocktail();

        CurrentCustomer = Data.SelectCustomer();
        CurrentOrder = CurrentCustomer.GetOrder();
        Debug.Log(_currentOrder.orderContents);
    }

    void MainInSelectBase()
    {
        Debug.Log("베이스 재료 선택");
    }

    void MainInSelectSub()
    {
        Debug.Log("부재료 선택");
    }
    void MainInCombine()
    {
        if (currentSubMaterials.Count == 0) currentSubMaterials.Add(new SubMaterials());
        CurrentCocktail = Data.MakeCocktail(currentBaseMaterials, currentSubMaterials);
        Debug.Log($"조합: {CurrentCocktail.cocktailName} 칵테일");
    }
    void MainInSetCocktail()
    {
        Data.CorrectCheck(CurrentCocktail, CurrentOrder);

        currentBaseMaterials.Clear();
        currentSubMaterials.Clear();
    }

    //임시 진행 버튼
    void InNextState()
    {
        GameState = (GameState)(((int)GameState + 1) % 6);
    }
}
