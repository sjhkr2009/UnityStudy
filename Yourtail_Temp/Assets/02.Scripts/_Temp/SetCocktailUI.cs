using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SetCocktailUI : UIBase_Popup
{
    enum Buttons
    {
        NextButton
    }

    enum Texts
    {
        GradeText,
        CoinText,
        LevelText
    }

    enum Images
    {
        CustomerImage,
        ExpImage
    }
    int grade;
    DataManager data = GameManager.Data;

    Image customerImage;

    void Start() => Init();

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        data = GameManager.Data;
        customerImage = GetImage((int)Images.CustomerImage);

        SetReward();
        SetCustomer();
        FillExpBar(data.beforeExp, data.afterExp, data.levelUp);

        SetPooling();
    }
    private void OnEnable()
    {
        if (!Inited) return;

        gameObject.SetCanvasOrder();
        GetText((int)Texts.LevelText).text = "";
        SetReward();
        SetCustomer();
        FillExpBar(data.beforeExp, data.afterExp, data.levelUp);
    }
	private void OnDisable()
	{
        GetImage((int)Images.ExpImage).DOKill();

    }
	private void OnDestroy()
    {
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
    }
    void SetReward()
    {
        data.SetReward();

        grade = data.CurrentGrade;
        GetText((int)Texts.GradeText).text = GradeToText();

        GetButton((int)Buttons.NextButton).onClick.AddListener(GameManager.Instance.SetDialog);

        if (grade == 1) GetText((int)Texts.CoinText).text = "코인 획득!";
        else GetText((int)Texts.CoinText).text = "";
    }
    void SetCustomer()
    {
        customerImage.sprite = data.CurrentCustomer.Image;
        customerImage.SetNativeSize();
    }
    string GradeToText()
    {
        switch (grade)
        {
            case -1:
                return "BAD";
            case 0:
                return "Soso";
            case 1:
                return "GOOD!";
            default:
                return "Error";
        }
    }
    void FillExpBar(int before, int after, bool levelUp)
    {
        Image fill = GetImage((int)Images.ExpImage);
        Button button = GetButton((int)Buttons.NextButton);
        button.interactable = false;

        if (!levelUp)
        {
            int required = Define.RequiredEXP[data.CurrentCustomer.Level];
            float beforePercent = ((float)before / required);
            float afterPercent = ((float)after / required);
            fill.fillAmount = beforePercent;
            fill.DOFillAmount(afterPercent, 1f).OnComplete(() =>
            {
                GetText((int)Texts.LevelText).text = $"호감도 변화: {(int)(beforePercent * 100)}% → {(int)(afterPercent * 100)}%";
                button.interactable = true;
            });
        }
        else
        {
            float beforePercent = (float)before / Define.RequiredEXP[data.CurrentCustomer.Level - 1];

            fill.fillAmount = (float)before / Define.RequiredEXP[data.CurrentCustomer.Level - 1];
            fill.DOFillAmount(1f, 1f).OnComplete(() =>
            {
                fill.fillAmount = 0f;
                fill.DOFillAmount((float)after / Define.RequiredEXP[data.CurrentCustomer.Level], 1f).OnComplete(() =>
                {
                    button.interactable = true;
                });
                GetText((int)Texts.LevelText).text = $"호감도 상승! ({data.CurrentCustomer.Level - 1} → {data.CurrentCustomer.Level})";

                GetText((int)Texts.LevelText).transform.localScale *= 0.8f;
                GetText((int)Texts.LevelText).transform.DOScale(1f, 0.2f);
            }).SetEase(Ease.Linear);

        }


    }
}
