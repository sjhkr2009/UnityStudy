using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMaterialUI : UIBase_Scene
{
    enum Images
    {
        Tequilla,
        Vodka,
        Whisky,
        Gin,
        Rum,
        Brandy,
        //----------------//
        OrangeJuice,
        LimeJuice,
        LemonJuice,
        GrenadineSyrup,
        OrangeLiqueur,
        SodaWater,
        Amaretto,
        GingerBeer,
        CoffeeLiqueur,
        Vermouth,
        Mint,
        Sugar,
        Campari,
        CherryLiqueur,
        Cola
    }
    enum SubSelectedIcons
    {
        SelectedSub1,
        SelectedSub2,
        SelectedSub3
    }
    enum BaseSelectedIcons
    {
        SelectedBase1
    }
    enum Buttons
    {
        PrevButton,
        NextButton,
        ResetButton,
        OrderButton,
        Window1to2,
        Window2to3,
        Window2to1,
        Window3to2
    }
    enum WindowObjects
    {
        Window1,
        Window2,
        Window3
    }

    void Start() => Init();
    public override void Init()
    {
        Init(1);

        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(WindowObjects));
        Bind<Button>(typeof(Buttons));
        Bind<BaseSelectedIcon>(typeof(BaseSelectedIcons));
        Bind<SubSelectedIcon>(typeof(SubSelectedIcons));

        Inited = true;
    }
    private void OnEnable()
    {
        if (!Inited)
            return;
    }
}
