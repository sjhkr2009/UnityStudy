using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaterialIcon : MonoBehaviour, IPointerClickHandler
{
    public CocktailMaterials myMaterial = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Input.InMaterialSelect(myMaterial.Id);
    }
}
