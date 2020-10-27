using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField, ReadOnly] float currentSpeed = 0f;
    private float Delta(float value) => (value * Time.deltaTime);

    Vector3 _destPos;
    Vector3 _prevpos;

    Animator anim;

    State playerState = State.Idle;
    public enum State
	{
        Die,
        Idle,
        Moving
	}

    void Start()
    {
        anim = GetComponent<Animator>();

        GameManager.Input.MouseAction -= OnMouseClick;
        GameManager.Input.MouseAction += OnMouseClick;

        // 임시 코드
        GameManager.UI.ShowSceneUI<UI_Inventory>();
    }

	private void Update()
	{
		switch (playerState)
		{
			case State.Die:
                UpdateOnDie();
                break;
			case State.Idle:
                UpdateOnIdle();
                break;
			case State.Moving:
                UpdateOnMoving();
                break;
			default:
				break;
		}
        anim.SetFloat("speed", currentSpeed);
    }

    void UpdateOnIdle()
	{
        
    }
    void UpdateOnDie()
    {

    }
    void UpdateOnMoving()
    {
        Vector3 dir = _destPos - transform.position;

        if (dir.magnitude < 0.01f)
        {
            playerState = State.Idle;
            currentSpeed = 0f;
        }
        else
        {
            float moveDist = Mathf.Clamp(Delta(_speed), 0f, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 15);
        }

        currentSpeed = Vector3.Distance(_prevpos, transform.position);
        if (currentSpeed < Delta(_speed / 5f))
		{
            playerState = State.Idle;
            currentSpeed = 0f;
        }

        _prevpos = transform.position;
    }

    void OnMouseClick(Define.MouseEvent evt)
	{
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30f, LayerMask.GetMask("Plane")))
        {
            _destPos = hit.point;
            _prevpos = transform.position;
            playerState = State.Moving;
        }
    }

    void OnFootTouch()
	{
        Debug.Log("뚜벅뚜벅");
	}
}
