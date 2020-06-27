using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSelectedIcon : MonoBehaviour
{
    BaseMaterials selectedMaterial = null;
    Image myImage;
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

        GameManager.Data.OnAddBaseMaterial -= SetIcon;
        GameManager.Data.OnRemoveBaseMaterial -= DeleteIcon;

        GameManager.Data.OnAddBaseMaterial += SetIcon;
        GameManager.Data.OnRemoveBaseMaterial += DeleteIcon;

        SetIcon();
    }

    void OnDestroy()
    {
        GameManager.Data.OnAddBaseMaterial -= SetIcon;
        GameManager.Data.OnRemoveBaseMaterial -= DeleteIcon;
    }

    void SetIcon(BaseMaterials selected)
    {
        if (GameManager.Data.CurrentBaseMaterials.Count != myCount) return;

        selectedMaterial = selected;
        myImage.sprite = selectedMaterial.image;
    }
    public void SetIcon()
    {
        if (GameManager.Data.CurrentBaseMaterials.Count < myCount)
        {
            myImage.sprite = null;
            return;
        }
        
        selectedMaterial = GameManager.Data.CurrentBaseMaterials[MyIndex];
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
}
