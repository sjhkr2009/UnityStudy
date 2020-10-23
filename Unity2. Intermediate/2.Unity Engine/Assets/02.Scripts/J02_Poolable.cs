using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J02_Poolable : MonoBehaviour
{
    public bool isUsing;

    private void OnEnable()
    {
        isUsing = true;
    }

    private void OnDisable()
    {
        isUsing = false;
    }
}
