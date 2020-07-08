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
        CocktailProof,
        CocktailTag,
        CocktailInfo
    }
    
    enum Buttons
    {
        NextButton
    }
    
    enum Images
    {
        CocktailImage,
        ProofSliderColor
    }

    enum Sliders
    {
        ProofSlider
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
        Bind<Slider>(typeof(Sliders));

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

        float proofNormalize = myCocktail.Proof / Define.CocktailMaxProof;
        Get<Slider>((int)Sliders.ProofSlider).DOValue(proofNormalize, 0.7f).
            OnComplete(() =>
            {
                GetText((int)Texts.CocktailProof).text = $"{myCocktail.Proof}%";
            });
        GetImage((int)Images.ProofSliderColor).DOColor(ProofToColor(proofNormalize), 0.7f);

        GetText((int)Texts.CocktailTag).text = ListToString(myCocktail.GetTagToString());
        GetText((int)Texts.CocktailInfo).text = myCocktail.Info;
    }

    string ListToString(List<string> list)
    {
        string result = "";
        for (int i = 0; i < list.Count; i++)
        {
            result += list[i];
            if (i != list.Count - 1) result += ", ";
        }
        return result;
    }
    Color ProofToColor(float proof)
    {
        float hue = Mathf.Clamp(0.5f - (proof * 0.6f), 0f, 0.5f);
        return Color.HSVToRGB(hue, 0.5f, 1f);
    }
}
