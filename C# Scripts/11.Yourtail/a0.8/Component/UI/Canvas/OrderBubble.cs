using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class OrderBubble : UIBase_Popup
{
    enum Texts
    {
        OrderText
    }
    enum Buttons
    {
        GetOrder,
        Panel
    }
    
    void Start()
    {
        Init();
    }

    public TablesUI tablesUI;
    string orderText;
    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        orderText = GameManager.Data.CurrentOrder.orderContents;
        GetText((int)Texts.OrderText).DOText(orderText, orderText.Length * Define.DoTextSpeed);

        EventHandler eventHandler = GetButton((int)Buttons.GetOrder).gameObject.GetOrAddComponent<EventHandler>();
        eventHandler.EventOnClick -= OnClickGetOrder;
        eventHandler.EventOnClick += OnClickGetOrder;

        GetButton((int)Buttons.Panel).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<OrderBubble>(); });
    }

    void OnClickGetOrder(PointerEventData evt)
    {
        if (GameManager.Instance.GameState != GameState.Idle) return;

        Text text = GetText((int)Texts.OrderText);
        if (text.text != orderText)
        {
            text.DOKill();
            text.text = orderText;
        }
        GameManager.Instance.GameState = GameState.Select;
    }

    private void OnDestroy()
    {
        if (tablesUI != null) tablesUI.CancelOrder();

        DOTween.Kill(GetText((int)Texts.OrderText));
        ResetButtons();
    }
}
