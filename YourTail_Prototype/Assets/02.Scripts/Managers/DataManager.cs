using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region 데이터베이스 세팅
    void Start()
    {
        Init();
    }

    void Init()
    {
        CurrentCustomer = null;


        if (CustomerList.Count > 0) CustomerList.Clear();
        if (BaseMaterialData.Count > 0) BaseMaterialData.Clear();
        if (SubMaterialData.Count > 0) SubMaterialData.Clear();
        if (CocktailData.Count > 0) CocktailData.Clear();
        if (CocktailList.Count > 0) CocktailList.Clear();

        AddCustomer(new Eagle());
        AddCustomer(new Dove());

        AddBase(new Rum());
        AddBase(new Brandy());

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
    
    public Customers GetRandomCustomer()
    {
        return CustomerList[UnityEngine.Random.Range(0, CustomerList.Count)];
    }
    public void SelectCustomer(Customers customer)
    {
        CurrentCustomer = customer;
        CurrentOrder = customer.GetOrder();
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
        if(customer == null) customer = CustomerNameData[order.CustomerName];
        Debug.Log($"{order.CustomerName} 의 현재 오더 : {customer.CurrentIndex + 1}");

        float satisfaction = 0f;
        float checkpoint = 0f;

        if (order.requiredCocktail != null)
        {
            checkpoint++;
            Debug.Log($"요구 칵테일 : {order.requiredCocktail} / 제공된 칵테일 : {cocktail.cocktailName}");
            if (cocktail.cocktailName == order.requiredCocktail) satisfaction += 100f;
        }
        if (order.requiredSweet != null)
        {
            checkpoint++;
            Debug.Log($"요구 당도 : {order.requiredSweet} / 제공된 당도 : {cocktail.Sweetness}");
            satisfaction += SatisfactionCheck(cocktail.Sweetness, (int)order.requiredSweet);
        }
        if (order.requiredProof != null)
        {
            checkpoint++;
            Debug.Log($"요구 도수 : {order.requiredProof} / 제공된 도수 : {cocktail.Proof}");
            satisfaction += SatisfactionCheck(cocktail.Proof, (int)order.requiredProof);
        }
        if (order.requiredFresh != null)
        {
            checkpoint++;
            Debug.Log($"요구 청량감 : {order.requiredFresh} / 제공된 청량감 : {cocktail.Refreshment}");
            satisfaction += SatisfactionCheck(cocktail.Refreshment, (int)order.requiredFresh);
        }

        if (checkpoint == 0) checkpoint = 1f;
        float successPoint = Mathf.Clamp(satisfaction / checkpoint, 0f, 100f);

        Debug.Log("만족도 : " + successPoint);
        
        if (successPoint >= 75f) customer.CurrentIndex++;

        Debug.Log($"{order.CustomerName} 의 평가 후 오더 : {customer.CurrentIndex + 1}");

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
}
