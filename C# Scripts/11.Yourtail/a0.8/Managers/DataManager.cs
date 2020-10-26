using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DataManager
{
    public static ScriptableData GameData => GameManager.Resource.LoadDatabase();
    
    public List<Customers> CustomerList { get; private set; } = new List<Customers>();
    public Dictionary<string, Customers> CustomerNameData { get; private set; } = new Dictionary<string, Customers>();

    public Dictionary<string, BaseMaterials> BaseMaterialIdData { get; private set; } = new Dictionary<string, BaseMaterials>();
    public Dictionary<int, BaseMaterials> BaseMaterialIndexData { get; private set; } = new Dictionary<int, BaseMaterials>();
    public List<BaseMaterials> BaseMaterialList { get; private set; } = new List<BaseMaterials>();
    public Dictionary<string, SubMaterials> SubMaterialIdData { get; private set; } = new Dictionary<string, SubMaterials>();
    public Dictionary<int, SubMaterials> SubMaterialIndexData { get; private set; } = new Dictionary<int, SubMaterials>();
    public List<SubMaterials> SubMaterialList { get; private set; } = new List<SubMaterials>();
    public Dictionary<string, Cocktail> CocktailData { get; private set; } = new Dictionary<string, Cocktail>();
    public List<Cocktail> CocktailList { get; private set; } = new List<Cocktail>();



    #region 현재 저장중인 정보

    public List<Cocktail> Recipe { get; set; } = new List<Cocktail>();
    public List<SubMaterials> CollectedMaterial { get; set; } = new List<SubMaterials>();
    public int BirdCoin { get; private set; }
    public event Action<int> OnSetCoin = n => { };
    public void SetBirdCoin(int value)
    {
        BirdCoin = value;
        OnSetCoin(value);
    }
    public void AddBirdCoin(int value)
    {
        BirdCoin += value;
        OnSetCoin(BirdCoin);
    }

    [ShowInInspector] private Order _currentOrder { get; set; }
    public event Action<Order> OnSetOrder = n => { };
    public Order CurrentOrder
    {
        get => _currentOrder;
        set
        {
            _currentOrder = value;
            OnSetOrder(value);
        }
    }
    [ShowInInspector, ReadOnly] public int CurrentTableIndex { get; set; }
    [ShowInInspector] private Customers _currentCustomer { get; set; }
    public event Action<Customers> OnSetCustomer = n => { };
    public Action DeleteCustomer = () => { };
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
    public event Action<Cocktail> OnSetCocktail = n => { };
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
    [SerializeField] int maxSub = 3;

    public void AddCurrentBase(BaseMaterials item)
    {
        CurrentBaseMaterials.Add(item);
        OnAddBaseMaterial(item);
        SetValidMaterials();
    }
    public void RemoveCurrentBase(BaseMaterials item)
    {
        CurrentBaseMaterials.Remove(item);
        OnRemoveBaseMaterial(item);
        SetValidMaterials();
    }

    public void AddCurrentSub(SubMaterials item)
    {
        CurrentSubMaterials.Add(item);
        OnAddSubMaterial(item);
        SetValidMaterials();
    }
    public void RemoveCurrentSub(SubMaterials item)
    {
        CurrentSubMaterials.Remove(item);
        OnRemoveSubMaterial(item);
        SetValidMaterials();
    }
    public event Action<BaseMaterials> OnAddBaseMaterial = n => { };
    public event Action<BaseMaterials> OnRemoveBaseMaterial = n => { };
    public event Action<SubMaterials> OnAddSubMaterial = n => { };
    public event Action<SubMaterials> OnRemoveSubMaterial = n => { };
    public event Action OnValidUpdate = () => { };

    // 칵테일 완성 후 평가 점수. GOOD/SOSO/BAD 중 하나의 결과를 각각 1, 0, -1의 정수로 가지고 있습니다.
    public int CurrentGrade { get; private set; }

    public List<string> ValidMaterials { get; private set; } = new List<string>();
    List<Cocktail> ValidCocktails = new List<Cocktail>();

    #endregion

    #region 데이터 초기 세팅
    public void Init()
    {
        CurrentCustomer = null;

        maxBase = Define.MaxBaseMaterial;
        maxSub = Define.MaxSubMaterial;

        GameManager.Input.InputMaterialSelect -= SelectMaterial;
        GameManager.Input.InputMaterialInfo -= GameManager.UI.SetMaterialInfo;
        GameManager.Input.InputBirdInfo -= GameManager.UI.SetBirdInfo;

        GameManager.Input.InputMaterialSelect += SelectMaterial;
        GameManager.Input.InputMaterialInfo += GameManager.UI.SetMaterialInfo;
        GameManager.Input.InputBirdInfo += GameManager.UI.SetBirdInfo;

        SetCustomers();
        SetSpirits();
        SetSubMaterials();
        SetCocktails();

        SetBirdCoin(GameData.Birdcoin);
    }
    void SetCustomers()
    {
        if (CustomerList.Count > 0) CustomerList.Clear();
        if (CustomerNameData.Count > 0) CustomerNameData.Clear();

        AddCustomer(new Eagle());
        AddCustomer(new Parrot());
        AddCustomer(new Flamingo());
        AddCustomer(new Swan());
        AddCustomer(new Penguin());
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
        //AddSub(new Smt_TonicWater());

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
        AddCocktail(new Ckt_Sidecar());
        AddCocktail(new Ckt_AppleJack());
        AddCocktail(new Ckt_Olympic());
        AddCocktail(new Ckt_JackRose());

        //AddCocktail(new Ckt_Classic());
        AddCocktail(new Ckt_FrenchConnection());
        AddCocktail(new Ckt_HavardCooler());
        AddCocktail(new Ckt_CafeRoyal());
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
        BaseMaterialList.Add(item);
    }
    void AddSub(SubMaterials item)
    {
        SubMaterialIdData.Add(item.Id, item);
        SubMaterialIndexData.Add(item.Index, item);
        SubMaterialList.Add(item);
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
            case GameState.Select:
                CurrentCocktail = new Cocktail();
                break;
            case GameState.Combine:
                CurrentCocktail = MakeCocktail();
                CurrentCorrectCheck();
                break;
            case GameState.SetCocktail:
                CurrentCustomer.ResetOrder();
                SaveData();
                break;
        }
    }

    /// <summary>
    /// 모든 새들 중에서 임의의 새를 반환합니다.
    /// </summary>
    public Customers GetRandomCustomer()
    {
        return CustomerList[UnityEngine.Random.Range(0, CustomerList.Count)];
    }

    /// <summary>
    /// 새를 입력하면 해당 새의 주문을 받습니다. 현재 손님과 오더 정보가 세팅됩니다.
    /// </summary>
    /// <param name="customer">주문을 받을 새를 클래스로 입력하세요. new로 클래스를 생성하지 말고 DataManager에서 생성한 클래스여야 합니다.</param>
    public void SelectCustomer(Customers customer)
    {
        CurrentCustomer = customer;
        CurrentOrder = customer.GetOrderInLow(40f, 30f, 20f, 10f);
    }

    /// <summary>
    /// id를 받아서 그에 맞는 재료를 선택합니다. 이미 선택된 재료를 다시 선택하면 재료를 삭제합니다.
    /// </summary>
    /// <param name="id">주재료 또는 부재료의 ID</param>
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

    /// <summary>
    /// 선택된 재료를 통해 칵테일을 만듭니다. 테스트용이 아니라면 선택된 재료 리스트는 입력하지 않아도 됩니다.
    /// </summary>
    public Cocktail MakeCocktail(List<BaseMaterials> currentBases = null, List<SubMaterials> currentSubs = null)
    {
        Cocktail empty = new Cocktail();
        if (currentBases == null) currentBases = CurrentBaseMaterials;
        if (currentSubs == null) currentSubs = CurrentSubMaterials;

        foreach (Cocktail cocktail in CocktailList)
        {
            bool isCorrect = true;
            if (currentBases.Count != cocktail.BaseIDList.Count) continue;
            if (currentSubs.Count != cocktail.SubIDList.Count) continue;

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

    /// <summary>
    /// 현재 오더에서 요구하는 내용과 만들어진 칵테일을 비교하여 현재 점수를 CurrentGrade 변수에 기록해 둡니다.
    /// </summary>
    void CurrentCorrectCheck()
    {
        CurrentGrade = CorrectCheck(CurrentCocktail, CurrentOrder);
    }

    /// <summary>
    /// 칵테일과 오더를 비교하여 점수를 산출하고, GOOD/SOSO/BAD의 세 가지 결과로 분류합니다. 각각의 결과는 1, 0, -1의 값으로 반환됩니다.
    /// </summary>
    int CorrectCheck(Cocktail cocktail, Order order)
    {
        float score = 0f;
        int checkpoint = 0;

        if(order.requiredCocktail != null && order.requiredCocktail.Count > 0)
        {
            checkpoint++;
            score += order.requiredCocktail.Contains(cocktail.cocktailName) ? 100f : 0f;
        }
        if (order.requiredProofGrade != null && order.requiredProofGrade.Count > 0)
        {
            checkpoint++;
            score += order.requiredProofGrade.Contains(cocktail.GetProofGrade()) ? 100f : 0f;
        }
        if (order.requiredTag != null && order.requiredTag.Count > 0)
        {
            checkpoint++;
            score += ListMatchingRate(cocktail.Tags, order.requiredTag);
        }
        if(order.avoidProofGrade != null && order.avoidProofGrade.Count > 0)
        {
            checkpoint++;
            score += order.avoidProofGrade.Contains(cocktail.GetProofGrade()) ? 0f : 100f;
        }
        if (order.avoidTag != null && order.avoidTag.Count > 0)
        {
            checkpoint++;
            score += 100f - ListMatchingRate(cocktail.Tags, order.avoidTag);
        }

        float finalScore = Mathf.Clamp(score / checkpoint, 0f, 100f);

        Debug.Log($"결과 점수: {finalScore}");
        return Define.CocktailScoreToGrage(finalScore);
    }

    /// <summary>
    /// 칵테일이 가진 리스트와 오더에서 요구하는 리스트의 일치율을 0~100의 퍼센트로 반환합니다.
    /// * 만약 리스트 일치율 확인 로직이 다른 동작에서 추가로 요구될 경우, 별도의 스크립트로 옮길 것이 권장됩니다.
    /// </summary>
    float ListMatchingRate<T>(List<T> cocktail, List<T> order)
    {
        if(order == null || order.Count == 0)
        {
            Debug.Log("빈 칵테일의 일치율 확인을 시도했습니다.");
            return -1f;
        }

        int equalCount = 0;

        foreach (T item in order)
        {
            if (cocktail.Contains(item))
                equalCount++;
        }

        float rate = (float)equalCount / order.Count;
        return rate * 100f;
    }

    /// <summary>
    /// 현재 기록된 점수에 따라 보상을 획득합니다. 결과에 따라 주문을 한 새의 호감도가 상승하며 코인을 획득합니다.
    /// </summary>
    public void SetReward()
    {
        switch (CurrentGrade)
        {
            case 1:
                AddBirdCoin(1);
                AddExp(15);
                return;
            case 0:
                AddExp(8);
                return;
            case -1:
                AddExp(3);
                return;
            default:
                return;

        }
    }
    public int beforeExp;
    public int afterExp;
    public bool levelUp { get; set; }
    /// <summary>
    /// 현재 새의 호감도를 증가시킵니다. UI 표기를 위해 상승 전의 호감도와 상승 후의 호감도가 기록됩니다.
    /// </summary>
    public void AddExp(int value)
    {
        beforeExp = CurrentCustomer.Exp;
        CurrentCustomer.Exp += value;
        afterExp = CurrentCustomer.Exp;
    }

    void SetValidMaterials()
	{
        ValidCocktails.Clear();
        ValidMaterials.Clear();

        if ((CurrentBaseMaterials.Count + CurrentSubMaterials.Count) == 0)
        {
            OnValidUpdate();
            return;
        }

        List<CocktailMaterials> currentList = new List<CocktailMaterials>();

        foreach (var item in CurrentBaseMaterials)
            currentList.Add(item);

        foreach (var item in CurrentSubMaterials)
            currentList.Add(item);

        foreach (var cock in CocktailList)
        {
            bool isValid = true;
            foreach (var material in currentList)
            {
                if (((material.materialType == CocktailMaterials.MaterialType.Base) && !cock.BaseIDList.Contains(material.Id)) ||
                    ((material.materialType == CocktailMaterials.MaterialType.Sub) && !cock.SubIDList.Contains(material.Id)))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
                ValidCocktails.Add(cock);
        }

		foreach (var cock in ValidCocktails)
		{
			foreach (var Id in cock.SubIDList)
			{
                if (!ValidMaterials.Contains(Id))
                    ValidMaterials.Add(Id);
            }
		}

        OnValidUpdate();
    }

    #endregion

    #region 저장 및 리셋
    public void SaveToPlayerPrefs()
    {
        SaveData();

        //GameData.Save();
    }
    public void LoadFromPlayerPrefs()
    {
        //GameData.Load();

        LoadData();
    }
    [Button]
    public void SaveData()
    {
        foreach (Customers item in CustomerList)
        {
            GameData.SetLevel(item.Name, item.Level);
            GameData.SetExp(item.Name, item.Exp);
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
            CustomerNameData[item.Key].Level = item.Value;

        foreach (var item in GameData.CustomerExp)
            CustomerNameData[item.Key].Exp = item.Value;

        BirdCoin = GameData.Birdcoin;
        Recipe.Clear();
        foreach (string item in GameData.CollectedRecipe)
        {
            Recipe.Add(CocktailData[item]);
        }
    }

    void ResetBaseMaterial()
    {
        int count = CurrentBaseMaterials.Count;
        for (int i = 0; i < count; i++)
        {
            RemoveCurrentBase(CurrentBaseMaterials[0]);
        }
    }
    void ResetSubMaterial()
    {
        int count = CurrentSubMaterials.Count;
        for (int i = 0; i < count; i++)
        {
            RemoveCurrentSub(CurrentSubMaterials[0]);
        }
    }
    void ResultReset()
    {
        beforeExp = 0;
        afterExp = 0;
        levelUp = false;
    }
    public void ResetSelected()
    {
        ResetBaseMaterial();
        ResetSubMaterial();
        ResultReset();
        CurrentCocktail = null;
    }
    void CurrentReset()
    {
        CurrentCustomer = null;
        CurrentOrder = null;
        CurrentCocktail = null;
        CurrentBaseMaterials.Clear();
        CurrentSubMaterials.Clear();
        ResultReset();
    }

    public void Clear()
	{
        OnAddBaseMaterial = null;
        OnAddSubMaterial = null;
        OnRemoveBaseMaterial = null;
        OnRemoveSubMaterial = null;
        OnSetCocktail = null;
        OnSetCoin = null;
        OnSetCustomer = null;
        OnSetOrder = null;
        OnValidUpdate = null;
	}
    #endregion
}
