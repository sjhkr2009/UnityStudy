using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //액션을 사용하기 위해 불러와야 한다.

public class Worker : MonoBehaviour
{
    // 액션: C#에서 제공하는, 사용하기 쉽게 미리 만들어진 델리게이트.
    //      입출력이 없는 함수는 굉장히 많으므로, 이는 따로 델리게이트를 선언하지 않고 Action 으로 대체할 수 있다.


    //기존 방식
    //delegate void Work(); //입력과 출력(return)이 없는 함수를 대행할 work라는 델리게이트를 만들어서, 할 일을 이 목록에 쌓아놓을 것이다.
    //Work work; //델리게이트를 변수로 불러온다.

    //위 두 줄은 아래로 대체할 수 있다.
    Action work;

    void MoveBricks()
    {
        Debug.Log("벽돌을 옮겼다");
    }

    void DigIn()
    {
        Debug.Log("땅을 팠다");
    }

    void PhilosophyStudy()
    {
        Debug.Log("잠을 잤다");
    }

    private void Start()
    {
        work += MoveBricks;
        work += DigIn;
        work += PhilosophyStudy;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            work();
        }
    }
}
