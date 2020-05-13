using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Transform myTransform;

    void Start()
    {
        //Rotate(x,y,z): 현재 상태에서 객체의 x,y,z축에 입력한 숫자만큼 회전을 부여한다.
        myTransform.Rotate(60, 60, 60);

        //유니티의 편의 기능으로, Transform은 어느 객체든 가지고 있기 때문에, 자신의 트랜스폼을 불러올 땐 7번째 줄에서처럼 불러오지 않고 그냥 다음과 같이 쓸 수도 있다.
        //자신의 transform에 한정된 예외적 기능.
        //transform.Rotate(60, 60, 60);
    }

    
    void Update()
    {
        //Time.deltaTime: 1프레임이 진행되는 시간값을 가진 변수. 100프레임일 경우 Time.deltaTime = 1/100 이 된다.
        //즉, 1초에 특정 값만큼 진행되길 원하면 해당 값에 Time.deltaTime을 곱하면 된다.
        //이하는 이 오브젝트가 1초마다 x,y,z 축으로 60도씩 회전하게 만드는 코드.
        transform.Rotate(60 * Time.deltaTime, 60 * Time.deltaTime, 60 * Time.deltaTime);
    }
}
