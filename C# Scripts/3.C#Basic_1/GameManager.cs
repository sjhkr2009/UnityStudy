using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글톤 활용
    //점수 관리 스크립트

    public static GameManager instance; //이 스크립트에 static 변수를 만든다.

    int score = 0;

    void Awake()
    {
        instance = this; //게임 시작 시 이 스크립트를 가진 오브젝트 자신을 정적 변수에 대입.
        //이제 다른 곳에서 이 오브젝트를 가져오지 않고 바로 사용할 수 있다.
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int newScore)
    {
        score += newScore;
    }
}
