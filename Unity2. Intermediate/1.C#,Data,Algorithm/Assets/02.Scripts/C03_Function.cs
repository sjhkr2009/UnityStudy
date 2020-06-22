using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor.UIElements;
using System.Globalization;

public class C03_Function : MonoBehaviour
{
    // 03. 함수 (Function)
    // 참고로 Function, Method, Procedure는 거의 같은 의미로 쓰이며, C#에서는 주로 '메소드'라 하지만, 게임에 가장 많이 사용되는 C++에서는 주로 Function이라 하기에 함수라고 부른다.

    //코드를 기능별로 분류해놓은 것. '반환형 함수명(매개변수){ 실행할 내용 }' 의 형식을 갖는다.


    //1. 기본 형태
    void HelloWorld() //Hello, World! 를 출력해주는 역할을 하고, 따로 반환형은 없다.
    {
        Debug.Log("Hello, World!");
    }

    int Add(int a, int b) //a,b를 입력하면 둘을 더한 값을 반환한다.
    {
        return a + b;
    }
    [Button]
    void TryFunctions()
    {
        HelloWorld(); //함수명을 통해 외부에서 실행할 수 있다.

        int result = Add(4, 5);//반환형이 있을 때는 해당 유형의 변수처럼 값으로 사용하거나, 다른 변수에 대입할 수 있다.
        Debug.Log($"4 + 5 = {result}");
    }



    //2. 지역 변수와 참조

    void AddOne(int a) //매개변수나 함수 내부에서 정의된 변수는 함수 외부에서 선언된 변수와 다름에 유의할 것.
    {
        a = a + 1; //매개변수에 1을 더한다. 이 a는 외부의 a를 그대로 복사한 다른 참조값이다.
        Debug.Log("AddOne의 a 값 : " + a); //a + 1을 출력한다. 인수로 1을 받았다면 이 값은 2가 될 것이다.
    }
    void RealAddOne(ref int a) //ref(레퍼런스)로 매개변수를 선언하면 변수가 선언된 참조값을 그대로 받아온다.
    {
        a = a + 1; //이 a는 외부에서 받아온 a의 메모리 공간을 그대로 받아왔으므로, 외부의 a 값에 변형을 가한다.
    }
    //ref는 실제 값의 메모리 참조를 받아오되, 반드시 반환하지 않아도 된다는 문제(?)가 있다.
    //반환을 강제하거나 반환할 값이 여러개일 경우 out을 사용한다.
    void WhatISAddOne(int a, out int b, out int c, out int d)
    {
        b = a + 1;
        c = b + 1;
        d = c + 1;
    }
    [Button]
    void TestAddOne() //a = 1을 선언하고, 매개변수로 a를 넣어 AddOne을 실행시킨다.
    {
        int a = 1;
        AddOne(a);
        Debug.Log("AddOne 이후 TestAddOne의 a 값 : " + a); //AddOne에서 a와 b는 AddOne 함수 내의 값이고, 여기서 로그를 띄우는 건 TestAddOne 함수 내의 값이므로 두 a는 별개의 공간에 선언된 다른 변수다.
        //따라서 TestAddOne의 a값은 변한 적이 없으므로 여전히 1이다.

        RealAddOne(ref a); //인자를 ref로 보냈으므로 이 함수의 a 변수를 복사하지 않고 그대로 전달한다.
        Debug.Log("RealAddOne 이후 TestAddOne의 a 값 : " + a); //TestAddOne 함수 내의 a에 1을 더했으니 a는 2가 된다.

        int num1;
        WhatISAddOne(a, out num1, out int num2, out int num3); //반환값을 저장할 공간을 함께 작성한다. 이미 존재하는 변수에 넣어도 되고, 여기서 선언해도 된다.
        Debug.Log($"위의 a값에 1,2,3을 더한 수는 {num1}, {num2}, {num3}"); //a가 2였으니 num1~3은 각각 3,4,5로 반환될 것이다.
    }



    //3. 오버로드

    //위에서 2개의 int를 더하는 함수를 Add로 정의했는데, 더하는 형식과 내용에 따라 Add2, Add3, Add4, ... 등의 함수를 만드는 것은 번거롭다.
    //반환형 또는 매개변수가 다를 경우 동일한 함수명을 사용할 수 있는데, 이를 오버로드라 한다.

    int Add(int a, int b, int c) //3개의 int 값을 인자로 받는 Add 함수
    {
        return a + b + c;
    }
    float Add(float a, float b) //2개의 float 값을 받아 float 값을 반환하는 Add 함수
    {
        return a + b;
    }

    //3.1. 선택적 매개변수

    //몇 개의 인자만 필수적으로 받고, 그 외에는 선택적으로 입력하도록 하는 함수를 생성할 수 있다.
    //선택적인 인자의 경우 기본값을 입력해주고, 필수적 매개변수 뒤쪽에 배치한다.
    int Add(int a, int b, int c = 0, int d = 0, int e = 0, int f = 0, int g = 0)
    {
        return a + b + c + d + e + f + g;
    }
    [Button]
    void TestAdd()
    {
        int a = Add(1, 2, f: 3, e:4); //'매개변수:인자'의 형태로 선택적 매개변수 중 일부 값만 넣어줄수도 있다. C++에선 순서에 유의해야 하지만, C#에서는 순서는 딱히 상관없다.
        Debug.Log(a); //1+2+3+4 = 10
    }




    //4. 응용 - 테스트 코드

    [Button, InfoBox("구구단 출력")]
    void MultiplicationTable()
    {
        for (int i = 2; i <= 9; i++)
        {
            for (int j = 1; j <= 9; j++)
            {
                Debug.Log($"{i} x {j} = {i * j}");
            }
        }
    }
    [Button, InfoBox("지정한 개수만큼 별 계단 쌓기")]
    void StarStair(int number)
    {
        string star = "";
        for (int i = 0; i < number; i++)
        {
            star += "*";
            Debug.Log(star);
        }
    }
    [Button, InfoBox("지정된 변수의 팩토리얼 출력")]
    void Factorial(int number)
    {
        long result = 1;
        for (int i = number; i > 1; i--) result *= i;
        Debug.Log(result);
    }
    [Button, InfoBox("재귀함수를 이용한 팩토리얼 출력")]
    int FactorialInRecursive(int number)
    {
        if (number <= 1) return 1; //1이 되면 1을 그냥 반환한다.
        
        return number * FactorialInRecursive(number - 1); //인수와 인수보다 1 작은 팩토리얼을 곱한 수를 반환한다. 5! = 5*4! = 5*4*3! = 5*4*3*2! = 5*4*3*2*1! = 5*4*3*2*1 의 형태로 반환된다.
    }
}
