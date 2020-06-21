using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F02_PlayerController : MonoBehaviour
{
    // State 패턴을 적용하여 플레이어 컨트롤러를 구현한다.
    // 기존에 마우스로 캐릭터를 이동시키던 부분은 B03 스크립트를 가져온다.

    // 플레이어 상태 정리
    public enum PlayerState
    {
        Idle,
        Moving,
        Die
    }

    PlayerState state = PlayerState.Idle;

    // 상태에 따른 Update문을 따로 정의해두고 실행시킨다.
    private void Update()
    {
        switch (state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Die:
                UpdateDie();
                break;
        }
    }

    void UpdateIdle()
    {

    }
    void UpdateMoving()
    {
        MoveToDest(destPos);
        RotateToDest(destRot);
    }
    void UpdateDie()
    {

    }


    [SerializeField] float speed = 5f;

    //bool hasDest = false; //State 패턴 구현 시 이러한 bool 값과 if문으로 플레이어 상태를 체크하지 않아도 된다.
    Vector3 destPos;
    Quaternion destRot;

    void Start()
    {
        A01_Manager.Input.OnMouseEvent -= SetDestination;
        A01_Manager.Input.OnMouseEvent += SetDestination;
    }
    private void OnDestroy()
    {
        A01_Manager.Input.OnMouseEvent -= SetDestination;
    }

    void SetDestination(E02_Define.MouseEvent evt)
    {
        if (state == PlayerState.Die) return; // 플레이어 사망 시 조작 관련 함수를 return 시킨다.
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Plane")))
        {
            destPos = hit.point;
            destRot = Quaternion.LookRotation(hit.point - transform.position);
            state = PlayerState.Moving; //한 상태에서 다른 상태로 이동하기 위한 조건이 필요하다.
        }
    }

    void MoveToDest(Vector3 _dest)
    {
        Vector3 dir = _dest - transform.position;
        if (dir.magnitude < speed * Time.deltaTime)
        {
            transform.position = _dest;
            state = PlayerState.Idle;
            return;
        }

        transform.position += dir.normalized * speed * Time.deltaTime;
    }

    void RotateToDest(Quaternion _dest)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _dest, 10 * Time.deltaTime);
    }
}
