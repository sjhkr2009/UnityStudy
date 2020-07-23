using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DataManager : MonoBehaviour
{
    public static ScriptableData GameData => GameManager.Resource.LoadDatabase();
    public static ScriptableTexts DialogData => GameManager.Resource.LoadDialogData();
    public List<Customers> CustomerList { get; private set; } = new List<Customers>();
    public Dictionary<string, Customers> CustomerNameData { get; private set; } = new Dictionary<string, Customers>();

    public Dictionary<string, BaseMaterials> BaseMaterialIdData { get; private set; } = new Dictionary<string, BaseMaterials>();
    public Dictionary<int, BaseMaterials> BaseMaterialIndexData { get; private set; } = new Dictionary<int, BaseMaterials>();
    public Dictionary<string, SubMaterials> SubMaterialIdData { get; private set; } = new Dictionary<string, SubMaterials>();
    public Dictionary<int, SubMaterials> SubMaterialIndexData { get; private set; } = new Dictionary<int, SubMaterials>();
    public Dictionary<string, Cocktail> CocktailData { get; private set; } = new Dictionary<string, Cocktail>();
    public List<Cocktail> CocktailList { get; private set; } = new List<Cocktail>();


    // 현재 정보를 요청하는 재료의 클래스. 재료 선택 화면에서 ? 버튼을 눌러 정보를 확인할 때 사용한다.
    public CocktailMaterials CurrentMaterialInfo { get; set; }
    void SetMaterialInfo(CocktailMaterials material) => CurrentMaterialInfo = material;

    #region 현재 저장중인 정보

    public List<Cocktail> Recipe { get; set; } = new List<Cocktail>();
    public List<SubMaterials> CollectedMaterial { get; set; } = new List<SubMaterials>();
    public int BirdCoin { get; private set; }
    public Action<int> OnSetCoin = n => { };
    public void SetBirdCoin(int value)
    {
        BirdCoin = value;
        OnSetCoin(value); //UI 갱신
    }
    public void AddBirdCoin(int value)
    {
        BirdCoin += value;
        OnSetCoin(BirdCoin); //UI 갱신
    }

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

    public int CurrentGrade { get; private set; }

    #endregion

    #region 데이터 초기 세팅
    void Start() => Init();
    void Init()
    {
        CurrentCustomer = null;
        DialogData.DialogSetting();

        GameManager.Input.InputMaterialSelect -= SelectMaterial;
        GameManager.Input.InputMaterialInfo -= SetMaterialInfo;

        GameManager.Input.InputMaterialSelect += SelectMaterial;
        GameManager.Input.InputMaterialInfo += SetMaterialInfo;

        SetCustomers();
        SetSpirits();
        SetSubMaterials();
        SetCocktails();
    }
    void SetCustomers()
    {
        if (CustomerList.Count > 0) CustomerList.Clear();
        if (CustomerNameData.Count > 0) CustomerNameData.Clear();

        AddCustomer(new Eagle());
        AddCustomer(new Goni());
    }
    void SetSpirits()
    {
        if (BaseMaterialIdData.Count > 0) BaseMaterialIdData.Clear();
        if (BaseMaterialIndexData.Count > 0) BaseMaterialIndexData.Clear();

        AddBase(new Bmt_Tequilla());
        AddBase(new Bmt_Vodka());
        AddBase(new Bmt_Whisky());
        AddBase(new Bmt_Gin());
        AddBase(new Bmt_Rum());

        AddBase(new Bmt_Brandy());
    }
    void SetSubMaterials()
    {
        if (SubMaterialIdData.Count > 0) SubMaterialIdData.Clear();
        if (SubMaterialIndexData.Count > 0) SubMaterialIndexData.Clear();

        AddSub(new Smt_OrangeJuice());
        AddSub(new Smt_LimeJuice());
        AddSub(new Smt_LemonJuice());
        AddSub(new Smt_GrenadineSyrup());
        AddSub(new Smt_TonicWater());

        AddSub(new Smt_OrangeLiqueur());
        AddSub(new Smt_SodaWater());
        AddSub(new Smt_Amaretto());
        AddSub(new Smt_GingerBeer());
        AddSub(new Smt_CoffeeLiqueur());

        AddSub(new Smt_Vermouth());
        AddSub(new Smt_Mint());
        AddSub(new Smt_Sugar());
        AddSub(new Smt_Campari());
        AddSub(new Smt_CherryLiqueur());

        AddSub(new Smt_Cola());
    }
    void SetCocktails()
    {
        if (CocktailData.Count > 0) CocktailData.Clear();
        if (CocktailList.Count > 0) CocktailList.Clear();

        AddCocktail(new Ckt_TequillaSunrise());
        AddCocktail(new Ckt_TequillaTonic());
        AddCocktail(new Ckt_Margarita());
        AddCocktail(new Ckt_Paloma());
        AddCocktail(new Ckt_GodMother());

        AddCocktail(new Ckt_MoscowMule());
        AddCocktail(new Ckt_VodkaTonic());
        AddCocktail(new Ckt_BlackRussian());
        AddCocktail(new Ckt_Screwdriver());
        AddCocktail(new Ckt_Cosmopolitan());

        AddCocktail(new Ckt_GodFather());
        AddCocktail(new Ckt_Manhattan());
        AddCocktail(new Ckt_MintJulep());
        AddCocktail(new Ckt_WhiskeySour());
        AddCocktail(new Ckt_HighBall());

        AddCocktail(new Ckt_Negroni());
        AddCocktail(new Ckt_DryMartini());
        AddCocktail(new Ckt_Aviation());
        AddCocktail(new Ckt_GinTonic());
        AddCocktail(new Ckt_PinkLady());

        AddCocktail(new Ckt_Daiquiri());
        AddCocktail(new Ckt_Mojito());
        AddCocktail(new Ckt_Bacardi());
        AddCocktail(new Ckt_CubaLibre());
        AddCocktail(new Ckt_Gimlet());

        AddCocktail(new Ckt_OrangeBlossom());
        AddCocktail(new Ckt_GinDaisy());
        AddCocktail(new Ckt_GinBuck());
        AddCocktail(new Ckt_GinFizz());
        AddCocktail(new Ckt_CampariCocktail());

        AddCocktail(new Ckt_KissInTheDark());
        AddCocktail(new Ckt_WhiteLady());
        AddCocktail(new Ckt_Ambassador());
        AddCocktail(new Ckt_TequilaSunset());
        AddCocktail(new Ckt_Mexicola());

        AddCocktail(new Ckt_FrozenMargarita());
    }
    
    void AddCustomer(Customers item)
    {
        CustomerList.Add(item);
        CustomerNameData.Add(item.Name, item);
    }
    void AddBase(BaseMaterials item)
    {
        BaseMaterialIdData.Add(item.Id, item);
        BaseMaterialIndexData.Add(item.Index, item);
    }
    void AddSub(SubMaterials item)
    {
        SubMaterialIdData.Add(item.Id, item);
        SubMaterialIndexData.Add(item.Index, item);
    }
    void AddCocktail(Cocktail item)
    {
        string id = item.Id;
        CocktailData.Add(id, item);
        CocktailList.Add(item);
    }
    #endregion

    #region 게임 진행 관련
    public void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Idle:
                CurrentReset();
                break;
            case GameState.Order:
                break;
            case GameState.SelectBase:
                break;
            case GameState.SelectSub:
                break;
            case GameState.Combine:
                CurrentCocktail = MakeCocktail(CurrentBaseMaterials, CurrentSubMaterials);
                CurrentCorrectCheck();
                break;
            case GameState.SetCocktail:
                SetReward();
                SaveData();
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
        if(BaseMaterialIdData.TryGetValue(id, out selectedBase))
        {
            if (CurrentBaseMaterials.Contains(selectedBase))
            {
                RemoveCurrentBase(selectedBase);
                return;
            }
            if (CurrentBaseMaterials.Count >= maxBase) RemoveCurrentBase(CurrentBaseMaterials[maxBase - 1]);
            AddCurrentBase(selectedBase);
        }
        else if(SubMaterialIdData.TryGetValue(id, out selectedSub))
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
    void CurrentCorrectCheck()
    {
        CurrentGrade = CorrectCheck(CurrentCocktail, CurrentOrder);
    }

    int CorrectCheck(Cocktail cocktail, Order order, Customers customer = null)
    {
        if (customer == null) customer = CustomerNameData[order.CustomerName];

        float score = 0f;
        int checkpoint = 0;

        if(order.requiredCocktail != null)
        {
            checkpoint++;
            if (order.requiredCocktail == cocktail.cocktailName) score += 100f;
        }

        if(order.requiredProofGrade != null)
        {
            checkpoint++;
            int difference = Mathf.Abs((int)order.requiredProofGrade - cocktail.GetProofGrade());
            score += Mathf.Clamp((100f - 10 * (difference * difference)), 0f, 100f);
        }

        if(order.requiredTag != null && order.requiredTag.Count > 0)
        {
            checkpoint++;
            float _score = 0;
            foreach (Define.CocktailTag tag in order.requiredTag)
            {
                if (cocktail.Tags.Contains(tag)) _score += 100f;
            }
            score += _score / order.requiredTag.Count;
        }

        float finalScore = Mathf.Clamp(score / (float)checkpoint, 0f, 100f);

        int result = 0;
        if (finalScore > Define.Evaluate_GoodHigherThan) result = 1;
        else if (finalScore < Define.Evaluate_BadLowerThan) result = -1;

        return result;
    }

    void SetReward()
    {
        switch (CurrentGrade)
        {
            case 1:
                AddBirdCoin(1);
                return;
            default:
                return;

        }
    }

    #endregion

    #region 저장 및 리셋
    public void SaveToPlayerPrefs()
    {
        SaveData();

        GameData.Save();
    }
    public void LoadFromPlayerPrefs()
    {
        GameData.Load();

        LoadData();
    }
    [Button]
    public void SaveData()
    {
        foreach (Customers item in CustomerList)
        {
            GameData.SetLevel(item.Name, item.Level);
        }
        GameData.SetBirdcoin(BirdCoin);
        foreach (Cocktail item in Recipe)
        {
            GameData.SetRecipe(item.Id);
        }
    }
    [Button]
    public void LoadData()
    {
        foreach (var item in GameData.CustomerLevel)
        {
            CustomerNameData[item.Key].Level = item.Value;
        }
        BirdCoin = GameData.Birdcoin;
        Recipe.Clear();
        foreach (string item in GameData.CollectedRecipe)
        {
            Recipe.Add(CocktailData[item]);
        }
    }

    public void OnRetry()
    {
        CurrentBaseMaterials.Clear();
        CurrentSubMaterials.Clear();
        CurrentCocktail = null;
    }
    void CurrentReset()
    {
        CurrentCustomer = null;
        CurrentOrder = null;
        CurrentCocktail = null;
        CurrentBaseMaterials.Clear();
        CurrentSubMaterials.Clear();
    }
    #endregion
}
