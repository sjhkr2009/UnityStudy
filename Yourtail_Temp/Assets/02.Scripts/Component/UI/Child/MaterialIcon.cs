using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaterialIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CocktailMaterials _myMaterial = null;
    [SerializeField] private Image _myImage = null;
    public CocktailMaterials MyMaterial
    {
        get => _myMaterial;
        set
        {
            _myMaterial = value;
            _myImage = gameObject.GetComponent<Image>();

            if(_myImage == null)
            {
                _myImage = gameObject.AddComponent<Image>();
                _myImage.sprite = _myMaterial.image;
                _myImage.SetNativeSize();
            }
            else
            {
                _myImage.sprite = _myMaterial.image;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Input.InMaterialSelect(_myMaterial.Id);
    }
}
