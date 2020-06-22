using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Messenger : MonoBehaviour
{
    // 람다 함수: 익명함수. 이름을 붙여 선언한 다음 불러오지 않고, 즉석에서 만들어낸다.
    // '(입력값, 입력값) => { 수행할 동작; 수행할 동작; };' 형태로 작성된다. 입력값이나 수행할 동작이 1개라면 '입력값 => 수행할 동작;' 의 축약형으로 사용 가능.

    public delegate void Send(string receiver);
    Send send;

    
    void SendMail(string man)
    {
        Debug.Log(man + " 에게 메일을 보냄: ");
    }

    void SendMoney(string man)
    {
        Debug.Log(man + " 에게 송금 완료!");
    }

    private void Start()
    {
        send += SendMail;
        send += SendMoney;

        //여기서 기능을 추가하고 싶다면?
        //위에 함수를
        /*
         * void Assainate(string man)
            {
                Debug.Log("Assainate" + man);
                Debug.Log("Hide Body");
            }
            */
        //이렇게 추가해줄 수도 있지만, 아래의 한 줄로 대체할 수 있다.
        send += (string man) => { Debug.Log("Assainate " + man); Debug.Log("Hide Body"); };

        //입력이 하나이거나, 내용이 한줄이면 다음과 같이 축약할 수 있다. (괄호 혹은 중괄호 생략)
        send += man => Debug.Log("Assainate " + man);

        //한 줄씩 적고싶다면 다음과 같이 작성해도 된다.
        send += man =>
        {
            Debug.Log("Assainate " + man);
            Debug.Log("Hide Body");
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            send("Ho");
        }
    }
}
