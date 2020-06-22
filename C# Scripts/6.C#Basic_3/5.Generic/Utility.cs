using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    // 제네릭: 변수의 유형을 직접 만들어 쓰는 것. <타입명>의 형태로 구현한다. 대표적인 예로 유니티는 GetComponent<타입명>(); 으로 특정 타입의 컴포넌트를 가져와서 사용한다.
    // 참고) 제네릭을 가져다 쓰는 경우는 있어도, 제네릭을 직접 구현할 일은 거의 없다. 가령 List<타입명> 을 쓰는 경우는 많지만, 직접 제네릭 클래스를 작성하는 경우는 드물다.
    
    //받은 값을 그대로 출력하는 Print 함수를 작성한다고 가정하자.
    //기존에 배운 오버로드를 사용해서 같은 함수에 여러 입력을 줄 수는 있지만, 입력 유형(정수, 소수, 문자 등등)에 따라 함수를 여러 번 작성해야 하는 번거로움은 남아 있다.
    /*
    public void Print(int inputMessage)
    {
        Debug.Log(inputMessage);
    }
    public void Print(string inputMessage)
    {
        Debug.Log(inputMessage);
    }
    public void Print(float inputMessage)
    {
        Debug.Log(inputMessage);
    }
    */

    //이를 제네릭을 사용하면 다음과 같이 작성할 수 있다.
    public void Print<T> (T inputMessage) // T에 대해서 Print 함수를 구현한다. T 자리에 다른 어떤 이름을 붙여도 상관없다.
    {
        Debug.Log(inputMessage);
    }

    //이후 함수를 사용할 때 입력값의 타입을 정해준다.
    private void Start()
    {
        Print<int>(30);
        Print<string>("Hello World!");






        //변수뿐만 아니라 클래스에도 사용할 수 있다.

        Container<string> con = new Container<string>();
        con.messages = new string[3];

        con.messages[0] = "Hello";
        con.messages[1] = "Unity";
        con.messages[2] = "Generic";

        for(int i = 0; i < con.messages.Length; i++)
        {
            Debug.Log(con.messages[i]);
        }

        Container<int> con2 = new Container<int>();
        con.messages = new string[3];

        con2.messages[0] = 2;
        con2.messages[1] = 4;
        con2.messages[2] = 8;

        for (int i = 0; i < con2.messages.Length; i++)
        {
            Debug.Log(con.messages[i]);
        }
    }
}

//메시지들을 가지고 있는 데이터 모음. 다양한 메시지 유형에 대응시키고 싶다.
public class Container<T>
{
    public T[] messages;
}

//이 예시로는 리스트 사용 시 List<int> 식으로 불러와 사용하는 경우가 있다.
