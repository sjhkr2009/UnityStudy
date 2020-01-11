using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    TestObject testObject;
    void Start()
    {
        testObject = Resources.Load<TestObject>("TestObject");
    }

    void Update()
    {
        
    }
}
