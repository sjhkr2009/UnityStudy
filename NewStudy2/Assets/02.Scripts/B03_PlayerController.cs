using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class B03_PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] bool rotateOnMove = false;

    bool hasDest = false;
    Vector3 destPos;
    Quaternion destRot;

    void Start()
    {
        A01_Manager.Input.OnKeyAction -= OnKeyBoard; //혹시 이 함수가 구독되어 있는데 또 추가하면 두 번 추가되므로, 한번 뺀 다음 추가하는게 안전하다.
        A01_Manager.Input.OnMouseEvent -= SetDestination;
        A01_Manager.Input.OnKeyAction += OnKeyBoard;
        A01_Manager.Input.OnMouseEvent += SetDestination;
    }
    private void OnDestroy()
    {
        A01_Manager.Input.OnKeyAction -= OnKeyBoard;
        A01_Manager.Input.OnMouseEvent -= SetDestination;
    }

    void OnKeyBoard()
    {
        if (hasDest) hasDest = false;
        Move();
        Rotate();
        if (rotateOnMove) RotateToMove();
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
    }

    void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.R))
            transform.eulerAngles = new Vector3(0f, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Q))
            transform.rotation = Quaternion.Euler(new Vector3(0f, (transform.eulerAngles.y + Time.deltaTime * speed), 0f));

        if (Input.GetKeyDown(KeyCode.E))
            transform.Rotate(new Vector3(0f, -(Time.deltaTime * speed), 0f));
    }

    /// <summary>
    /// 이동 시 이동하는 방향을 쳐다보게 하기
    /// </summary>
    void RotateToMove()
    {
        if (Input.GetKey(KeyCode.W))
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
        if (Input.GetKey(KeyCode.S))
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.1f);
        // Quaternion.LookRotation(벡터 값)을 통해 특정 방향을 쳐다보는 쿼터니언 값을 구할 수 있다.
        // 캐릭터가 바라볼 방향을 벡터를 의미하며, 해당 지점을 보는 게 아니다. 즉 캐릭터가 어느 위치에 있는가와 무관하게 Vector3.forward는 앞을 바라본다.

        if (Input.GetKey(KeyCode.A))
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.1f);
        if (Input.GetKey(KeyCode.D))
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
        // Quaternion.Slerp(시작 값, 끝 값, 보간 정도)를 통해 중간값으로 회전하게 할 수 있다. 보간 정도를 0~1 사이로 입력하여 특정 중간 지점이 산출된다. 0이면 시작값, 1이면 끝값을 대입한다.
        // 매 프레임마다 실행할 경우 부드러운 이동이 가능하다. 보간 정도에 Time.deltatime을 이용할수도 있다.
    }

    void SetDestination(E02_Define.MouseEvent evt)
    {
        if (evt != E02_Define.MouseEvent.Click) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Plane")))
        {
            destPos = hit.point;
            destRot = Quaternion.LookRotation(hit.point - transform.position);
            hasDest = true;
        }
    }

    void MoveToDest(Vector3 _dest)
    {
        Vector3 dir = _dest - transform.position;
        if (dir.magnitude < speed * Time.deltaTime)
        {
            transform.position = _dest;
            hasDest = false;
            return;
        }

        transform.position += dir.normalized * speed * Time.deltaTime;
        //Translate 사용 시 플레이어 로컬 좌표 기준으로 이동하므로, 월드 좌표 기준 방향인 dir 값은 position 값에 직접 더해서 이동시킨다.
    }

    void RotateToDest(Quaternion _dest)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _dest, 10 * Time.deltaTime);
    }

    private void Update()
    {
        if (hasDest)
        {
            MoveToDest(destPos);
            RotateToDest(destRot);
        }
    }
}
