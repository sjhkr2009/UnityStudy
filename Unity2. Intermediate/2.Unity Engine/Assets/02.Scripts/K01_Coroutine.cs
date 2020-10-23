using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K01_Coroutine : MonoBehaviour
{
    // 코루틴
    // 1. 함수의 상태를 저장하거나 복원하는 기능
    // -> 엄청 오래 걸리는 작업을 잠시 끊거나, 원하는 타이밍에 함수를 중지/재실행하는 경우에 사용한다.
    // 2. return -> 클래스를 포함하여 원하는 타입으로 리턴 가능


    // ex: 아이템 생성

    // 아이템을 만들고, DB에 저장한 후 다음 로직을 실행한다.
    // 문제점: DB 저장에 실패했는데 다음 로직을 계속 실행하면, 클라이언트에는 있는 아이템이 DB에 없을 수 있다.
    //  -> 아이템 복사 버그가 생기거나, 강화에 성공했는데 서버에 재접속하니 이전 상태로 롤백되어있는 현상 등이 발생할 수 있다.

    // 따라서 DB에 요청을 보내고 함수를 중단한 뒤, 응답이 오면 다음 로직을 시행하게 할 수 있다.


    // ex2: 스킬 사용

    // 사용 4초 후 폭발하는 스킬이 있을 때, 가장 간단한 방법은 deltaTime을 통해 시간을 체크하여 4초가 초과하면 폭발을 실행하게 하는 것이다.
    // 문제점: 한두 번이면 상관없겠지만, 서버 등 시간을 체크해야 하는 함수는 무수히 많다. 여기서 모두 deltaTime을 더하고 실행 여부를 확인하는 것은 낭비다.
    //  -> 대부분의 게임에서는 시간을 관리하는 공용 시스템이 존재한다. 시간 관리자에 4초 후 알려줄 것을 요청하면 4초가 지나고 알려주는 방식.



    class Test
    {
        public int Id { get; set; }
    }
    class CoroutineTest : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < 100000; i++)
            {
                // 무거운 작업을 한 프레임에 전부 실행하지 않고, 특정 횟수만큼 실행한 후 다음 프레임으로 넘기거나 일정 시간 쉴 수 있다.
                if (i % 1000 == 0)
                    yield return null;
            }
            
            yield return new Test { Id = 1 };
            yield return new Test { Id = 2 };
            yield return new Test { Id = 3 };

            //도중에 빠져나갈 때는 yield break 를 사용한다.
            yield break;
            yield return new Test { Id = 4 };
        }
        public IEnumerator ExplodeAfterSeconds(float seconds)
        {
            Debug.Log("폭발 대기");
            yield return new WaitForSeconds(seconds);
            Debug.Log("폭발!");
        }
    }

    Coroutine co;

    // 코루틴은 다른 함수와 구분 가능하게 Co 등의 약어를 붙이기도 한다.
    IEnumerator CoStopExplode(float seconds = 0f)
    {
        Debug.Log("폭발 중지 대기");
        yield return new WaitForSeconds(seconds);
        Debug.Log("폭발 중지");

        if(co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }

    void Start()
    {
        CoroutineTest test = new CoroutineTest();

        foreach (var t in test)
        {
            if (t == null) continue;
            Test value = (Test)t;
            Debug.Log(value);
        }

        // 다른 클래스에 정의된 코루틴은 다음과 같이 사용한다.
        StartCoroutine(test.ExplodeAfterSeconds(4));

        // 코루틴의 중지는 다음과 같이 사용한다.
        Coroutine coroutine = StartCoroutine(test.ExplodeAfterSeconds(4));
        StopCoroutine(coroutine);

        // 코루틴 중지도 코루틴을 이용하여 다음과 같이 사용할 수 있다.
        co = StartCoroutine(test.ExplodeAfterSeconds(4f));
        StartCoroutine(nameof(CoStopExplode), 2f);
    }
}
