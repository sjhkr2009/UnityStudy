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

            ColorFiltering(GameManager.Instance.IsDebuggingMode);
        }
	}

	private void Start()
	{
        GameManager.Game.OnValidUpdate -= SetValid;
        GameManager.Game.OnValidUpdate += SetValid;

        GameManager.Instance.EventOnDebugModeChange -= ColorFiltering;
        GameManager.Instance.EventOnDebugModeChange += ColorFiltering;
    }

    void ColorFiltering(bool isDebugMode)
    {
        float alpha = (!isDebugMode || IsActive) ? 1f : 0.5f;
        _myImage.color = new Color(alpha, alpha, alpha, alpha);
    }
	public void OnPointerClick(PointerEventData eventData)
    {
        // Temp: 비활성화된 재료도 선택 가능하게 변경
        // if (!IsActive) return;
        
        GameManager.Input.InMaterialSelect(_myMaterial.Id);
    }

    void SetValid()
	{
        // Temp: 베이스 재료도 필터링 대상에 포함
        // if (_myMaterial.materialType == CocktailMaterials.MaterialType.Base) return;

        if (GameManager.Game.CurrentSubMaterials.Count == 0 && GameManager.Game.CurrentBaseMaterials.Count == 0)
		{
            IsActive = true;
            return;
		}

        IsActive = GameManager.Game.ValidMaterials.Contains(_myMaterial.Id);
    }
}
