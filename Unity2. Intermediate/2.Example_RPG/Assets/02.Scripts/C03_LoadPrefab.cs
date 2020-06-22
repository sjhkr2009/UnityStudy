using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C03_LoadPrefab : MonoBehaviour
{
    void Start()
    {
        GameObject gameObject = A01_Manager.Resource.Instantiate("unitychan");

        A01_Manager.Resource.Destroy(gameObject, 3f);
    }
}
