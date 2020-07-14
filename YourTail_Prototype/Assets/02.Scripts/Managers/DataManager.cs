using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DataManager : MonoBehaviour
{
    public List<Customers> CustomerList { get; private set; } = new List<Customers>();
    public Dictionary<string, Customers> CustomerNameData { get; private set; } = new Dictionary<string, Customers>();

    public Dictionary<string, BaseMaterials> BaseMaterialData { get; private set; } = new Dictionary<string, BaseMaterials>();
    public List<BaseMaterials> BaseMaterialList { get; private set; } = new List<BaseMaterials>();
    public Dictionary<string, SubMaterials> SubMaterialData { get; private set; } = new Dictionary<string, SubMaterials>();
    public List<SubMaterials> SubMaterialList { get; private set; } = new List<SubMaterials>();
    public Dictionary<string, Cocktail> CocktailData { get; private set; } = new Dictionary<string, Cocktail>();
    public List<Cocktail> CocktailList { get; private set; } = new List<Cocktail>();


    // 현재 정보를 요청하는 재료의 클래스. 재료 선택 화면에서 ? 버튼을 눌러 정보를 확인할 때 사용한다.
    public CocktailMaterials CurrentMaterialInfo { get; set; }
    void SetMaterialInfo(CocktailMaterials material) => CurrentMaterialInfo = material;

    #region 현재 정보
    [ShowInInspector] private Order _currentOrder { get; set; }
    public Action<Order> OnSetOrder = n => { };
    public Order CurrentOrder
    {
        get => _currentOrder;
        set
        {
            _currentOrder = value;
            OnSetOrder(value);
        }
    }
    [ShowInInspector] private Customers _currentCustomer { get; set; }
    public Action<Customers> OnSetCustomer = n => { };
    public Customers CurrentCustomer
    {
        get => _currentCustomer;
        set
        {
            _currentCustomer = value;
            if (value != null) OnSetCustomer(value);
        }
    }
    [ShowInInspector] private Cocktail _currentCocktail { get; set; }
    public Action<Cocktail> OnSetCocktail = n => { };
    public Cocktail CurrentCocktail
    {
        get => _currentCocktail;
        set
        {
            _currentCocktail = value;
            OnSetCocktail(value);
        }
    }

    [ShowInInspector] public List<BaseMaterials> CurrentBaseMaterials { get; private set; } = new List<BaseMaterials>();
    [ShowInInspector] public List<SubMaterials> CurrentSubMaterials { get; private set; } = new List<SubMaterials>();
    [SerializeField, ReadOnly] int maxBase = 1;
    [SerializeField] int maxSub = 2;

    public void AddCurrentBase(BaseMaterials item)
    {
        CurrentBaseMaterials.Add(item);
        OnAddBaseMaterial(item);
    }
    public void RemoveCurrentBase(BaseMaterials item)
    {
        CurrentBaseMaterials.Remove(item);
        OnRemoveBaseMaterial(item);
    }

    public void AddCurrentSub(SubMaterials item)
    {
        CurrentSubMaterials.Add(item);
        OnAddSubMaterial(item);
    }
    public void RemoveCurrentSub(SubMaterials item)
    {
        CurrentSubMaterials.Remove(item);
        OnRemoveSubMaterial(item);
    }
    public Action<BaseMaterials> OnAddBaseMaterial = n => { };
    public Action<BaseMaterials> OnRemoveBaseMaterial = n => { };
    public Action<SubMaterials> OnAddSubMaterial = n => { };
    public Action<SubMaterials> OnRemoveSubMaterial = n => { };

    #endregion

    #region 데이터베이스 세팅
    void Start()
    {
        Init();
    }

    void Init()
    {
        CurrentCustomer = null;

        GameManager.Input.InputMaterialSelect -= SelectMaterial;
        GameManager.Input.InputMaterialInfo -= SetMaterialInfo;

        GameManager.Input.InputMaterialSelect += SelectMaterial;
        GameManager.Input.InputMaterialInfo += SetMaterialInfo;

        if (CustomerList.Count > 0) CustomerList.Clear();
        if (BaseMaterialData.Count > 0) BaseMaterialData.Clear();
        if (SubMaterialData.Count > 0) SubMaterialData.Clear();
        if (CocktailData.Count > 0) CocktailData.Clear();
        if (CocktailList.Count > 0) CocktailList.Clear();

        AddCustomer(new Eagle());
        AddCustomer(new Dove());

        AddBase(new Tequilla());
        AddBase(new Vodka());

        AddSub(new Curacao());
        AddSub(new Pineapple());
        AddSub(new Lime());
        AddSub(new Lemon());

        AddCocktail(new BetweenTheSheets());
        AddCocktail(new BlueHawaii());
    }
    
    void AddCustomer(Customers item)
    {
        CustomerList.Add(item);
        CustomerNameData.Add(item.Name, item);
    }
    void AddBase(BaseMaterials item)
    {
        string id = item.Id;
        BaseMaterialData.Add(id, item);
        BaseMaterialList.Add(item);
    }
    void AddSub(SubMaterials item)
    {
        string id = item.Id;
        SubMaterialData.Add(id, item);
        SubMaterialList.Add(item);
    }
    void AddCocktail(Cocktail item)
    {
        string id = item.Id;
        CocktailData.Add(id, item);
        CocktailList.Add(item);
    }
    #endregion
    
    public void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Order:
                break;
            case GameState.SelectBase:
                break;
            case GameState.SelectSub:
                break;
            case GameState.Combine:
                CurrentCocktail = MakeCocktail(CurrentBaseMaterials, CurrentSubMaterials);
                Debug.Log($"조합: {CurrentCocktail.cocktailName} 칵테일");
                break;
            case GameState.SetCocktail:
                CurrentCorrectCheck();
                DOVirtual.DelayedCall(0.5f, CurrentReset);
                break;
        }
    }

    public Customers GetRandomCustomer()
    {
        return CustomerList[UnityEngine.Random.Range(0, CustomerList.Count)];
    }
    public void SelectCustomer(Customers customer)
    {
        CurrentCustomer = customer;
        CurrentOrder = customer.GetOrder();
    }

    public void SelectMaterial(string id)
    {
        BaseMaterials selectedBase = null;
        SubMaterials selectedSub = null;
        if(BaseMaterialData.TryGetValue(id, out selectedBase))
        {
            if (CurrentBaseMaterials.Contains(selectedBase))
            {
                RemoveCurrentBase(selectedBase);
                return;
            }
            if (CurrentBaseMaterials.Count >= maxBase) RemoveCurrentBase(CurrentBaseMaterials[maxBase - 1]);
            AddCurrentBase(selectedBase);
        }
        else if(SubMaterialData.TryGetValue(id, out selectedSub))
        {
            if (CurrentSubMaterials.Contains(selectedSub))
            {
                RemoveCurrentSub(selectedSub);
                return;
            }
            if (CurrentSubMaterials.Count >= maxSub) RemoveCurrentSub(CurrentSubMaterials[maxSub - 1]);
            AddCurrentSub(selectedSub);
        }
    }
    public Cocktail MakeCocktail(List<BaseMaterials> currentBases, List<SubMaterials> currentSubs)
    {
        Cocktail empty = new Cocktail();

        foreach (Cocktail cocktail in CocktailList)
        {
            bool isCorrect = true;
            if (currentBases.Count != cocktail.BaseMaterials.Count) continue;
            if (currentSubs.Count != cocktail.SubMaterials.Count) continue;

            foreach (BaseMaterials _base in currentBases)
            {
                if (!cocktail.BaseIDList.Contains(_base.Id))
                {
                    isCorrect = false;
                    break;
                }
            }
            
            if (!isCorrect) continue;

            foreach (SubMaterials _sub in currentSubs)
            {
                if (!cocktail.SubIDList.Contains(_sub.Id))
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect) return cocktail;
        }

        return empty;
    }
    public float CurrentCorrectCheck()
    {
        return CorrectCheck(CurrentCocktail, CurrentOrder);
    }

    float CorrectCheck(Cocktail cocktail, Order order, Customers customer = null)
    {
        if (customer == null) customer = CustomerNameData[order.CustomerName];

        int score = 0;

        if(order.requiredCocktail != null)
        {

        }

        return 0;
    }

    #region 폐기된 로직
    //float CorrectCheck(Cocktail cocktail, Order order, Customers customer = null)
    //{
    //    if(customer == null) customer = CustomerNameData[order.CustomerName];
    //    Debug.Log($"{order.CustomerName} 의 현재 오더 : {customer.CurrentIndex + 1}");

    //    float satisfaction = 0f;
    //    float checkpoint = 0f;

    //    if (order.requiredCocktail != null)
    //    {
    //        checkpoint++;
    //        Debug.Log($"요구 칵테일 : {order.requiredCocktail} / 제공된 칵테일 : {cocktail.cocktailName}");
    //        if (cocktail.cocktailName == order.requiredCocktail) satisfaction += 100f;
    //    }
    //    if (order.requiredSweet != null)
    //    {
    //        checkpoint++;
    //        Debug.Log($"요구 당도 : {order.requiredSweet} / 제공된 당도 : {cocktail.Sweetness}");
    //        satisfaction += SatisfactionCheck(cocktail.Sweetness, (int)order.requiredSweet);
    //    }
    //    if (order.requiredProof != null)
    //    {
    //        checkpoint++;
    //        Debug.Log($"요구 도수 : {order.requiredProof} / 제공된 도수 : {cocktail.Proof}");
    //        satisfaction += SatisfactionCheck(cocktail.Proof, (int)order.requiredProof);
    //    }
    //    if (order.requiredFresh != null)
    //    {
    //        checkpoint++;
    //        Debug.Log($"요구 청량감 : {order.requiredFresh} / 제공된 청량감 : {cocktail.Refreshment}");
    //        satisfaction += SatisfactionCheck(cocktail.Refreshment, (int)order.requiredFresh);
    //    }

    //    if (checkpoint == 0) checkpoint = 1f;
    //    float successPoint = Mathf.Clamp(satisfaction / checkpoint, 0f, 100f);

    //    Debug.Log("만족도 : " + successPoint);

    //    if (successPoint >= 75f) customer.CurrentIndex++;

    //    Debug.Log($"{order.CustomerName} 의 평가 후 오더 : {customer.CurrentIndex + 1}");

    //    return successPoint;
    //}
    //private float SatisfactionCheck(int current, int required)
    //{
    //    int difference = Mathf.Abs(required - current);
    //    switch (difference)
    //    {
    //        case 0:
    //            return 100f;
    //        case 1:
    //            return 85f;
    //        case 2:
    //            return 60f;
    //        case 3:
    //            return 30f;
    //        default:
    //            return 0f;
    //    }
    //}
    #endregion
    void CurrentReset()
    {
        CurrentCustomer = null;
        CurrentOrder = null;
        CurrentCocktail = null;
        CurrentBaseMaterials.Clear();
        CurrentSubMaterials.Clear();
    }
}
