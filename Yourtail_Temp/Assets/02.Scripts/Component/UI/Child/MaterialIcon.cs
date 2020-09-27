using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

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
    private bool _isActive = true;
    [ShowInInspector] public bool IsActive
	{
        get => _isActive;
		set
		{
            _isActive = value;

            float alpha = value ? 1f : 0.5f;
            _myImage.color = new Color(alpha, alpha, alpha, alpha);
        }
	}

	private void Start()
	{
        GameManager.Data.OnValidUpdate -= SetValid;
        GameManager.Data.OnValidUpdate += SetValid;
    }

	public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsActive)
            return;
        
        GameManager.Input.InMaterialSelect(_myMaterial.Id);
    }

    void SetValid()
	{
        if (_myMaterial.materialType == CocktailMaterials.MaterialType.Base)
            return;

        if (GameManager.Data.CurrentSubMaterials.Count == 0 && GameManager.Data.CurrentBaseMaterials.Count == 0)
		{
            IsActive = true;
            return;
		}

        IsActive = GameManager.Data.ValidMaterials.Contains(_myMaterial.Id);
    }
}
