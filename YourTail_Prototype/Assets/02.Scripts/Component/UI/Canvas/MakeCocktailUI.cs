using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MakeCocktailUI : UIBase_Popup
{
    public Cocktail myCocktail = null;
    [SerializeField] Sprite makingImage;
    enum Texts
    {
        CocktailName,
        CocktailSweet,
        CocktailProof,
        CocktailFresh
    }
    
    enum Buttons
    {
        NextButton
    }
    
    enum Images
    {
        CocktailImage
    }
    
    void Start()
    {
        Init();
    }
    private void OnDestroy()
    {
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
    }
    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.SetCocktail; });

        if (makingImage == null)
            makingImage = GameManager.Resource.LoadImage(Define.ImageType.Cocktail, 0);

        Image cocktailImage = GetImage((int)Images.CocktailImage);
        cocktailImage.sprite = makingImage;

        cocktailImage.transform.DORotate(Vector3.forward * -720f, 2.5f, RotateMode.LocalAxisAdd).OnComplete(() =>
        {
            Debug.Log("칵테일 출력");
            myCocktail = GameManager.Data.CurrentCocktail;
            SetResult();
        });
    }

    void SetResult()
    {
        if (myCocktail == null) return;

        GetImage((int)Images.CocktailImage).sprite = myCocktail.image;
        GetText((int)Texts.CocktailName).text = $"{myCocktail.Name_kr}\n({myCocktail.Name_en})";
        GetText((int)Texts.CocktailSweet).text = IntToStarText(myCocktail.Sweetness);
        GetText((int)Texts.CocktailProof).text = IntToStarText(myCocktail.Proof);
        GetText((int)Texts.CocktailFresh).text = IntToStarText(myCocktail.Refreshment);
    }

    string IntToStarText(int value)
    {
        if (value <= 0) return "-";

        string result = "";
        for (int i = 0; i < value; i++)
            result += "★";

        return result;
    }
}
