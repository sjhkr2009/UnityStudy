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
        MaterialImage,
        IconBackground
    }
    enum Transforms
    {
        IconBackground
    }

    private void Start() => Init();
    public override void Init()
    {
        base.Init();
        myMaterial = GameManager.Data.CurrentMaterialInfo;

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<RectTransform>(typeof(Transforms));

        SetInfo();
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
}
