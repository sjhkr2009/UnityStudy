﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class C01_DataType : MonoBehaviour
{
    // 01. 자료형
    
    private void Start()
    {
        // 1. 자료형 정리

        //정수형
        byte aByte; //1바이트 (0~255 범위 표현)
        short aShort; //2바이트 (약 -3만~3만 범위 표현)
        int aInt; //4바이트 (약 -21억~21억)
        long aLong; //8바이트 (일반적으로 쓸 일은 없으나, 대형 온라인 게임에서 아이템 수가 총합 21억개를 넘어가는 경우가 있다. 따라서 고유 ID 등은 long으로 선언하는게 좋다.

        sbyte aSbyte; //1바이트 (-128~127)
        ushort aUshort; //2바이트 (약 0~6만)
        uint aUint; //4바이트 (약 0~42억)
        ulong aUlong; //8바이트 (0~)
                      //마이너스를 표현할 수 있는 자료형에서, 첫 비트는 양수/음수를 나타낸다.
                      //sbyte에서 이진수로 01111111은 127이지만, 11111111은 첫번째 1이 -128을 의미하고 뒤의 127이 더해져 -1이 된다.

        //참-거짓
        bool aBool = true; //1바이트, true 또는 false의 값을 갖는다.

        //소수
        float aFloat = 3.14f; //4바이트 (소수점 이하 7자리까지 정확, f를 붙이지 않으면 double로 인식)
        double aDouble = 3.14; //8바이트 (소수점 이하 15자리까지 정확)

        //문자
        char aChar = 's'; //2바이트, 작은따옴표 사용, 하나의 문자만을 저장 가능
        char bChar = '4';
        char cChar = '가';
        string aString = "Hello, World!"; //큰 따옴표 사용, 문자열을 저장 가능

        //--------------------------------------------------------------------------------------

        //1.1. 2진수와 16진수

        //2진수는 숫자 앞에 0b를 붙여서, 16진수는 0x를 붙여서 표현한다. 16진수에서 10~15는 a~f의 알파벳으로 표시하며, 대/소문자는 구분하지 않는다.
        aInt = 0b1101; //(=13)
        aInt = 0x2F; //(=47)

        //--------------------------------------------------------------------------------------

        //1.2. var

        //var은 타입을 컴파일러가 알아서 찾도록 한다.

        var a = 3;
        var b = "Hello World!";

        //하지만 C++이나 C#의 장점은 타입을 강력하게 지정시켜 준다는 것.
        //var의 남용은 자제하고, 기존 타입으로 선언할 수 있는 경우엔 가급적 그쪽을 사용하자.






        //--------------------------------------------------------------------------------------

        // 2. 형변환

        //1) 저장공간의 크기가 다른 경우
        int bInt = 1000;
        byte bByte = 10;
        aInt = bByte; //더 큰 바구니로는 바로 대입 가능
        aByte = (byte)bInt; //더 작은 바구니로는 명시적으로 캐스팅해줘야 한다. 단, 일부 데이터 손실 발생 가능.
                            //값이 저장범위를 초과 또는 미달하는 것을 오버플로우(overflow) 또는 언더플로우(underflow)라고 한다.
                            //이 경우 1의 자리부터 데이터 범위가 허용하는 공간까지 표현한다.

        int cInt = 0x0FFFFFFF;
        short bShort = (short)cInt; //이 때 short는 2바이트만 표현하므로 16진수 '0FFFFFFF'에서 뒤의 4자리인 'FFFF'만 표현한다.
                                    //0x FFFF는 이진수로 0b 1111 1111 1111 1111인데, 십진수로는 -1에 해당하므로 bShort는 -1이 된다.

        //2) 저장공간은 같으나 부호가 다른 경우

        aByte = 255; //(= 0b11111111)
        aSbyte = (sbyte)aByte; //명시적으로 캐스팅해야 하며, 비트 값 11111111을 그대로 변환한다. 즉 aSbyte는 십진수로 -1이 된다.

        //3) 소수 변환
        aFloat = 3.14f;
        aDouble = aFloat; //소수 변환 시 주의할 점은, 정확히 같은 값으로 변환되지 않는다는 것이다. aDouble은 소수점 이하 8자리부터 오차가 발생하여 3.140000039201043과 같은 식으로 변환된다.







        //--------------------------------------------------------------------------------------

        //3. 데이터 연산

        // + - * / % ++ -- += -= *= /= %= < > <= >= == != && ||

        aInt = 100 / 3; //정수 몫인 33이 반환된다. //float끼리 나눌 경우 float가 반환된다.
        aInt = 100 % 3; //나머지인 1이 반환된다.

        int hp = 100;
        hp++; //hp에서 1을 더한 값을 hp에 대입한다.
        hp--; //hp에서 1을 뺀 값을 hp에 대입한다.
              //Debug.Log(hp++)을 실행하면 1을 더하기 전의 값이 나온다. 이 순서를 바꿔 1을 더한 값을 바로 대입하고 싶다면 ++hp; 로 순서를 바꾸면 된다. --hp도 마찬가지.

        hp += 3; // hp = hp + 3과 같음
        hp %= 5; // hp = hp % 5와 같음

        aBool = hp > 100; //조건이 맞으면 true, 틀리면 false를 반환
        aBool = hp <= 100; //같거나 작은, 같거나 큰 비교를 위해서는 부등호 뒤에 =을 붙인다.
        aBool = (hp < 500 && 3 > 5) || 2 > 1;  //둘 이상의 연산에는 &&과 ||을 사용하며, &&은 앞뒤의 조건이 둘 다 참일 경우만 true를, ||는 둘 다 거짓일 경우만 false를 반환한다.
                                               //&&과 ||은 부등호보단 나중에 계산되며, &&와 || 사이엔 우선순위 없이 앞에서부터 순서대로 비교한다.

        //--------------------------------------------------------------------------------------

        //3.1. 비트 연산

        // << >> &(and) |(or) ^(xor) ~(not)
        // 비트연산도 자기 자신에 대한 연산일 경우 >>=, ~=, &= 등으로 줄일 수 있다.

        aInt = 4; //(= 0b100)
        aInt = aInt << 1; //이진수로 표현된 모든 비트를 1칸씩 왼쪽으로 민다. 즉 0b100 -> 0b1000으로, aInt는 8이 된다.
        bInt = 5; //(= 0b101)
        bInt >>= 1; //<<=, >>=로도 표현 가능. 이 때 1의 자리에 있던 1은 사라지고 0b10, 즉 2가 된다.

        aByte = 12; //(= 0b1100)
        aByte = (byte)(aByte >> 1); //0b1100(= 12) -> 0b0110(= 6)으로, 6이 된다. 비트연산 후에는 int 자료형이 반환되므로 int가 아니라면 명시적으로 캐스트해야 한다.

        //~는 각 자리수의 0과 1을 뒤바꾼다.

        /*
         * 주의: 음수를 포함하는 자료형에서는 가장 큰 자리수의 비트가 부호(+,-)를 나타내는데, 이 비트는 오른쪽 또는 왼쪽으로 밀리기는 하지만, 연산이 끝난 후 부호를 표시하는 비트는 원래대로 돌아온다.
         * 즉 sbyte 자료형에서 0b10101010이 있을 때, >> 1 연산을 하면 0b01010101이 되는데, 맨 앞자리는 원래의 1로 돌아가서 0b11010101이 된다.
         * 그래서 비트연산 시 음수를 포함하지 않는 uint 등의 자료형으로 변환시키는 게 좋다.

        */

        aByte = 15; //(= 0b1111)
        bByte = 36; //(= 0b100100)
        byte cByte = (byte)(aByte & bByte); //&은 각 자리수가 모두 1일 때만 1을 반환한다. 여기서는 0b00001111과 0b00100100을 비교한다.
                                            //0 0 0 0 1 1 1 1
                                            //0 0 1 0 0 1 0 0
                                            //0 0 0 0 0 1 0 0 -> 계산 결과는 0b100 = 4
                                            // |는 하나라도 1이면 1을 반환한다.
                                            // ^는 두 값이 다르면 1을, 같으면 0을 반환한다.

        //비트 연산은 주로 ID 생성 또는 암호화에 사용된다.

        // 1) ID 생성
        // 세번째 NPC에 3 (= 0b11)이라는 아이디를 부여하고, <<28만큼 비트연산을 하면 약 8억의 숫자를 가지게 된다.

        // 2) 암호화
        // 같은 값을 두 번 ^(xor) 연산하면 처음과 동일한 값이 나온다. 예를 들어 다음과 같이 사용된다
        int id = 123;
        int key = 401;

        aInt = id ^ key; //이 상태로 데이터를 전송하면, key 값을 모르는 사람은 aInt 데이터를 가로채도 무슨 수인지 알 수 없다.

        bInt = aInt ^ key; //다시 key를 ^연산하면 원래의 123이 나온다.

        //--------------------------------------------------------------------------------------

        //3.2. 연산 우선순위

        /*
         * 1. ++ --
         * 2. * / %
         * 3. + -
         * 4. << >>
         * 5. < >
         * 6. == !=
         * 7. &
         * 8. ^
         * 9. |
         * ...
         *
        */

        //단, 실제로 우선순위를 전부 외우지는 않고, 우선순위를 가져야 하는 부분은 괄호로 묶는 게 좋다.
        
    }
}