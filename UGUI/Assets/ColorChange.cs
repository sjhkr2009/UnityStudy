using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour
{
    public Slider slider;
    int colorSelect;
    bool isColorOn;
    float colorCon;
    public void ChangeColor(bool isOn)
    {
        if (isOn)
        {
            GetComponent<Renderer>().material.color = MakeColor(colorSelect);
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
        isColorOn = isOn;
    }

    public void ColorSet(int index)
    {
        colorSelect = index;
        ChangeColor(isColorOn);
    }

    public void ColorConcentration(float scroll)
    {
        colorCon = scroll;
        ChangeColor(isColorOn);
    }
    
    Color MakeColor(int index)
    {
        Color color;

        switch (index)
        {
            case 0:
                color = new Color(colorCon, 0, 0);
                break;
            case 1:
                color = new Color(0, 0, colorCon);
                break;
            case 2:
                color = new Color(0, colorCon, 0);
                break;
            case 3:
                color = new Color(colorCon, colorCon, colorCon);
                break;
            default:
                color = Color.white;
                break;
        }

        return color;
    }

    private void Awake()
    {
        colorSelect = 0;
    }
}
