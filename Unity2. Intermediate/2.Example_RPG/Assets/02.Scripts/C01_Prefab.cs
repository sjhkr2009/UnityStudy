using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C01_Prefab : MonoBehaviour
{
    public GameObject prefab;

    GameObject unitychan;
    private void Start()
    {
        unitychan = Instantiate(prefab);

        Destroy(unitychan, 3f);
    }
}
