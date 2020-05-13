using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloMath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int a = 5;
        int b = 7;
        int sum = a + b;
        Debug.Log(sum);

        sum = a - b;
        Debug.Log(sum);

        Debug.Log(a * b);

        Debug.Log(a / b); //몫 출력
        Debug.Log(b / a);

        Debug.Log(a % b); //나머지 출력
        Debug.Log(b % a);


        Debug.Log("------절취선------");


        int i = 0;
        i = i + 1;
        Debug.Log(i);

        i++; //i = i + 1; 과 동일함
        Debug.Log(i);
        i--; //i = i - 1; 과 동일함
        Debug.Log(i);

        ++i; //위와 동일
        Debug.Log(i);
        --i; //위와 동일
        Debug.Log(i);

        //차이점
        a = 0;
        Debug.Log(a++); //a를 먼저 출력하고 ++을 연산
        Debug.Log(a);

        b = 0;
        Debug.Log(++b);
        Debug.Log(b); //++를 먼저 연산하고 b를 출력


        Debug.Log("------절취선------");


        //자기 자신을 연산할 때

        a = 10;
        a = a + 5;
        Debug.Log(a);
        a += 5; // a = a+5 와 동일함
        Debug.Log(a);

        //다른 연산자에도 응용 가능
        a -= 5;
        Debug.Log(a);

        a *= 5;
        Debug.Log(a);

        a /= 5;
        Debug.Log(a);

        a %= 5;
        Debug.Log(a);
        
    }
    
}
