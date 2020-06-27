using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSubMaterialUI : UIBase_Popup
{
    enum MaterialImages
    {
        Curacao,
        Pineapple,
        Lime,
        Lemon
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
        NextButton
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(MaterialImages));
        Bind<GameObject>(typeof(SelectedUIObjects));
        Bind<Button>(typeof(Buttons));

        SetSubImage();

        SetSelectedUI();

        GetButton((int)Buttons.PrevButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.SelectBase; });
        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.Combine; });
    }
    private void OnDestroy()
    {
        GetButton((int)Buttons.PrevButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
    }
    private void Start()
    {
        Init();
    }
    void SetSubImage()
    {
        for (int i = 0; i < GameManager.Data.SubMaterialList.Count; i++)
            SetIcon(i);
    }

    void SetIcon(int index)
    {
        Image _image = GetImage(index);
        MaterialIcon icon = _image.gameObject.GetOrAddComponent<MaterialIcon>();
        icon.myMaterial = GameManager.Data.SubMaterialList[index];

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
