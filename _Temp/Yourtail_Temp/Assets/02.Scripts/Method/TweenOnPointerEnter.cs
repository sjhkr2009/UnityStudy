using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TweenOnPointerEnter : TweenBase, IPointerEnterHandler,IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        DoChange();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.DOKill();
        transform.DOKill();
        DoOrigin();
    }
    void Start()
    {
        SetOrigin();
    }
}
