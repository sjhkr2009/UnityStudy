using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugModeTest : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] Image bg;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        bg = GetComponentInChildren<Image>();

        toggle.onValueChanged.AddListener(isOn => 
        { 
            GameManager.Instance.ChangeDebugMode(isOn);
            float alpha = isOn ? 0.85f : 0.3f;
            bg.color = new Color(1f, 1f, 1f, alpha);
        });

        toggle.isOn = true;
        toggle.isOn = false;
    }
}
