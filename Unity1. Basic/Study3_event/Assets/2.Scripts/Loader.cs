using System; //Action, Func 사용을 위해 필요
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Loader : MonoBehaviour
{
    //델리게이트: 함수를 저장할 수 있는 변수
    
    public delegate void TestAction();
    public delegate void TestActionInt(int n);
    public delegate float TestActionIntFloat(int n);


    //Action: 미리 C#에 선언된 델리게이트. <> 내에 인자를 넣을 수 있다. 반환형은 void.
    Action _action;
    TestAction _testAction;

    public Action<int> _actionInt;
    public TestActionInt _testActionInt;

    //Func: 미리 C#에 선언된 델리게이트. <> 내에 인자와 반환형을 넣을 수 있다. 마지막 항목이 반환형이 된다.
    Func<int, float> _actIntFloat;
    TestActionIntFloat _testActIntFloat;



    //델리게이트는 대입이 가능 -> 함수 3개 저장해놨는데 다른 함수를 +=가 아니라 =로 대입해버리면, 저장되어 있던 함수들은 덮어씌워져 날아간다.
    //이를 보완하기 위해 이벤트를 사용한다.

    public event Action<int> EventAction; //델리게이트와 동일하게 선언하되 접근 한정자 뒤에 event를 붙인다.
                                          //주로 외부에서 호출되므로 public으로 선언

    //델리게이트와 달리 외부 스크립트에서 대입이 불가능하다.
    //단, 추가한 함수는 OnDestroy될 때 추가한 스크립트에서 지워줘야 한다.
    //그렇지 않으면 해당 오브젝트가 파괴되었는데도 계속 참조하고 있어서, 가비지 컬렉터가 지우지 않는 쓰레기 데이터가 된다. 이것이 쌓이면 게임속도가 점점 느려진다.

    private void OnDestroy()
    {
        EventAction = null; //외부 스크립트가 아닌 이곳에서는 대입 가능. 파괴 시 비워준다.
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }
    
}
