using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloCSharp : MonoBehaviour
{
    
    void Start()
    {
        //형변환(casting) : 서로 다른 데이터들끼리 값을 주고받음
        int height = 170;
        float heightDetail = 170.3f;

        heightDetail = height; //문제 없음: 잃어버리는 정보가 없으니 자동으로 형변환됨 (암묵적 형변환)

        int weight = 56;
        float weightDetail = 55.8f;

            //weight = weightDetail; // Error: 소수점 이하를 잃어버리게 됨.
        weight = (int)weightDetail; //강제 형변환 (소수점 이하는 버림)




        //조건문(if)
        bool isMan = true;

        if(isMan == true)
        {
            Debug.Log("남자");
        }else if (!isMan) //'해당 변수가 false라면'
        {
            Debug.Log("여자");
        }

        isMan = false;
        if (isMan != true)
        {
            Debug.Log("여자");
        }
        else if (isMan) //'해당 변수가 true라면'
        {
            Debug.Log("남자");
        }


        int love = 70;
        if (love < 50)
        {
            Debug.Log("배드엔딩");
        }
        else if (love >= 50)
        {
            Debug.Log("해피엔딩");
        }


        int age = 55;
        if(age <= 19 || age >= 65) //'||'은 '또는(or)'의 의미
        {
            Debug.Log("일을 하지 않음");
        }
        else if (age > 19 && age < 65) //'&&'은 '그리고(and)'의 의미
        {
            Debug.Log("일할 나이");
        }

        Debug.Log("! true = " + (!true)); // 앞에 느낌표를 붙이면 true/false가 뒤집힌다.


        //switch 분기문
        //if 문으로 대체 가능하지만, 변수의 값에 따라 서로 다른 사항을 실행시킬 때 더 편리함

        int year = 2017;

        switch (year)
        {
            case 2009: //year가 2009일 경우 아래 사항을 실행
                Debug.Log("괴물");
                break; //종료 선언 필수

            case 2012:
                Debug.Log("레미제라블");
                break;

            case 2017:
                Debug.Log("트랜스포머5");
                break;

            default:
                Debug.Log("해당사항 없음");
                break;
        }



        //루프(Loop)문, 반복문

        //for문 : 순번을 넘기며 반복
        //for(반복 시작점; 반복하는 조건; 각 반복 사이에 실행할 것)
        for (int i = 2; i <= 10; i += 2) //i = 1부터, 10 이하일 때 반복, 반복마다 i를 1씩 증가시킴
        {
            Debug.Log("현재 순번: " + i);
        }
        Debug.Log("루프 끝");

        //while : 조건이 맞는 한 영원히 돌아감
        bool isShot = false;
        int index = 0;
        int luckyNumber = 4;

        while(isShot == false)
        {
            index++;
            Debug.Log("현재 시도: " + index);
            if (index == luckyNumber)
            {
                Debug.Log("총알에 맞았다!");
                isShot = true;
            }
            else
            {
                Debug.Log("총알에 맞지 않았다.");
            }
        }

        //do{}while: while문과 유사하지만, 무조건 1회는 실행 후 조건을 체크
        index = 0;
        isShot = true;

        do
        {
            index++;
            Debug.Log("현재 시도: " + index);
            if (index == luckyNumber)
            {
                Debug.Log("총알에 맞았다!");
                isShot = true;
            }
            else
            {
                Debug.Log("총알에 맞지 않았다.");
            }
        } while (isShot == false);



    }
    
}
