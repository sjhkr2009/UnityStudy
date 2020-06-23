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
    
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));

        string orderText = GameManager.Data.CurrentOrder.orderContents;
        GetText((int)Texts.OrderText).DOText(orderText, orderText.Length * 0.025f);
    }
}
