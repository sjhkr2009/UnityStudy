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
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        if (CocktailManager == null)
        {
            CocktailManager = GetComponent<CocktailManager>();
            if (CocktailManager == null) CocktailManager = gameObject.AddComponent<CocktailManager>();
        }
    }

    public static CocktailManager CocktailManager { get; private set; }

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
    public void AddCurrentBase(string itemcode)
    {
        BaseMaterials value = CocktailManager.BaseMaterialData[itemcode];

        if (currentBaseMaterials.Contains(value)) return;
        currentBaseMaterials.Add(value);
        OnSetBaseMaterial(value);
    }
    public void AddCurrentSub(string itemcode)
    {
        SubMaterials value = CocktailManager.SubMaterialData[itemcode];

        if (currentSubMaterials.Contains(value)) return;
        currentSubMaterials.Add(value);
        OnSetSubMaterial(value);
    }

    public Action<GameState> OnGameStateChange = n => { };
    public Action<Customers> OnSetCustomer = n => { };
    public Action<Order> OnSetOrder = n => { };
    public Action<BaseMaterials> OnSetBaseMaterial = n => { };
    public Action<SubMaterials> OnSetSubMaterial = n => { };
    public Action<Cocktail> OnSetCocktail = n => { };

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

    void MainInOrder()
    {
        CurrentCustomer = SelectCustomer();
        CurrentCustomer.currentIndex = UnityEngine.Random.Range(0, 2);
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
        Combine();
        Debug.Log($"조합: {CurrentCocktail.cocktailName} 칵테일");
    }
    void MainInSetCocktail()
    {
        CorrectCheck();
        CurrentCustomer.currentIndex++; //이 작업이 유효하려면 각 손님을 싱글톤으로 생성하거나, 손님의 정보가 PlayerPrefs 또는 Dictionary 등으로 게임내에 저장되어야 함.

        currentBaseMaterials.Clear();
        currentSubMaterials.Clear();
    }


    #region 임시 함수

    //테스트용으로 무조건 독수리가 출현하도록 한다. 손님의 주문 내역을 관리하는 스크립트로 이관할 것.
    Customers SelectCustomer()
    {
        Customers customer = new Eagle();

        return customer;
    }

    //게임 상태 넘기기 버튼
    public void NextButton()
    {
        GameState = (GameState)(((int)GameState + 1) % 6);
    }
    //조합하기
    void Combine()
    {
        CurrentCocktail = CocktailManager.MakeCocktail(currentBaseMaterials, currentSubMaterials);
    }
    private void Start()
    {
        GameState = GameState.None;
    }
    public float CorrectCheck()
    {
        float satisfaction = 0f;
        float checkpoint = 0f;

        if (CurrentOrder.requiredCocktail != null)
        {
            checkpoint++;
            Debug.Log($"요구 칵테일 : {CurrentOrder.requiredCocktail} / 제공된 칵테일 : {CurrentCocktail.cocktailName}");
            if (CurrentCocktail.cocktailName == CurrentOrder.requiredCocktail) satisfaction += 100f;
        }
        if (CurrentOrder.requiredSweet != null)
        {
            checkpoint++;
            Debug.Log($"요구 당도 : {CurrentOrder.requiredSweet} / 제공된 당도 : {CurrentCocktail.Sweetness}");
            satisfaction += SatisfactionCheck(CurrentCocktail.Sweetness, (int)CurrentOrder.requiredSweet);
        }
        if (CurrentOrder.requiredProof != null)
        {
            checkpoint++;
            Debug.Log($"요구 도수 : {CurrentOrder.requiredProof} / 제공된 도수 : {CurrentCocktail.Proof}");
            satisfaction += SatisfactionCheck(CurrentCocktail.Proof, (int)CurrentOrder.requiredProof);
        }
        if (CurrentOrder.requiredFresh != null)
        {
            checkpoint++;
            Debug.Log($"요구 청량감 : {CurrentOrder.requiredFresh} / 제공된 청량감 : {CurrentCocktail.Refreshment}");
            satisfaction += SatisfactionCheck(CurrentCocktail.Refreshment, (int)CurrentOrder.requiredFresh);
        }
        
        if (checkpoint == 0) checkpoint = 1f;
        float successPoint = Mathf.Clamp(satisfaction / checkpoint, 0f, 100f);

        Debug.Log("만족도 : " + successPoint);
        return successPoint;
    }
    private float SatisfactionCheck(int current, int required)
    {
        int difference = Mathf.Abs(required - current);
        switch (difference)
        {
            case 0:
                return 100f;
            case 1:
                return 85f;
            case 2:
                return 60f;
            case 3:
                return 30f;
            default:
                return 0f;
        }
    }

    #endregion
}
