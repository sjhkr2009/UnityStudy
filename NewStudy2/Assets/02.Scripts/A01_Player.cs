using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A01_Player : MonoBehaviour
{
    // 싱글톤으로 만든 매니저를 가져온다.
    
    void Start()
    {
        A01_Manager manager = A01_Manager.Instance;
    }

    void Update()
    {
        
    }
}
