using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintInputField : MonoBehaviour
{
    public void OnInpurFieldTextChanged(string newText)
    {
        Debug.Log("타이핑하는 중... ( " + newText + " )");
    }

    public void OnInputFieldTextDone(string newText)
    {
        Debug.Log("타이핑 완료: " + newText);
    }
}
