using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    //public event Action<int> ;

    Loader _loader;

    private void Start()
    {
        _loader = FindObjectOfType<Loader>();

        _loader.EventAction += Wow;

        _loader._actionInt += Wow;

        _loader._actionInt = ABC; //델리게이트는 외부에서 대입 가능
        //_loader.EventAction = ABC; //이벤트는 불가

    }

    private void OnDestroy() //파괴될 때 여기서 추가한 함수들은 델리게이트/이벤트에서 빼 준다.
    {
        _loader._actionInt -= ABC;
        _loader.EventAction -= Wow;
    }

    void Wow(int n)
    {

    }

    void ABC(int n)
    {

    }
}
