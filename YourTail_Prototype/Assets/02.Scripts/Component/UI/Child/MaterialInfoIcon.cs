using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialInfoIcon : MonoBehaviour, IPointerClickHandler
{
    public MaterialIcon parent;

    private void Start()
    {
        if (parent == null) parent = transform.GetComponentInParent<MaterialIcon>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Input.InMaterialInfo(parent.myMaterial);
        GameManager.UI.OpenPopupUI<MaterialInfoWindow>();
    }
}
