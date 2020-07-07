using System.Collections;
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
        MaterialImage
    }

    private void Start() => Init();
    public override void Init()
    {
        base.Init();
        myMaterial = GameManager.Data.CurrentMaterialInfo;

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        SetInfo();
    }

    void SetInfo()
    {
        GetText((int)Texts.Name).text = myMaterial.Name;
        GetText((int)Texts.Info).text = myMaterial.Info;

        GetImage((int)Images.MaterialImage).sprite = myMaterial.image;
    }
}
