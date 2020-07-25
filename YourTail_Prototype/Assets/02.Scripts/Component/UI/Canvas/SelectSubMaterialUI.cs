using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSubMaterialUI : UIBase_Popup
{
    enum MaterialImages
    {
        OrangeJuice,
        LimeJuice,
        LemonJuice,
        GrenadineSyrup,
        TonicWater,
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

    enum SelectedUIObjects
    {
        SelectedBase1,
        SelectedSub1,
        SelectedSub2,
        SelectedSub3
    }

    enum Buttons
    {
        PrevButton,
        NextButton,
        Window1to2,
        Window2to3,
        Window2to1,
        Window3to2
    }

    enum Windows
    {
        Window1,
        Window2,
        Window3
    }
    enum Texts
    {
        OrderText
    }

    List<GameObject> windows = new List<GameObject>();

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(MaterialImages));
        Bind<GameObject>(typeof(SelectedUIObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Transform>(typeof(Windows));
        Bind<Text>(typeof(Texts));

        SetSubImage();
        SetSelectedUI();
        SetButtons();
        SetWindows();

        GetText((int)Texts.OrderText).text = GameManager.Data.CurrentOrder.orderContents;
    }
    private void OnDestroy() => ResetButtons();
    private void Start() => Init();

    void SetWindows()
    {
        windows.Add(Get<Transform>((int)Windows.Window1).gameObject);
        windows.Add(Get<Transform>((int)Windows.Window2).gameObject);
        windows.Add(Get<Transform>((int)Windows.Window3).gameObject);

        foreach (GameObject window in windows) window.SetActive(false);

        windows.OpenWindow(0);
    }
    void SetButtons()
    {
        GetButton((int)Buttons.PrevButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.SelectBase; });
        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.Combine; });

        GetButton((int)Buttons.Window1to2).onClick.AddListener(() => { windows.OpenWindow(1); });
        GetButton((int)Buttons.Window2to3).onClick.AddListener(() => { windows.OpenWindow(2); });
        GetButton((int)Buttons.Window2to1).onClick.AddListener(() => { windows.OpenWindow(0); });
        GetButton((int)Buttons.Window3to2).onClick.AddListener(() => { windows.OpenWindow(1); });
    }
    void ResetButtons()
    {
        GetButton((int)Buttons.PrevButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Window1to2).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Window2to1).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Window2to3).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Window3to2).onClick.RemoveAllListeners();
    }
    void SetSubImage()
    {
        for (int i = 0; i < GameManager.Data.SubMaterialIndexData.Count; i++)
            SetIcon(i);
    }

    void SetIcon(int index)
    {
        Image _image = GetImage(index);
        MaterialIcon icon = _image.gameObject.GetOrAddComponent<MaterialIcon>();
        icon.myMaterial = GameManager.Data.SubMaterialIndexData[index + 1];

        _image.sprite = icon.myMaterial.image;
    }
    void SetSelectedUI()
    {
        BaseSelectedIcon base1 = Get<GameObject>((int)SelectedUIObjects.SelectedBase1).GetOrAddComponent<BaseSelectedIcon>();
        base1.MyCount = 1;

        SubSelectedIcon sub1 = Get<GameObject>((int)SelectedUIObjects.SelectedSub1).GetOrAddComponent<SubSelectedIcon>();
        sub1.MyCount = 1;

        SubSelectedIcon sub2 = Get<GameObject>((int)SelectedUIObjects.SelectedSub2).GetOrAddComponent<SubSelectedIcon>();
        sub2.MyCount = 2;

        SubSelectedIcon sub3 = Get<GameObject>((int)SelectedUIObjects.SelectedSub3).GetOrAddComponent<SubSelectedIcon>();
        sub3.MyCount = 3;
    }
}
