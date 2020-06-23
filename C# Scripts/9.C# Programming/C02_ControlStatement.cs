using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Threading;

public class C02_ControlStatement : MonoBehaviour
{
    // 02. 제어문

    // 1. 조건문
    void C02_1()
    {
        int hp = 10;
        bool isDead = (hp <= 0);

        if (isDead) //if 뒤에 한 칸을 띄우고 (조건)을 서술하는 것이 가장 보편적임.
        Debug.Log("You are Dead"); //한 줄이면 중괄호를 안 써도, 들여쓰기 여부와 무관하게 if문 다음 줄은 조건문으로 간주한다.

        else if (!isDead) //else if는 앞의 if문이 거짓일 경우에만 조건을 체크한다.
        {
            Debug.Log("You are not Dead");
            Debug.Log($"Your Hp is {hp}"); //2줄 이상이면 중괄호 안에 조건을 서술한다.
        }

        switch (hp) //정수(enum 포함)나 문자열을 넣을 수 있다.
        {
            case 0: //괄호 안의 값이 0이면 시행
                isDead = false;
                break; //실행했으면 switch 문을 빠져나가야 함
            default: //case 중에 해당하는 게 없다면 시행
                isDead = false;
                break;
        }
        //switch문은 분기가 많을 때 간편하지만, 값에 따른 분기문만 만들 수 있어서 넣을 수 있는 조건이 제한적이다.
        //switch문으로 표현되는 건 if 문으로 100% 바꿀 수 있지만, 그 반대는 아님. switch 문으로 서술 가능한 if~else if의 반복은 switch 문으로 변경하면 가독성이 좋다.

        //삼항연산자
        // 변수 = 조건 ? 맞을때 대입 : 틀릴때 대입
        bool isAlive = (hp > 0) ? true : false; //형태가 특이해서 사용자들 사이에서도 호불호가 갈리는 편.
        
    }

    // 1.1. 응용 - 가위바위보 게임

    //0번은 가위, 1번은 바위, 2번은 보로 간주하는 방식은 하드 코딩에 해당하는데, 이러면 가위를 3으로 바꾸는 등의 변화를 주기가 어려워진다.
    //따라서 Rock, Scissors, Paper 등 값에 이름을 붙이는 게 필요하다. (computerChoice에서 구현된 방식)
    //그러나 값이 많아지면 겹치지 않게 하기 어려운데, 이를 enum으로 해결할 수 있다. (playerChoice에서 구현된 방식)

    enum RSPChoice { 가위, 바위, 보 = 2 } //자동으로 0부터 값이 할당되나, 직접 수정하여 정해줄수도 있다. 직접 정해주지 않은 값은 이전 값 + 1의 수를 자동으로 할당받는다.
    [Button]
    void RSP(RSPChoice playerChoice)
    {
        const int scissors = 0; //값처럼 사용하기 위해서는 변하지 않는 const 형태로 선언해야 한다.
        const int rock = 1;
        const int paper = 2; 

        int computerChoice = Random.Range(0, 3); //0이상 3미만의 랜덤한 값을 반환

        switch (computerChoice)
        {
            case scissors:
                Debug.Log("컴퓨터가 가위를 선택했습니다.");
                break;
            case rock:
                Debug.Log("컴퓨터가 바위를 선택했습니다.");
                break;
            case paper:
                Debug.Log("컴퓨터가 보를 선택했습니다.");
                break;
        }

        //경우의 수도 최대한 간편하게 나열할 방법을 찾으면 좋다.
        if ((int)playerChoice == computerChoice) Debug.Log("무승부입니다."); //두 선택이 같다면 무승부
        else if ((playerChoice == RSPChoice.가위 && computerChoice == paper) || (playerChoice == RSPChoice.보 && computerChoice == rock) || (playerChoice == RSPChoice.바위 && computerChoice == scissors))
            Debug.Log("승리했습니다.");
        else Debug.Log("패배했습니다."); //무승부와 승리 조건 3가지 외에는 패배 처리
    }
    


    // 2. 반복문

    void C02_2()
    {
        //크게는 for과 while로 나뉜다.
        //둘의 문법은 다르지만 역할 자체는 동일해서, for문으로 할 수 있는건 while문으로도 할 수 있고 그 반대도 성립한다.

        //while : 조건에 맞는 한 반복하는 방식. while(조건){ 실행할 내용 } 으로 서술된다.
        //빠져나갈 수단이 없다면 무한히 반복하여 과부하를 일으키게 되므로 주의
        int count = 0;
        while(count < 3) //count가 3 미만이면 반복 시행. 여기선 0,1,2일 때 한 번씩 총 3번 반복 실행된다.
        {
            Debug.Log("Hello, World! (in while)");
            count++; //이 부분을 빼면 count가 0으로 유지되어 무한루프에 빠짐
        }

        //do ~ while : 한 번 실행한 후 조건을 계산한다. 자주 사용되지는 않는다.
        do
        {
            Debug.Log("Goodbye, World!!");
        } while (false);

        //for : 초기 값에서 반복할 때마다 값을 변화시켜, 조건에 맞는 한 반복되는 반복문. for (초기화문; 조건문; 반복문) { 실행할 내용 } 으로 서술된다.
        //조건문의 구조를 보기 편하기 때문에 while보다 자주 사용된다. 실행 순서는 초기화문 -> [조건문 -> 실행할 내용 -> 반복문] -> [조건문 -> 실행할 내용 -> 반복문] -> ... (조건문이 틀릴 때까지 반복)

        for (int i = 0; i < 3; i++) // 3개의 부분을 다 채울 필요는 없이 for(;;){실행할 내용} 으로 입력해도 작동한다. 다만 이 경우 무한루프에 빠지므로 대부분 괄호 안의 3가지를 다 입력한다.
        {
            Debug.Log("Hello, World! (in for)");
        }

        //반복문 중단

        //continue : 이번 반복에 한하여 실행할 내용을 무시한다.
        //break : 반복을 중단하고 빠져나온다. 해당 반복문의 실행할 내용이 서술된 중괄호를 벗어난 것으로 간주한다.
        for (int i = 0; i < 10; i++)
        {
            if (i == 3) continue; //3일 때는 무시하고 다음 반복으로 넘어간다.
            else if (i == 6) break; //6일 때는 반복문을 즉시 빠져나간다.
            Debug.Log($"Hello, {i + 1}"); //즉 반복문의 조건만 보면 10회 반복이지만, 1,2,4,5일 때만 실행된다.
        }
    }

    //  2.1. 응용 - 소수 여부 판별하기
    [Button]
    void IsPrime(int number)
    {
        bool isPrime = true;

        for (int i = 2; i < number; i++)
        {
            if((number % i) == 0)
            {
                isPrime = false;
                break;
            }
        }

        if (isPrime) Debug.Log("소수입니다.");
        else Debug.Log("소수가 아닙니다.");
    }

    //  2.2. 응용 - 약수 알아내기
    [Button]
    void AliquotCheck(int number)
    {
        for (int i = 1; i < number; i++)
        {
            if ((number % i) != 0) continue;
            Debug.Log($"{number}의 약수 : {i}");
        }
    }
}
