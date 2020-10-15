using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectionUI : UIBase_Popup
{
    enum Windows
    {
        CockCollection
    }
    enum Contents
    {
        CocktailRecipes
    }
    
    enum Buttons
    {
        CloseButton
    }

    enum Toggles
    {
        FilterGrade0,
        FilterGrade1,
        FilterGrade2,
        FilterGrade3,
        FilterGrade4,
        Count
    }

    RectTransform contents;
    List<Cocktail> cockList;
    List<Cocktail> filteringList = new List<Cocktail>();
    List<CocktailInfoCard> cocktailInfo = new List<CocktailInfoCard>();


    void Start() => Init();
    public override void Init()
    {
        base.Init();

        GameManager.Instance.ignoreOnMouse = true;

        Bind<GameObject>(typeof(Windows));
        Bind<RectTransform>(typeof(Contents));
        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        contents = Get<RectTransform>((int)Contents.CocktailRecipes);
        cockList = GameManager.Data.CocktailList;

        SetAllRecipes();
        SetButtons();
        SetToggles();

        SetPooling();

        GameManager.UI.ClosePopupUI<CollectionUI>();
    }
    void OnEnable()
    {
        if (!Inited || !DontDestroy)
            return;

        gameObject.SetCanvasOrder();
        GameManager.Instance.ignoreOnMouse = true;

        for (int i = 0; i < (int)Toggles.Count; i++)
            Get<Toggle>(i).isOn = false;
    }
    private void OnDisable()
    {
        GameManager.Instance.ignoreOnMouse = false;
    }

    private void OnDestroy()
    {
        ResetButtons();
        GameManager.Instance.ignoreOnMouse = false;
    }

    void SetToggles()
    {
        Get<Toggle>((int)Toggles.FilterGrade0).onValueChanged.AddListener(Filter0);
        Get<Toggle>((int)Toggles.FilterGrade1).onValueChanged.AddListener(Filter1);
        Get<Toggle>((int)Toggles.FilterGrade2).onValueChanged.AddListener(Filter2);
        Get<Toggle>((int)Toggles.FilterGrade3).onValueChanged.AddListener(Filter3);
        Get<Toggle>((int)Toggles.FilterGrade4).onValueChanged.AddListener(Filter4);
    }
    
    void SetAllRecipes()
    {
        cockList = GameManager.Data.CocktailList;
        for (int i = 0; i < cockList.Count; i++)
        {
            GameObject gameObject = GameManager.Resource.Instantiate("UI/Others/CocktailInfoCard", contents);
            CocktailInfoCard component = gameObject.GetOrAddComponent<CocktailInfoCard>();
            component.MyCocktail = cockList[i];
            cocktailInfo.Add(component);
        }

        int height = cockList.Count / 4;
        if ((cockList.Count % 4) != 0) height++;
        contents.sizeDelta = new Vector2(0f, (520f * height) + 20f);
    }
    void SetButtons()
    {
        GetButton((int)Buttons.CloseButton).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<CollectionUI>(); });
    }
    void SetFilteredRecipes()
    {
        foreach (CocktailInfoCard item in cocktailInfo)
        {
            if (filteringList.Contains(item.MyCocktail))
            {
                if (!item.gameObject.activeSelf) item.gameObject.SetActive(true);
            }
            else
            {
                if (item.gameObject.activeSelf) item.gameObject.SetActive(false);
            }
        }
        int height = filteringList.Count / 4;
        if ((filteringList.Count % 4) != 0) height++;
        contents.sizeDelta = new Vector2(0f, (520f * height) + 20f);
    }
    void AddFiltering(Cocktail cocktail)
    {
        if (!filteringList.Contains(cocktail))
            filteringList.Add(cocktail);
    }
    void RemoveFiltering(Cocktail cocktail)
    {
        if (!filteringList.Contains(cocktail))
            filteringList.Remove(cocktail);
    }
    void AddFilterByProof(int grade)
    {
        if (filteringList.Count == cockList.Count)
            filteringList.Clear();

        foreach (Cocktail item in cockList)
        {
            if (item.GetProofGradeToInt() == grade)
                AddFiltering(item);
        }
        SetFilteredRecipes();
    }
    void RemoveFilterByProof(int grade)
    {
        List<Cocktail> _deleteList = new List<Cocktail>();
        foreach (Cocktail item in filteringList)
        {
            if (item.GetProofGradeToInt() == grade)
                _deleteList.Add(item);
        }
        foreach (Cocktail item in _deleteList)
        {
            filteringList.Remove(item);
        }

        if(filteringList.Count > 0)
        {
            SetFilteredRecipes();
        }
        else
        {
            foreach (CocktailInfoCard item in cocktailInfo)
            {
                if (!item.gameObject.activeSelf) 
                    item.gameObject.SetActive(true);
            }
            int height = cockList.Count / 4;
            if ((cockList.Count % 4) != 0) height++;
            contents.sizeDelta = new Vector2(0f, (520f * height) + 20f);
        }
    }
    void Filter0(bool isChecking)
    {
        if (isChecking) AddFilterByProof(0);
        else RemoveFilterByProof(0);
    }
    void Filter1(bool isChecking)
    {
        if (isChecking) AddFilterByProof(1);
        else RemoveFilterByProof(1);
    }
    void Filter2(bool isChecking)
    {
        if (isChecking) AddFilterByProof(2);
        else RemoveFilterByProof(2);
    }
    void Filter3(bool isChecking)
    {
        if (isChecking) AddFilterByProof(3);
        else RemoveFilterByProof(3);
    }
    void Filter4(bool isChecking)
    {
        if (isChecking) AddFilterByProof(4);
        else RemoveFilterByProof(4);
    }
}
