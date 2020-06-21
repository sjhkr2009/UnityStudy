using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBaseMaterialUI : UIBase_Popup
{
    List<BaseMaterials> _list = null;
    enum Images
    {
        Rum,
        Brandy
    }
    
    public override void Init()
    {
        base.Init();
        if(_list == null || _list.Count == 0) _list = GameManager.Data.BaseMaterialList;

        Bind<Image>(typeof(Images));

        SetBaseImage();
    }
    private void Start()
    {
        Init();
    }
    void SetBaseImage()
    {
        for (int i = 0; i < _list.Count; i++) SetImage(i);
    }

    void SetImage(int imageIndex)
    {
        Image _image = GetImage(imageIndex);
        _image.sprite = _list[imageIndex].image;
        _image.gameObject.GetOrAddComponent<SelectMaterialOnClick>().index = imageIndex + 1;
    }
}
