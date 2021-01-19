using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase_Scene : UIBase
{
    public virtual void Init()
    {
        GameManager.UI.SetCanvasOrder(gameObject, false);
    }
    protected void Init(int orderLayer)
    {
        GameManager.UI.SetCanvasOrder(gameObject, false, orderLayer);
    }

    public bool Inited { get; protected set; } = false;
}
