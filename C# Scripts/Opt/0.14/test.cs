using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            if(i == 2)
            {
                continue;
            }
            if(i == 4)
            {
                break;
            }
            if(i == 6)
            {
                return;
            }
            Debug.Log(i);
        }
        Debug.Log("끝");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
