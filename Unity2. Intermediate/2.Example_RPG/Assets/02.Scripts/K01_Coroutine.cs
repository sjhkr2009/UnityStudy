using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K01_Coroutine : MonoBehaviour
{
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
    }
}
