using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class C15_Exception : MonoBehaviour
{
    // 15. 예외 처리 (Exception)

    // 에러가 났을 때 처리해줄 동작. using System 필요.
    // 게임에서는 거의 쓰지 않는다. 게임에서 에러를 이런 식으로 넘겨봐야 제대로 동작하지 않는 건 마찬가지라, 잘못된 코드 자체를 수정해야 하기 때문.
    // 오히려 에러를 나게 해서 해당 부분을 빠르게 수정하는 게 낫다.


    // 1. 기본 형태

    [Button]
    void Test01()
    {
        int a = 5;
        int b = 0;
        //int c = a / b;    //이렇게 0으로 나누면 DivideByZeroException 이라는 에러가 뜬다.

        try
        {
            int c = a / b;
            Debug.Log("계산 명령을 끝냈습니다.");
        }
        catch(DivideByZeroException e)
        {
            Debug.Log("0으로 나눌 수 없습니다."); //에러가 뜨는 대신 catch로 넘어간다. DivideByZeroException에 해당하므로 이 부분을 실행한다.
        }
        catch(Exception e) //Exception
        {
            Debug.Log("알 수 없는 에러"); //위에서 잡아내지 못 한 에러일 경우 다음 catch로 넘어간다. Exception은 모든 에러를 잡아내는 부분이다.
        }
        finally //에러 체크를 끝낸 후 추가로 할 동작은 finally에 입력한다.
        {
            Debug.Log("계산 끝!");
        }

        Debug.Log("---------------------------------");

        try
        {
            int c = 0; //여기선 에러가 나지 않는다.
        }
        catch(Exception e)
        {
            Debug.Log("알 수 없는 에러");
        }
        finally //finally는 에러를 잡든 못 잡든 마지막에 무조건 실행된다.
        {
            Debug.Log("c에 0을 대입했습니다.");
        }

        Debug.Log("---------------------------------");

        C15_EmptyClass emptyClass = null;
        //emptyClass.a = 0; //null인 클래스에서 값에 접근하려 하면 Null Reference 에러가 뜬다.

        try
        {
            emptyClass.a = 0;
        } 
        catch(DivideByZeroException e)
        {
            Debug.Log("0으로 나눌 수 없어요.");
        }
        catch(NullReferenceException e)
        {
            Debug.Log("클래스가 비어있어요."); //이 부분에서 에러를 잡아낼 것이다.
        }
        catch(Exception e)
        {
            Debug.Log("알 수 없는 에러");
        }

        Debug.Log("실행 끝!"); //에러를 잡아내면 그 뒤의 catch는 무시하고 다음 코드로 넘어간다.
    }



    //-------------------------------------------------------------------------------




    // 2. 커스텀 에러 클래스

    [Button]
    void Test02()
    {
        try
        {
            throw new C15_TextException(); //Exception을 상속한 인위적인 에러 코드를 만들고, throw로 에러 클래스를 던질 수 있다.
        }
        catch(C15_TextException e)
        {
            Debug.Log("던진 에러 받음");
        }
        Debug.Log("실행 끝!");
    }

}

class C15_EmptyClass
{
    public int a;
}
class C15_TextException : Exception { }