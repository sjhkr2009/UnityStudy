using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Delegate : MonoBehaviour
{
    public delegate void TestDelegate(); //델리게이트: 함수 포인터. 함수의 주소를 저장한다.
                                         //클래스가 참조하기 위한 인스턴스들의 주소를 담는 변수라면, 델리게이트는 함수의 주소를 담아두는 변수.

    private TestDelegate del; //델리게이트 자료형을 생성한 후, dele 변수를 만든다.

    //느슨한 결합 - 고급 코딩 방식
    //A 클래스가 입력을 받고, B클래스는 이동을 한다.
    //A와 B가 서로를 모르더라도, 게임 매니저를 통해서 A의 입력에 따라 B가 이동하게 할 수 있다.
    //A가 입력을 받으면 게임 매니저에게 알려주고, B에게 이동하라고 지시한다. (예를 들어 플레이어의 스크립트에 transform을 포함시키지 않고도 제어할 수 있다) 
    //A와 B를 강하게 결합시킨다면, 입력을 수정할 경우 플레이어에게도 영향을 미칠 수 있다.


    void Wow() //인자가 없고, 반환형이 없음(void).
    {
        Debug.Log("Wow");
    }

    void Hey()
    {
        Debug.Log("Hey");
    }

    [Button]
    void Init()
    {
        del = Wow; //같은 형태의 함수를 이렇게 저장할 수 있다.
        del(); //지금 델리게이트에 들어 있는 Wow()를 실행한다.
    }

    [Button]
    void Init2()
    {
        del += Hey; //내부적으로 리스트처럼 되어 있어서 여러 함수를 넣을수도 있다. 이 경우 델리게이트 내에 저장된 함수들을 차례로 출력한다.
        del();
    }

    //주의할 점
    //1. 델리게이트가 현재 null이 아닌지 확인하고 쓴다.
    //2. 다 쓴 다음에는 델리게이트에서 함수를 빼 준다.
}