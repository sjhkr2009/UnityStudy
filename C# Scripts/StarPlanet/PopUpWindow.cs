using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWindow : MonoBehaviour
{
    public event Action EventOnPopUpOpen = () => { };

    private void OnEnable()
    {
        EventOnPopUpOpen();
    }
}
