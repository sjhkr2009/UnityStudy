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
        GetOrder
    }
    
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        string orderText = GameManager.Data.CurrentOrder.orderContents;
        GetText((int)Texts.OrderText).DOText(orderText, orderText.Length * Define.DoTextSpeed);

        EventHandler eventHandler = GetButton((int)Buttons.GetOrder).gameObject.GetOrAddComponent<EventHandler>();
        eventHandler.EventOnClick -= OnClickGetOrder;
        eventHandler.EventOnClick += OnClickGetOrder;
    }

    void OnClickGetOrder(PointerEventData evt)
    {
        if (GameManager.Instance.GameState != GameState.Idle) return;
        GameManager.Instance.GameState = GameState.SelectBase;
    }
}
