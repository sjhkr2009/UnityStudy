using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TweenOnClick : TweenBase, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        DoChange();
    }
}
