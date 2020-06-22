using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    //포지션 변경은 Global 죄표계를 기준으로 한다.
    void Start()
    {
        transform.position = new Vector3(0, 0, 0); //자신의 위치를 0,0,0 으로 이동.
        //이렇게 해도 유니티에서 Sphere는 절대 좌표를 기준으로 이미 0,0,0에 있으므로 이동하지 않는다.


        //지정된 상대 좌표(즉 유니티에서 표기되는 것)로 이동시키려면 다음과 같이 적어준다. 
        transform.localPosition = new Vector3(0, 0, 0);
        //이렇게 하면 부모 오브젝트 Cube를 기준으로 0,0,0 좌표로 이동한다.
        //트랜스폼의 크기나 회전 등을 조정할 때도 localScale, loaclRotation 을 사용한다.

        //반대로 절대 좌표계(글로벌)를 기준으로 해야 할 때는 lossyPosition, lossyScale 등을 사용하면 된다.
    }
    
}
