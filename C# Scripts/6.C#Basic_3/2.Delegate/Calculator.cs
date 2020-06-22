using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    // Delegate: 할 일을 적어둔 명단. 어떤 기능을 목록에 추가해두면 델리게이트가 대신 발동시켜준다. 단, 조건에 맞지 않는 함수는 발동하지 않는다.

    //여기서는 계산 기능 함수를 담은 스크립트를 써서, 델리게이트를 통해 사용해 보기로 한다.

    delegate float Calc(float a, float b); //Calc 라는 이름의 델리게이트를 만든다. float 두 개를 받아 float로 반환하는 함수를 대행할 수 있다.

    Calc calc; //이 델리게이트를 변수를 통해 가져온다.

    
    public float Sum(float a, float b)
    {
        Debug.Log(a + b);
        return a + b;
    }

    public float Subtract(float a, float b)
    {
        Debug.Log(a - b);
        return a - b;
    }

    public float Multiply(float a, float b)
    {
        Debug.Log(a * b);
        return a * b;
    }


    //기존 방식
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Sum(1, 10);
            Subtract(1, 10);
            Multiply(1, 10);
        }
        //이렇게 하면 발동하려는 함수를 모두 명시해야 한다.
        //또한 계산 방식이나 내용을 바꾸고 싶다면 코드를 매번 수정해야 한다.
    }
    */

    //델리게이트 사용
    private void Start()
    {
        calc = Sum; //델리게이트 목록에 Sum 함수를 추가한다. 함수를 호출하고 결과값을 받으려는 게 아니므로, ()는 붙이지 않고 함수명만 적는다.
        calc = calc + Subtract; //기존 델리게이트에 다른 함수를 추가할 수도 있다. 변수에 목록을 넣는 것이므로, 그냥 calc = Subtract 등으로 대입해버리면 기존 값에 추가되지 않고 덮어씌워진다.
        calc += Multiply; //(좀 더 간단한 형태)
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("결과값: " + calc(1, 10)); //calc 하나로 목록에 있는 3개의 함수 모두 발동

            calc -= Subtract; //원치않는 함수는 중간에 뺄 수도 있다.
            Debug.Log("결과값: " + calc(1, 10));
        }
    }

}
