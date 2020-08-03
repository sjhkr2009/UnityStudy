using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBeforeShake : UIBase_Popup
{
    enum Objects
    {
        SpiritErrorText,
        SubErrorText,
        SpiritInfoIcon,
        SubInfoIcon1,
        SubInfoIcon2,
        SubInfoIcon3,
        Count
    }
    
    enum Texts
    {
        SpiritName,
        SubName1,
        SubName2,
        SubName3,
        CocktailName
    }

    enum Buttons
    {
        DoButton,
        CancelButton
    }

    enum Images
    {
        SpiritImage,
        SubImage1,
        SubImage2,
        SubImage3
    }

    enum InfoIcons
    {
        SpiritInfoIcon,
        SubInfoIcon1,
        SubInfoIcon2,
        SubInfoIcon3
    }
    
    void Start() => Init();

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(Objects));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<MaterialInfoIcon>(typeof(InfoIcons));

        AllSetActiveFalse();

        SetSpirit();
        SetSubMaterial(0, Images.SubImage1, Texts.SubName1, InfoIcons.SubInfoIcon1, Objects.SubInfoIcon1);
        SetSubMaterial(1, Images.SubImage2, Texts.SubName2, InfoIcons.SubInfoIcon2, Objects.SubInfoIcon2);
        SetSubMaterial(2, Images.SubImage3, Texts.SubName3, InfoIcons.SubInfoIcon3, Objects.SubInfoIcon3);

        GetButton((int)Buttons.CancelButton).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<CheckBeforeShake>(); });
        GetButton((int)Buttons.DoButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.Combine; });

        Cocktail temp = GameManager.Data.MakeCocktail();
        GetText((int)Texts.CocktailName).text = $"예상: {temp.Name_kr}";
    }
    void AllSetActiveFalse()
    {
        for (int i = 0; i < (int)Objects.Count; i++)
        {
            Get<GameObject>(i).SetActive(false);
        }
    }

    void SetSubMaterial(int index, Images image, Texts name, InfoIcons icon, Objects iconObject)
    {
        SubMaterials myMaterial = GameManager.Data.CurrentSubMaterials.Count > index ? GameManager.Data.CurrentSubMaterials[index] : null;

        if (myMaterial == null)
        {
            if (index == 0)
            {
                Get<GameObject>((int)Objects.SubErrorText).SetActive(true);
                GetButton((int)Buttons.DoButton).interactable = false;
            }
            return;
        }

        GetImage((int)image).sprite = myMaterial.image;
        GetText((int)name).text = myMaterial.Name;
        Get<GameObject>((int)iconObject).SetActive(true);
        Get<MaterialInfoIcon>((int)icon).myMaterial = myMaterial;
    }
    void SetSpirit(int index = 0, Images image = Images.SpiritImage, Texts name = Texts.SpiritName, InfoIcons icon = InfoIcons.SpiritInfoIcon, Objects iconObject = Objects.SpiritInfoIcon)
    {
        BaseMaterials myMaterial = GameManager.Data.CurrentBaseMaterials.Count > index ? GameManager.Data.CurrentBaseMaterials[index] : null;

        if (myMaterial == null)
        {
            if (index == 0)
            {
                Get<GameObject>((int)Objects.SpiritErrorText).SetActive(true);
                GetButton((int)Buttons.DoButton).interactable = false;
            }
            return;
        }

        GetImage((int)image).sprite = myMaterial.image;
        GetText((int)name).text = myMaterial.Name;
        Get<GameObject>((int)iconObject).SetActive(true);
        Get<MaterialInfoIcon>((int)icon).myMaterial = myMaterial;
    }
}
