using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class C04_Debugging : MonoBehaviour
{
    // 04. 디버깅

    // 프로그래밍을 할 때 코딩보다 디버깅에 할애하는 시간이 더 크다.

    //코드 왼쪽을 클릭하면 붉은색의 break point가 찍힌다. 디버그를 하면 프로그램이 이 지점에서 멈추고 대기한다. 이 상태에서...

    // F10 : 코드의 다음 줄로 이동. 다른 참조나 함수를 읽어올 때도 그 쪽으로 이동하지는 않는다.
    // F11 : 다음 동작 하나를 실행. 도중에 다른 함수를 만나면 해당 함수 안으로 이동한다. 도중에 특정 변수의 값을 조작할수도 있다.

    // break point 위의 노란 화살표를 드래그하여 다음으로 실행할 부분에 드랍하면, 함수의 실행 순서도 조작할 수 있다.

    //-----------------------------------------------------------------------------


    //break point에서 우클릭 시 여러 옵션이 나온다.

    // 중단점 삭제: 포인트를 삭제한다. 마우스 왼쪽 클릭으로도 가능.
    // 중단점 해제: 중단점 삭제와 유사하나 흔적을 남긴다. 나중에 확인해야 할 경우 표시해둘 수 있다.

    // 조건: 해당 줄에서 코드 실행을 멈출 조건을 설정할 수 있다.
    //예를 들여 필드상의 오브젝트를 관리하는 오브젝트에서, 특정 오브젝트의 실행을 확인하고 싶다면 그 오브젝트의 ID만 조건으로 걸어 확인할 수 있다.




    [Button]
    void DebugStart()
    {
        HelloWorld();
    }

    void TestStart(int a = 1)
    {
        Debug.Log("Start!");
        if(a != 1) Debug.Log($"Real Start : {a}");
    }

    void HelloWorld()
    {
        TestStart();
        TestStart(0);
        Debug.Log("Hello, World!");
    }
}
