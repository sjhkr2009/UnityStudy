﻿using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    [SerializeField] Vector3 originPos;
    
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

    private void Awake()
    {
        originPos = transform.position;
    }
    void Start()
    {
        customerImage = gameObject.GetOrAddComponent<SpriteRenderer>();
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
    public void SetLayer(bool isSelected)
    {
        if(isSelected)
            customerImage.sortingLayerName = "SelectedCustomer";
        else
            customerImage.sortingLayerName = "Customer";
    }
    private void OnDisable()
    {
        transform.position = originPos;
        customerImage.sortingLayerName = "Customer";
    }
}
