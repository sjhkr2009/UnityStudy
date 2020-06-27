using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    Vector3 originPos;
    
    bool _hasCustomer = false;
    public bool HasCustomer
    {
        get => _hasCustomer;
        set
        {
            _hasCustomer = value;
            if (!value)
            {
                customerImage.sprite = null;
                currentCustomer = null;
                gameObject.SetActive(false);
            }
        }
    }
    [ReadOnly] public Customers currentCustomer;
    [ReadOnly] public SpriteRenderer customerImage;
    public Action<Customers> EventOnSelectCustomer = c => { };
    
    void Start()
    {
        customerImage = gameObject.GetOrAddComponent<SpriteRenderer>();
        originPos = transform.position;
        customerImage.sortingLayerName = "Customer";
        DeleteCustomer();
    }

    public void SetCustomer(Customers customer)
    {
        if (HasCustomer) return;

        HasCustomer = true;
        currentCustomer = customer;
        customerImage.sprite = customer.image;
    }
    public void DeleteCustomer() { HasCustomer = false; }

    private void OnMouseUpAsButton()
    {
        EventOnSelectCustomer(currentCustomer);
        Debug.Log(currentCustomer.GetOrder().orderContents);
    }
    private void OnDisable()
    {
        transform.position = originPos;
    }
}
