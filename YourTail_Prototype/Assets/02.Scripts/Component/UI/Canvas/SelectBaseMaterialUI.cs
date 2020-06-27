using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBaseMaterialUI : UIBase_Popup
{
    enum MaterialImages
    {
        Rum,
        Brandy
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
        NextButton
    }
    
    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(MaterialImages));
        Bind<GameObject>(typeof(SelectedUIObjects));
        Bind<Button>(typeof(Buttons));

        SetBaseImage();

        SetSelectedUI();

        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.SelectSub; });
    }
    private void OnDestroy()
    {
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
    }
    private void Start()
    {
        Init();
    }
    void SetBaseImage()
    {
        for (int i = 0; i < GameManager.Data.BaseMaterialList.Count; i++)
            SetIcon(i);
    }

    void SetIcon(int index)
    {
        Image _image = GetImage(index);
        MaterialIcon icon = _image.gameObject.GetOrAddComponent<MaterialIcon>();
        icon.myMaterial = GameManager.Data.BaseMaterialList[index];

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
