using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialInfoIcon : MonoBehaviour, IPointerClickHandler
{
    public MaterialIcon parent;
    public CocktailMaterials myMaterial;

    private void Start()
    {
        parent = transform.GetComponentInParent<MaterialIcon>();
        if (parent != null) myMaterial = transform.GetComponentInParent<MaterialIcon>().myMaterial;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (myMaterial == null) return;

        GameManager.Input.InMaterialInfo(myMaterial);
        GameManager.UI.OpenPopupUI<MaterialInfoWindow>();
    }
}
