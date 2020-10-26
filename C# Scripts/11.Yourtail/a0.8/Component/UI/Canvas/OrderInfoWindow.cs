using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderInfoWindow : UIBase_Popup
{
    enum Transforms
    {
        OrderTextBG,
        CustomerImageBG
    }
    enum Texts
    {
        OrderText
    }
    enum Images
    {
        CustomerImage,
        background
    }
	enum Buttons
	{
        background
    }

    void Start() => Init();

    public override void Init()
    {
        base.Init();

        Bind<RectTransform>(typeof(Transforms));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        GetImage((int)Images.CustomerImage).sprite = GameManager.Data.CurrentCustomer.Image;
        GetText((int)Texts.OrderText).text = GameManager.Data.CurrentOrder.orderContents;
        GetButton((int)Buttons.background).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<OrderInfoWindow>(); });

        hasDestroyMotion = true;
        destroyTime = 0.33f;
    }

    public override void OnDestroyMotion()
    {
        base.OnDestroyMotion();

        GetText((int)Texts.OrderText).text = "";
        GetImage((int)Images.background).DOFade(0f, 0.3f);
        Get<RectTransform>((int)Transforms.OrderTextBG).DOScale(0f, 0.2f);
        DOVirtual.DelayedCall(0.1f, () => { Get<RectTransform>((int)Transforms.CustomerImageBG).DOScale(0f, 0.2f); });
        ResetButtons();
    }

    private void OnDestroy()
    {
        GetImage((int)Images.background).DOKill();
        Get<RectTransform>((int)Transforms.OrderTextBG).DOKill();
        Get<RectTransform>((int)Transforms.CustomerImageBG).DOKill();
    }
}
