using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;
    [SerializeField, ReadOnly] float currentSpeed = 0f;
    private float Delta(float value) => (value * Time.deltaTime);

    Vector3 _destPos;
    Vector3 _prevpos;

    Animator anim;
    NavMeshAgent nav;

    enum CursorType
	{
        None,
        Idle,
        Attack
	}
    CursorType _cursorType = CursorType.None;
    Texture2D cursorArrow;
    Texture2D cursorAttack;
    Texture2D cursorHand;

    [ShowInInspector, ReadOnly] State playerState = State.Idle;
    public enum State
	{
        Die,
        Idle,
        Moving,
        Skill
	}

    void Start()
    {
        _stat = GetComponent<PlayerStat>();
        anim = GetComponent<Animator>();
        nav = gameObject.GetOrAddComponent<NavMeshAgent>();

        GameManager.Input.MouseAction -= OnMouseClick;
        GameManager.Input.MouseAction += OnMouseClick;

        cursorArrow = GameManager.Resource.Load<Texture2D>("Textures/Cursor/Arrow");
        cursorAttack = GameManager.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        cursorHand = GameManager.Resource.Load<Texture2D>("Textures/Cursor/Hand");
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
            case State.Skill:
                UpdateOnSkill();
                break;
            default:
				break;
		}
        anim.SetFloat("speed", currentSpeed);
        UpdateMouseCursor();
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
            float moveDist = Mathf.Clamp(Delta(_stat.MoveSpeed), 0f, dir.magnitude);
            nav.Move(dir.normalized * moveDist);
            //transform.position += dir.normalized * moveDist;

            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1f, LayerMask.GetMask("Block")))
            {
                playerState = State.Idle;
                currentSpeed = 0f;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 15);
        }

        currentSpeed = Vector3.Distance(_prevpos, transform.position);
        if (currentSpeed < Delta(_stat.MoveSpeed / 5f))
		{
            playerState = State.Idle;
            currentSpeed = 0f;
            return;
        }

        _prevpos = transform.position;
    }

    void UpdateOnSkill()
	{

	}

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    void UpdateMouseCursor()
	{
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30f, _mask))
        {
            
            switch (hit.collider.gameObject.layer)
            {
                case (int)Define.Layer.Ground:
                    if (_cursorType != CursorType.Idle)
					{
                        Cursor.SetCursor(cursorHand, new Vector2(cursorArrow.width / 4, 0), CursorMode.Auto);
                        _cursorType = CursorType.Idle;
                    }
                    break;
                case (int)Define.Layer.Monster:
                    if (_cursorType != CursorType.Attack)
					{
                        Cursor.SetCursor(cursorAttack, new Vector2(cursorArrow.width / 5, 0), CursorMode.Auto);
                        _cursorType = CursorType.Attack;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    void OnMouseClick(Define.MouseEvent evt)
	{
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30f, _mask))
        {
			switch (hit.collider.gameObject.layer)
			{
                case (int)Define.Layer.Ground:
                    _destPos = hit.point;
                    _prevpos = transform.position;
                    playerState = State.Moving;
                    break;
                case (int)Define.Layer.Monster:
                    break;
                default:
					break;
			}
		}
    }

    void OnFootTouch()
	{
        Debug.Log("뚜벅뚜벅");
	}
}
