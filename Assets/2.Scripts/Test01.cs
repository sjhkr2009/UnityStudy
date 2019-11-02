using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test01 : MonoBehaviour
{
    //변수: 값을 담는 그릇
    //함수: 0개 이상의 값을 받아서 작업을 수행하고 하나의 값을 반환한다.

    //값(리터럴): 1, 2, 3, true, 1.15f, 1.1, FruitType.Apple 등

    //변수선언은 자료형 + 이름
    //C#의 자료형: 구조체(struct), 클래스(class), 이넘(enum)의 3가지
    //int, float, string 등등은 모두 구조체에 해당
    //클래스 밖에서 변수 선언 불가

    //특정 단어에서 F12 누르면 정의로 이동, 다시 돌아가려면 'Ctrl' + '-' 키를 누르면 된다.
    //변수 위에서 Ctrl + R을 두 번 누르면 해당 변수명 전체가 선택되고, 일괄 변경할 수 있다.

    void Start()
    {
        //PrintHelloWorld(10);

        for (int i = 2; i < 10; i++)
        {
            if(i % 2 == 0)
            {
                continue;
            }
            
            Debug.Log($"- {i}단 -");
            for (int j = 1; j < 10; j++)
            {
                Debug.Log($"{i} x {j} = {i * j}");
            }
            Debug.Log("------------------");
        }
    }

    void Update()
    {
        
    }

    void PrintHelloWorld(int count)
    {
        int i = 0;
        while (i < count)
        {
            Debug.Log($"Hello, world{i}");
            i++;
        }
    }
}
