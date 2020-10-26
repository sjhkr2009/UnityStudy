using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase_Popup : UIBase
{
    public bool DontDestroy { get; private set; }
    public bool Inited { get; private set; } = false;
    public virtual void Init()
    {
        GameManager.UI.SetCanvasOrder(gameObject);
    }

    /// <summary>
    /// 이 UI 팝업창을 풀링 처리합니다. 창을 닫을 때 파괴되는 대신 비활성화 처리됩니다.
    /// OnEnable/OnDisable 함수에서 활성화 및 비활성화 처리 때 동작을 따로 서술해야 하며, 활성화 시 레이어 조정을 위해 OnEnable에서 gameObject.SetCanvasOrder() 함수가 요구됩니다.
    /// </summary>
    protected void SetPooling()
    {
        DontDestroy = true;
        Inited = true;
    }
}
