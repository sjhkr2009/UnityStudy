using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class UIBase_Popup : UIBase
{
    public bool DontDestroy { get; private set; }
    public bool Inited { get; private set; } = false;
    public virtual void Init()
    {
        GameManager.UI.SetCanvasOrder(gameObject);
    }
    protected void SetPooling()
    {
        DontDestroy = true;
        Inited = true;
    }
}
