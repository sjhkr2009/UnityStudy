using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloArray : MonoBehaviour
{
    
    void Start()
    {

        //배열

        int score1 = 75;
        int score2 = 90;
        int score3 = 50;
        int score4 = 65;
        int score5 = 45;

        Debug.Log(score1);
        Debug.Log(score2);
        Debug.Log(score3);
        Debug.Log(score4);
        Debug.Log(score5);

        //배열: 여러개의 값을 하나의 변수로 다루게 해 준다.
        int[] scores = new int[10]; //scores 변수에 10개의 공간을 할당. 각각의 이름은 score[0] ~ score[9]
        scores[0] = 75;
        scores[1] = 90;
        scores[2] = 50;
        scores[3] = 65;
        scores[4] = 45;
        scores[5] = 30;
        scores[6] = 50;
        scores[7] = 55;
        scores[8] = 95;
        scores[9] = 70;
        //scores[10] = 80; //이렇게 공간을 초과할 경우 out of index 오류.

        //배열 이후 for문 활용
        for(int i = 0; i < 10; i++)
        {
            Debug.Log("학생" + (i+1) + "번째의 점수: " + scores[i]);
        }

        scores = new int[20]; //새로 공간을 할당하면 기존의 값, 즉 score[0]~[9]에 입력한 값들은 모두 사라진다.



    }
}
