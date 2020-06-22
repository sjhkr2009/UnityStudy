using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplicationTable : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MultiplyInTable(2, 9, 9);
        }
    }
    
    void MultiplyInTable(int startNumber, int endNumber, int count)
    {
        int i = startNumber;
        while(i <= endNumber)
        {
            Debug.Log($"- {i}단 -");
            int j = 1;
            while(j <= count)
            {
                Debug.Log($"{i} x {j} = {i * j}");
                j++;
            }
            Debug.Log("\n");
            i++;
        }
    }
}
