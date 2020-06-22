using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover2 : MonoBehaviour
{
    //물체 이동시키기 2

    public Vector3 move = new Vector3(1, 1, 1);
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Move(); //이동 함수를 실행
        }
    }

    void Move()
    {
        transform.Translate(move * Time.deltaTime); //초당 move 만큼 이동

        //기본적으로 물체의 x,y,z축을 기준으로 이동한다.
        //게임세상의 절대적 좌표를 기준으로 이동시키려면 아래와 같이 Space.World 를 붙여준다. (Space.Self 는 기본값이니 있든 없든 똑같음)
        transform.Translate(move * Time.deltaTime, Space.World);


        //Transform은 기본적으로 상대적인 수치를 기록하므로, Transform의 조정 역시 자기 자신을 기준으로 변화한다.
        //참고) 하위 오브젝트가 있다면, 크기나 회전, 위치 모두 상위(부모) 오브젝트 기준으로 상대적인 위치, 크기, 회전이 표기되어 있다.
    }
}
