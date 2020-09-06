using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Z01_PlayerMove : MonoBehaviour
{
    // F02 기반 수정 - NavMesh 추가
    
    public enum PlayerState
    {
        Idle,
        Moving,
        Die
    }

    [SerializeField] PlayerState state = PlayerState.Idle;
    [SerializeField] NavMeshAgent navMashAgent;

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

    Vector3 destPos;
    Quaternion destRot;

    void Start()
    {
        A01_Manager.Input.OnMouseEvent -= SetDestination;
        A01_Manager.Input.OnMouseEvent += SetDestination;

        navMashAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
    }

    void SetDestination(E02_Define.MouseEvent evt)
    {
        if (state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Plane")))
        {
            destPos = hit.point;
            destRot = Quaternion.LookRotation(hit.point - transform.position);
            state = PlayerState.Moving;
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

        navMashAgent.Move(dir.normalized * speed * Time.deltaTime);

        if( Physics.Raycast(transform.position + (Vector3.up * 0.5f), dir, 0.75f, LayerMask.GetMask("Block")))
		{
            state = PlayerState.Idle;
		}
    }

    void RotateToDest(Quaternion _dest)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _dest, 10 * Time.deltaTime);
    }
}
