using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubSelectedIcon : MonoBehaviour, IPointerClickHandler
{
    // BaseSelectedIcon이랑 하나로 합치고싶긴 한데 마감이 급해서 복붙한 스끄립-뜨

    [SerializeField, ReadOnly] SubMaterials selectedMaterial = null;
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

        GameManager.Data.OnAddSubMaterial -= SetIcon;
        GameManager.Data.OnRemoveSubMaterial -= DeleteIcon;

        GameManager.Data.OnAddSubMaterial += SetIcon;
        GameManager.Data.OnRemoveSubMaterial += DeleteIcon;

        SetIcon();
    }

    void SetIcon(SubMaterials selected)
    {
        if (GameManager.Data.CurrentSubMaterials.Count != myCount) return;

        selectedMaterial = selected;
        myImage.sprite = selectedMaterial.image;
    }
    public void SetIcon()
    {
        if (GameManager.Data.CurrentSubMaterials.Count < myCount)
        {
            myImage.sprite = GameManager.UI.NullImage;
            return;
        }

        selectedMaterial = GameManager.Data.CurrentSubMaterials[MyIndex];
        if (selectedMaterial != null)
            myImage.sprite = selectedMaterial.image;
    }
    void DeleteIcon(SubMaterials selected)
    {
        //if (selected == selectedMaterial)
        //{
        //    selectedMaterial = null;
        //    myImage.sprite = null;
        //}
        SetIcon();
    }
    public void ResetIcon()
    {
        if (myImage == null)
            myImage = gameObject.GetOrAddComponent<Image>();

        selectedMaterial = null;
        myImage.sprite = GameManager.UI.NullImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedMaterial != null)
        {
            GameManager.Input.InMaterialSelect(selectedMaterial.Id);
        }
    }
}
