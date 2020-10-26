using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : UIBase_Scene
{

    enum Texts
    {
        CoinText
    }
    void Start() => Init();
    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));

        SetCoin(GameManager.Data.BirdCoin);

        GameManager.Data.OnSetCoin -= SetCoin;
        GameManager.Data.OnSetCoin += SetCoin;
    }
    private void OnDestroy()
    {
        GameManager.Data.OnSetCoin -= SetCoin;
    }

    void SetCoin(int value)
    {
        GetText((int)Texts.CoinText).text = value.ToString();
    }
}
