using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TweenOnPointerEnter : TweenBase, IPointerEnterHandler,IPointerExitHandler
{
    bool isChanging = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        DoChange();
        isChanging = true;
        DOVirtual.DelayedCall(Delay, () => { isChanging = false; });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isChanging)
        {
            image.DOKill();
            transform.DOKill();
        }
        DoOrigin();
    }
    void Start()
    {
        SetOrigin();
    }
}
