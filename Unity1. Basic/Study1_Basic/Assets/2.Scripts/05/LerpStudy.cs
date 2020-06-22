using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpStudy : MonoBehaviour
{
    Vector3 target = new Vector3(0f, 2f, 0f);
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 lerpVector = Vector3.Lerp(Vector3.zero, target, 0.2f);
        Debug.Log(lerpVector);
        
    }
}
