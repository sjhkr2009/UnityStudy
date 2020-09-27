using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MakeCocktailUI : UIBase_Popup
{
    public Cocktail myCocktail = null;
    [SerializeField] Sprite makingImage;
    CocktailMaking makingUI;
    enum Texts
    {
        CocktailName,
        CocktailProof,
        CocktailInfo
    }
    
    enum Buttons
    {
        NextButton,
        RetryButton
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
    Canvas myCanvas;
    int originOrder;
    void Start() => Init();
    private void OnDestroy()
    {
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.RetryButton).onClick.RemoveAllListeners();

        makingUI.OnEndMaking -= SetResult;
    }
    public override void Init()
    {
        base.Init();

        myCanvas = gameObject.GetComponent<Canvas>();
        originOrder = myCanvas.sortingOrder;
        myCanvas.sortingOrder = -1;

        makingUI = GameManager.UI.OpenPopupUI<CocktailMaking>();
        makingUI.OnEndMaking -= SetResult;
        makingUI.OnEndMaking += SetResult;

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<Slider>(typeof(Sliders));

        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.SetCocktail; });
        GetButton((int)Buttons.RetryButton).onClick.AddListener(() => { GameManager.Input.InRetryCocktail(); });

        if (makingImage == null)
            makingImage = GameManager.Resource.LoadImage(Define.ImageType.Cocktail, 0);

        Image cocktailImage = GetImage((int)Images.CocktailImage);
        cocktailImage.sprite = makingImage;
    }

    void SetResult()
    {
        myCanvas.sortingOrder = originOrder;
        GameManager.UI.TryClosePopupUI<CocktailMaking>();

        myCocktail = GameManager.Data.CurrentCocktail;
        if (myCocktail == null) return;

        GetImage((int)Images.CocktailImage).sprite = myCocktail.image;
        GetText((int)Texts.CocktailName).text = $"{myCocktail.Name_kr}\n({myCocktail.Name_en})";

        float proofNormalize = myCocktail.Proof / Define.CocktailMaxProof;
        Get<Slider>((int)Sliders.ProofSlider).DOValue(proofNormalize, 0.7f).
            OnComplete(() =>
            {
                GetText((int)Texts.CocktailProof).text = $"{myCocktail.Proof}%";
            });
        GetImage((int)Images.ProofSliderColor).DOColor(Define.ProofToColor(proofNormalize), 0.7f);

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
}
