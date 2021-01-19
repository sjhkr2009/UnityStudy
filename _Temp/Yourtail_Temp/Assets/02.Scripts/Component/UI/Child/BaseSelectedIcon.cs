using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class BaseSelectedIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, ReadOnly] BaseMaterials selectedMaterial = null;
    [SerializeField] Image myImage;
    [SerializeField] int myCount = 1;
    public int MyCount
    {
        get => myCount;
        set
        {
            myCount = value;
            if (myImage != null) SetIcon();
        }
    }
    int MyIndex => myCount - 1;
    
    void Start()
    {
        myImage = gameObject.GetOrAddComponent<Image>();

        GameManager.Game.OnAddBaseMaterial -= SetIcon;
        GameManager.Game.OnRemoveBaseMaterial -= DeleteIcon;

        GameManager.Game.OnAddBaseMaterial += SetIcon;
        GameManager.Game.OnRemoveBaseMaterial += DeleteIcon;

        SetIcon();
    }

    void SetIcon(BaseMaterials selected)
    {
        if (GameManager.Game.CurrentBaseMaterials.Count != myCount) return;

        selectedMaterial = selected;
        myImage.sprite = selectedMaterial.image;
    }
    public void SetIcon()
    {
        if (GameManager.Game.CurrentBaseMaterials.Count < myCount)
        {
            myImage.sprite = GameManager.UI.NullImage;
            return;
        }
        
        selectedMaterial = GameManager.Game.CurrentBaseMaterials[MyIndex];
        if (selectedMaterial != null)
            myImage.sprite = selectedMaterial.image;
    }
    void DeleteIcon(BaseMaterials selected)
    {
        //if (selected != selectedMaterial) return;

        //selectedMaterial = null;
        //myImage.sprite = null;
        SetIcon();
    }
    public void ResetIcon()
	{
        if(myImage == null)
            myImage = gameObject.GetOrAddComponent<Image>();

        selectedMaterial = null;
        myImage.sprite = GameManager.UI.NullImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(selectedMaterial != null)
        {
            GameManager.Input.InMaterialSelect(selectedMaterial.Id);
        }
    }
}
