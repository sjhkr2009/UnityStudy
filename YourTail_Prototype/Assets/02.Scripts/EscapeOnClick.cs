using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EscapeOnClick : MonoBehaviour, IPointerClickHandler
{
    public bool isUI = true;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isUI) GameManager.Input.InEscape();
    }

    void OnMouseUpAsButton()
    {
        if(!isUI) GameManager.Input.InEscape();
    }
}
