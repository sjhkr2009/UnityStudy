using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSubtractor2 : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameManager2.GetInstance().AddScore(-2); //instance2 는 public이 아니므로, 함수를 통해 호출
            Debug.Log(GameManager2.GetInstance().GetScore());
        }
    }
}
