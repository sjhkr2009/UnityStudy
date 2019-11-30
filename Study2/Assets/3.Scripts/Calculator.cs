using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    
    void Start()
    {
        MultiplicationTable();
    }
    void Update()
    {
        
    }
    void MultiplicationTable()
    {
        for (int i = 2; i <= 9; i++)
        {
            Debug.Log($"- {i}단 -");
            for (int j = 1; j <= 9; j++)
            {
                Debug.Log($"{i} x {j} = {i * j}");
            }
        }
    }
}
