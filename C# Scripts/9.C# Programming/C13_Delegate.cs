using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class C13_Delegate : MonoBehaviour
{
    // 13. 대리자(Delegate)

    // 1. 델리게이트
    // delegate : 함수 자체를 저장할 수 있는 형식. 인자와 반환형이 동일할 경우에만 저장 가능하다.

    // callback 방식. 업체 사장에게 연락하면 대리자인 비서가 받는데, 비서에게 용건을 알려주고 거꾸로 우리에게 다시 연락을 달라는 개념.

    // (특히 UI작업에서) 특정 버튼을 누르면 동작을 수행하도록 해야 할 때가 많다.
    // 하지만 UI를 눌렀을 때 할 작업들을 쭉 나열하기엔,
    // 1. 게임 내 동작에 관한 함수와 UI 동작에 관한 함수는 분리되어 있는 게 (스파게티 코드 방지에) 좋고,
    // 2. 일부 함수는 외부에서 호출하거나 변형(인자를 달리하여 입력하는 등)이 불가능한 경우가 있다.

    // 따라서, 함수 자체를 인자로 넘겨줘서 발동되었음을 알리고, 이 때 수행되어야 할 함수들을 호출한다.


    public delegate int OnClicked(); //인자가 없고 int 반환형을 가진 함수를 저장할 수 있다.
    int ClickTest()
    {
        Debug.Log("클릭 감지");
        return 0;
    }
    int ClickTest2()
    {
        Debug.Log("클릭 감지2");
        return 0;
    }

    public void ButtonPressed(OnClicked clickedFunction)
    {
        clickedFunction();
    }

    [Button]
    void Test01()
    {
        //델리게이트를 매개변수로 작동하는 함수에, 인자(파라미터)로 함수를 넣어주면 된다. 인자는 델리게이트와 인자/반환형이 동일한 함수여야 한다.
        ButtonPressed(ClickTest);

        //내부적으로는 다음과 같이 동작한다. (델리데이트 선언 후 이를 인자로 넣어 함수 실행)
        OnClicked clicked = new OnClicked(ClickTest);
        ButtonPressed(clicked);

        //델리게이트를 변수로 선언하면 추가적으로 실행할 함수를 더하거나 뺄 수 있다.
        clicked += ClickTest2;
        clicked -= ClickTest;
        ButtonPressed(clicked); //단, clicked가 완전히 비어 있으면 Null Reference 에러가 뜬다.

        clicked -= ClickTest2;
        clicked += ClickTest;
        clicked += ClickTest;

        clicked(); //델리게이트를 함수처럼 쓰면 델리게이트 내의 함수들을 발동시킨다.
    }

    // 단점:  델리게이트는 함수의 직접 호출을 막기 위한 장치지만, 델리게이트 자체는 다른 곳에서 쓸 수 있게 public으로 선언되어 있다.
    //        그래서 어디서나 가져다 쓸 수 있어 마찬가지로 코드가 꼬이기 쉽다.  

    //-------------------------------------------------------------------------




    //2. 이벤트 (Event)

    //외부에서 함수를 더하거나 뺄 수는 있지만, 실행은 선언된 곳 내부에서만 가능한 형식.
    //델리게이트를 한번 더 매핑(mapping)한 방식이다.

    [Button]
    void Test02()
    {
        C13_InputManager inputManager = new C13_InputManager();

        //inputManager.Inputkey(); //이벤트를 외부에서 직접 호출하는건 불가능.
        inputManager.Inputkey += InputTest; //이벤트가 호출되면 실행할 함수를 추가해준다.

        inputManager.Event();
    }

    void InputTest()
    {
        Debug.Log("이벤트 발동");
    }
}

class C13_InputManager
{
    public event Action Inputkey; //Action은 델리게이트를 담은 형식. 반환형이 없는 이벤트의 이름을 Action으로 선언할 수 있다. 인자가 있는 경우 <int, float> 와 같이 입력한다. using System 필요.
    //반환형이 있거나 Action 을 사용하지 않을 경우 델리게이트를 먼저 선언하고, 선언된 이름으로 event를 선언해야 한다.

    public void Event()
    {
        Inputkey();
    }
}