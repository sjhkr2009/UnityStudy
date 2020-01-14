using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Test : MonoBehaviour
{
    enum TestEnum { A, B, C, D }

    public int number;

    [Button]
    public void TestFunction()
    {
        Debug.Log(TestEnum.A);
        Debug.Log((int)TestEnum.B);
        N10(number = 10);
    }

    void N10(int n)
    {
        Debug.Log(n + 10);
    }
}
