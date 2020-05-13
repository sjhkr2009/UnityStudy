using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloUnity : MonoBehaviour
{
    
    void Start()
    {
        //주석: 컴퓨터가 처리하지 않는 라인
        /*
        메모 용도로 사용
        */

        //콘솔 출력
        Debug.Log("Hello, world!");

        //정수형 변수
        int age = 24;
        int money = -1000;

        Debug.Log(age);
        Debug.Log(money);

        //소수형 변수(실수)
        float heignt = 169.704f; //끝에 f 붙임, 소수점 아래 7자리까지만 정확, 그 이상은 근사값으로 처리.
        double pi = 3.14159265359; //끝에 f 없음, 소수점 아래 15자리까지 정확, float보다 메모리를 두 배로 사용함.

        //참 혹은 거짓
        bool isMan = true;
        bool isWoman = false;

        //문자 하나
        char grade = 'C';

        //문장
        string subject = "독일어2";

        Debug.Log("내 나이는: " + age);
        Debug.Log("내 성적은: " + grade);


        //할당하는 값을 기준으로 자동으로 타입을 결정
        var myName = "Jiho"; //string myName = "Jiho"; 와 동일함
        var myAge = 24; //int myAge = 24; 와 동일함

        Debug.Log("이름: " + myName);

    }
}
