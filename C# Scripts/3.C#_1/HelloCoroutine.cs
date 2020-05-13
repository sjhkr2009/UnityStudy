using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloCoroutine : MonoBehaviour
{
    //코루틴의 첫 번째 기능: 대기시간을 삽입한다.
    //두 번째 기능: 코루틴이 시작되고 바로 다음 동작을 실행한다. (일반적인 함수는 함수 하나가 끝나고 다음 줄로 진행)
        //하나의 함수가 끝나기 전에 다른 동작이 동시에 진행되므로, 멀티 쓰레딩 프로그램을 구현할 수 있음.
        //참고) 이를 '비동기(Async) 방식'이라고 함. (함수는 동기(Sync) 방식)

    void Start()
    {
        StartCoroutine("HelloUnity"); // 1.이 함수가 실행되자마자(끝나기 전에)
        StartCoroutine("HiCSharp"); // 2.이 함수가 실행되고
        Debug.Log("End"); // 3.다음 줄인 이 함수가 실핼된다
        //StartCoroutine(HelloUnity()); //이렇게 하면 아래 Update 함수에 있는 것처럼 수동으로 멈출 수 없음.
    }

    IEnumerator HelloUnity()
    {
        while (true)
        {
            Debug.Log("Hello Unity");
            yield return new WaitForSeconds(3f);
        }
        //이런 무한루프는 일반적으로 다음 함수를 진행하지 못 하고(동기 방식) 컴퓨터에 과부하를 줘서(대기시간 없음) 에러를 일으키지만,
        //코루틴은 대기시간 삽입, 비동기 방식이라는 두 가지 특징 때문에 문제없이 실행된다.
        //따라서 코루틴과 무한루프는 좋은 조합이다.

        // yield return null; 이라고 하면 그 프레임만 쉬게 된다. (즉 프레임마다 반복)
    }

    IEnumerator HiCSharp()
    {
        Debug.Log("Hi");
        yield return new WaitForSeconds(5f);
        Debug.Log("C#");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine("HelloUnity"); //코루틴 중지시키기
            Debug.Log("Stop Hellounity");
        }
    }
}
