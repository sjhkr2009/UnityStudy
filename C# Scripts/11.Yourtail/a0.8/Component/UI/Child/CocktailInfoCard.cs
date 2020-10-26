using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CocktailInfoCard : UIBase
{
    private Cocktail _myCocktail;
    public Cocktail MyCocktail
    {
        get => _myCocktail;
        set
        {
            _myCocktail = value;
            SetInfo(value);
        }
    }
    
    enum Texts
    {
        Spirit,
        Sub1,
        Sub2,
        Sub3,
        ProofText,
        CocktailName
    }

    enum Sliders
    {
        ProofSlider
    }

    enum Images
    {
        CocktailImage,
        ProofSliderColor
    }

    bool inited = false;
    private void Start() => Init();
    void Init()
    {
        if (inited) return;

        Bind<Text>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
        Bind<Image>(typeof(Images));

        inited = true;
    }

    void SetInfo(Cocktail cocktail)
    {
        Init();

        GetText((int)Texts.CocktailName).text = cocktail.Name_kr;
        GetImage((int)Images.CocktailImage).sprite = cocktail.image;

        float proof = cocktail.Proof / Define.CocktailMaxProof;
        Get<Slider>((int)Sliders.ProofSlider).value = proof;
        GetImage((int)Images.ProofSliderColor).color = Define.ProofToColor(proof);
        GetText((int)Texts.ProofText).text = $"{cocktail.Proof}%";

        GetText((int)Texts.Spirit).text = GameManager.Data.BaseMaterialIdData[cocktail.BaseIDList[0]].Name;
        
        int subIndex = (int)Texts.Sub1;
        foreach (string id in cocktail.SubIDList)
        {
            GetText(subIndex).text = GameManager.Data.SubMaterialIdData[id].Name;
            subIndex++;
        }
    }
}
