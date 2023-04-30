using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaugeWithText : MonoBehaviour {
    [SerializeField] private TMP_Text text;
    [SerializeField] private Slider slider;

    public void Up(){}

    private void Awake() {
        if (!text) text = GetComponentInChildren<TMP_Text>();
        if (!slider) slider = GetComponentInChildren<Slider>();
    }

    public void SetValue(int currentValue, int maxValue) {
        if (text) text.text = $"{currentValue}/{maxValue}";
        if (slider) slider.value = (float)currentValue / maxValue;
    }
}
