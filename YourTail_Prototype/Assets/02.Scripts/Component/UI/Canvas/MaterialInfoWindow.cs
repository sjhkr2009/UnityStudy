﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialInfoWindow : UIBase_Popup
{
    CocktailMaterials myMaterial;

    enum Texts
    {
        Name,
        Info
    }
    enum Images
    {
        MaterialImage,
        IconBackground
    }
    enum Transforms
    {
        IconBackground
    }
    enum Buttons
    {
        CloseButton
    }

    private void Start() => Init();
    public override void Init()
    {
        base.Init();
        myMaterial = GameManager.Data.CurrentMaterialInfo;

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<RectTransform>(typeof(Transforms));
        Bind<Button>(typeof(Buttons));

        SetInfo();

        GetButton((int)Buttons.CloseButton).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<MaterialInfoWindow>(); });
    }

    void SetInfo()
    {
        GetText((int)Texts.Name).text = myMaterial.Name;
        GetText((int)Texts.Info).text = myMaterial.Info;

        GetImage((int)Images.MaterialImage).sprite = myMaterial.image;


        if (myMaterial.materialType == CocktailMaterials.MaterialType.Base)
        {
            GetImage((int)Images.IconBackground).enabled = false;
            Get<RectTransform>((int)Transforms.IconBackground).sizeDelta = Define.baseMaterialSize;
        }
        else
        {
            GetImage((int)Images.IconBackground).enabled = true;
        }
    }

    private void OnDestroy()
    {
        GetButton((int)Buttons.CloseButton).onClick.RemoveAllListeners();
    }
}
