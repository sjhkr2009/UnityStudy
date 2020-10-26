using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomerTable : MonoBehaviour, IPointerClickHandler
{
    [ShowInInspector, ReadOnly] public Customers currentCustomer;
    [ShowInInspector, ReadOnly] public int myIndex;
    [SerializeField] Image myImage;
    public Action<Customers> EventOnSelectCustomer = c => { };

    public bool IsEmpty { get; private set; } = true;

    void Start()
    {
        if (myImage == null)
            myImage = gameObject.GetOrAddComponent<Image>();

        currentCustomer = null;
        myImage.sprite = GameManager.UI.NullImage;
    }

	public void OnPointerClick(PointerEventData eventData)
	{
        if (IsEmpty || currentCustomer == null)
            return;

        GameManager.Data.CurrentTableIndex = myIndex;
        EventOnSelectCustomer(currentCustomer);
    }

    public void SetCustomer(Customers bird)
	{
        gameObject.SetActive(true);
        currentCustomer = bird;
        myImage.sprite = bird.Image;
        IsEmpty = false;
	}
    
    public void DeleteCustomer()
	{
        myImage.sprite = GameManager.UI.NullImage;
        currentCustomer = null;
        IsEmpty = true;
        gameObject.SetActive(false);

	}
}
