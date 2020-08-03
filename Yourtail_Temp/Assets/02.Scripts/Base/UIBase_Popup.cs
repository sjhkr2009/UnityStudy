using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase_Popup : UIBase
{
    public virtual void Init()
    {
        GameManager.UI.SetCanvasOrder(gameObject);
    }
}
