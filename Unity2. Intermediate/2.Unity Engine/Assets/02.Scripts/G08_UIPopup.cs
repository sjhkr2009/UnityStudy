using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G08_UIPopup : G05_UIBase
{
    protected virtual void Init()
    {
        A01_Manager.UI.SetCanvasOrder(gameObject);
    }
}
