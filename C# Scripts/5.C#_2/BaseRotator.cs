using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRotator : MonoBehaviour
{
    //Override: 부모 클래스에서 만든 함수를 자식 클래스에 덮어씌운 후, 자기만의 함수로 만드는 것. 
    //virtual void() 의 형태로 함수를 작성하고, 자식 클래스에서 override void()를 통해 덮어쓰는 방식으로 구현한다.

    //여기서는 오브젝트를 회전시키는 스크립트를 작성한다.
    //회전하는 모든 오브젝트들은 이 스크립트를 상속받아 회전하지만, 자식 클래스에서 회전 방향은 각자 바꿀 수 있게 한다.

    public float speed = 60f;

    //함수 앞에 virtual을 붙일 경우, 자식 클래스에서 덮어씌울 수 있다.
    protected virtual void Rotate() //protected: 자식 클래스에서만 쓸 수 있고, 상속받지 않은 곳에서는 가져다 쓸 수 없다.
    {
        transform.Rotate(speed * Time.deltaTime, 0, 0); //x방향으로 초당 60만큼 회전
    }

    void Update()
    {
        Rotate();
    }
}
