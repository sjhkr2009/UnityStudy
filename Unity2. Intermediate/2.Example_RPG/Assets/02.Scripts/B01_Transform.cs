using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B01_Transform : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] bool rotateOnMove = false;

    void Update()
    {
        Move();
        Rotate();
        if(rotateOnMove) RotateToMove();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (!rotateOnMove) transform.Translate(Vector3.forward * Time.deltaTime * speed);
            else transform.position += Vector3.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (!rotateOnMove) transform.position += transform.TransformDirection(Vector3.back * Time.deltaTime * speed);
            else transform.position += Vector3.back * Time.deltaTime * speed;
        }
        // 월드 좌표계가 아닌, 캐릭터의 정면을 향해 움직여야 한다.
        // transform.TransformDirection(벡터 값)을 통해 월드 좌표를 캐릭터 기준 로컬 좌표로 변환할 수 있다. 반대로 캐릭터 기준 좌표를 월드 좌표로 변환하려면 InverseTransformDirection을 사용하면 된다.
        // Translate로 이동 시 이는 기본 적용되어 있다.

        if (Input.GetKey(KeyCode.A))
        {
            if (!rotateOnMove) transform.Translate(Vector3.left * Time.deltaTime * speed);
            else transform.position += Vector3.left * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!rotateOnMove) transform.Translate(Vector3.right * Time.deltaTime * speed);
            else transform.position += Vector3.right * Time.deltaTime * speed;
        }
        // 단, 이동 시 플레이어가 해당 방향을 보게 한다면, 이동을 월드 좌표 기준으로 하도록 해야 한다.
        // 로컬 기준으로 90도 오른쪽을 보게 회전시키고, 그 상태에서 로컬 기준으로 90도 오른쪽으로 이동할 경우 결과적으로 처음 위치에서 뒤로 이동하는 것이 되기 때문이다.
    }

    void Rotate()
    {
        if(Input.GetKeyDown(KeyCode.R))
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        // transform.eulerAngles을 통해 rotation의 벡터값을 가져오거나 대입할 수 있다. 단, transform.eulerAngles의 값을 식에 넣어 연산하는 것은 권장되지 않는다. (대입만 권장)

        if (Input.GetKeyDown(KeyCode.Q))
            transform.rotation = Quaternion.Euler(new Vector3(0f, (transform.eulerAngles.y + Time.deltaTime * speed), 0f));
        // Quaternion.Euler(벡터 값)을 통해 Vector3 값을 rotation의 기본 형태인 쿼터니언으로 변환할 수 있다. 쿼터니언은 transform.rotation에 대입이 가능하다.

        if (Input.GetKeyDown(KeyCode.E))
            transform.Rotate(new Vector3(0f, -(Time.deltaTime * speed), 0f));
        //일반적인 회전에는 Rotate()를 사용한다.
    }

    /// <summary>
    /// 이동 시 이동하는 방향을 쳐다보게 하기
    /// </summary>
    void RotateToMove()
    {
        if (Input.GetKey(KeyCode.W))
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        if (Input.GetKey(KeyCode.S))
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        // Quaternion.LookRotation(벡터 값)을 통해 특정 방향을 쳐다보는 쿼터니언 값을 구할 수 있다.
        // 캐릭터가 바라볼 방향을 벡터를 의미하며, 해당 지점을 보는 게 아니다. 즉 캐릭터가 어느 위치에 있는가와 무관하게 Vector3.forward는 앞을 바라본다.

        if (Input.GetKey(KeyCode.A))
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.1f);
        if (Input.GetKey(KeyCode.D))
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
        // Quaternion.Slerp(시작 값, 끝 값, 보간 정도)를 통해 중간값으로 회전하게 할 수 있다. 보간 정도를 0~1 사이로 입력하여 특정 중간 지점이 산출된다. 0이면 시작값, 1이면 끝값을 대입한다.
        // 매 프레임마다 실행할 경우 부드러운 이동이 가능하다. 보간 정도에 Time.deltatime을 이용할수도 있다.
    }
}
