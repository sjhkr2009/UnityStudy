using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAdder2 : MonoBehaviour
{

    void Awake()
    {
        Debug.Log("Start Score: " + GameManager2.GetInstance().GetScore()); //게임 시작 시 GameManager2의 함수가 한 번 실행되게 한다. (중복 혹은 null 체크를 위함)
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager2.GetInstance().AddScore(5); //instance2 는 public이 아니므로, 함수를 통해 호출
            Debug.Log(GameManager2.GetInstance().GetScore());
        }
    }
}
