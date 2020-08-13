using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(gameObject.GetComponent<BoxCollider>());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(gameObject.AddComponent<BoxCollider>());
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Component component1 = GetComponent<BoxCollider>();
            Destroy(component1);
            Debug.Log(gameObject.GetComponent<BoxCollider>());
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Component component1 = GetComponent<BoxCollider>();
            DestroyImmediate(component1);
            Debug.Log(gameObject.GetComponent<BoxCollider>());
        }

    }
}
