using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningText : MonoBehaviour
{
    public event Action<Text> EventOnTextEnable = (T) => { };

    [SerializeField] Text warningText;
    void Awake()
    {
        if (warningText == null) warningText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        EventOnTextEnable(warningText);
    }
}
