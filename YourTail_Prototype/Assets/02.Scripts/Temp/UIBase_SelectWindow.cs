using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase_SelectWindow : UIBase_Popup
{
    [SerializeField] Transform _contents;
    Transform Contents
    {
        get
        {
            if (_contents == null)
                _contents = transform.Find("Contents");
            return _contents;
        }
    }
    protected Image GetIconImage(int index)
    {
        GameObject go = null;
        if (index < Contents.childCount) go = Contents.GetChild(index).gameObject;
        else go = GameManager.Resource.Instantiate("MaterialIcon", Contents);

        Image image = go.transform.GetChild(0).GetOrAddComponent<Image>();

        return image;
    }
}
