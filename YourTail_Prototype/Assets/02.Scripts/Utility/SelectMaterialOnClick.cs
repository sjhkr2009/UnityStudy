using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectMaterialOnClick : MonoBehaviour, IPointerClickHandler
{
    public int index = 1;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Input.InMaterialSelect(index);
    }
}
