using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase_Scene : UIBase
{
    public virtual void Init()
    {
        GameManager.UI.SetCanvasOrder(gameObject, false);
    }
}
