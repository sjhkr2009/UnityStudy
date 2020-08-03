using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    void Start() => Init();
    public override void Init()
    {
        base.Init();

        GameManager.Instance.ignoreOnMouse = true;

        Bind<GameObject>(typeof(Windows));
        Bind<RectTransform>(typeof(Contents));
        Bind<Button>(typeof(Buttons));

        SetRecipes();
        SetButtons();
    }
    private void OnDestroy()
    {
        ResetButtons();
        GameManager.Instance.ignoreOnMouse = false;
    }

    void SetRecipes()
    {
        List<Cocktail> list = GameManager.Data.CocktailList;
        RectTransform parent = Get<RectTransform>((int)Contents.CocktailRecipes);
        for (int i = 0; i < list.Count; i++)
        {
            GameObject gameObject = GameManager.Resource.Instantiate("UI/Others/CocktailInfoCard", parent);
            gameObject.GetOrAddComponent<CocktailInfoCard>().MyCocktail = list[i];
        }

        int height = list.Count / 4;
        parent.sizeDelta = new Vector2(0f, (520f * height) + 20f);
    }
    void SetButtons()
    {
        GetButton((int)Buttons.CloseButton).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<CollectionUI>(); });
    }
}
