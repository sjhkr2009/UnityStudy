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
    [SerializeField, ReadOnly] GameObject _lockOnTarget;

    Animator anim;
    NavMeshAgent nav;

    PlayerState _state = PlayerState.Idle;
    [ShowInInspector, ReadOnly]
    public PlayerState State
	{
        get => _state;
		set
		{
            _state = value;

			switch (_state)
			{
				case PlayerState.Die:
                    break;
				case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
				case PlayerState.Moving:
                    _prevpos = transform.position;
                    anim.CrossFade("WALK", 0.05f);
                    break;
				case PlayerState.Skill:
                    // TimeOffset을 인자로 받는 버전을 사용하면 Loop 체크가 되어 있지 않아도 애니메이션을 반복할 수 있다. (지정한 프레임으로 돌아가서 반복)
                    anim.CrossFade("ATTACK", 0.1f);
                    break;
				default:
					break;
			}
		}
	}

    public enum PlayerState
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

        GameManager.Input.MouseAction -= OnMouseEvent;
        GameManager.Input.MouseAction += OnMouseEvent;

        State = PlayerState.Idle;
    }

	private void Update()
	{
		switch (State)
		{
			case PlayerState.Die:
                UpdateOnDie();
                break;
			case PlayerState.Idle:
                UpdateOnIdle();
                break;
			case PlayerState.Moving:
                UpdateOnMoving();
                break;
            case PlayerState.Skill:
                UpdateOnSkill();
                break;
            default:
				break;
		}
    }

    void UpdateOnIdle()
	{
        if (_lockOnTarget != null && Vector3.Magnitude(_lockOnTarget.transform.position - transform.position) < 1.5f)
            State = PlayerState.Skill;
    }
    void UpdateOnDie()
    {

    }
    void UpdateOnMoving()
    {
        if ((_lockOnTarget != null) && (Vector3.Magnitude(_destPos - transform.position) <= 1.5f))
        {
            State = PlayerState.Skill;
            return;
        }
        
        Vector3 dir = _destPos - transform.position;

        float moveDist = Mathf.Clamp(Delta(_stat.MoveSpeed), 0f, dir.magnitude);
        nav.Move(dir.normalized * moveDist);
        //transform.position += dir.normalized * moveDist;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 13f);

        currentSpeed = Mathf.Max(0.01f, Vector3.Distance(_prevpos, transform.position));

        if ((dir.magnitude < 0.01f) ||
            (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1f, LayerMask.GetMask("Block")) && !Input.GetMouseButton(0)) ||
            (currentSpeed < Delta(_stat.MoveSpeed / 8f) && !Input.GetMouseButton(0)))
        {
            State = PlayerState.Idle;
            return;
        }

        _prevpos = transform.position;
        anim.SetFloat("speed", currentSpeed);
    }

    void UpdateOnSkill()
	{
        Quaternion lookDir = Quaternion.LookRotation(_lockOnTarget.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookDir, Time.deltaTime * 20f);
    }

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    bool _stopSkill = false;
    void OnMouseEvent(Define.MouseEvent evt)
	{
        if (State == PlayerState.Die)
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 30f, _mask);

        if (!raycastHit && _lockOnTarget == null)
            return;

        switch (evt)
		{
			case Define.MouseEvent.Press:
                _destPos = (_lockOnTarget == null) ? hit.point : _lockOnTarget.transform.position;
				break;
			case Define.MouseEvent.PointerDown:
                if (raycastHit)
                    OnPointerDown(hit);
                break;
			case Define.MouseEvent.PointerUp:
                if (State == PlayerState.Skill)
                    _stopSkill = true;
                break;
			case Define.MouseEvent.Click:
				break;
			default:
				break;
		}
    }
    void OnPointerDown(RaycastHit hit)
	{
        _destPos = hit.point;

        switch (hit.collider.gameObject.layer)
        {
            case (int)Define.Layer.Ground:
                _lockOnTarget = null;
                State = PlayerState.Moving;
                break;

            case (int)Define.Layer.Monster:
                _lockOnTarget = hit.collider.gameObject;
                if (State == PlayerState.Skill && (Vector3.Magnitude(_destPos - transform.position) <= 1.5f))
                    return;
                
                State = PlayerState.Moving;
                break;

            default:
                _lockOnTarget = null;
                break;
        }
    }

    void OnHitEvent()
	{
        StartCoroutine(StateSkillToIdle(0.3f));
    }
    IEnumerator StateSkillToIdle(float delayTime)
	{
        yield return new WaitForSeconds(delayTime);

        if (State == PlayerState.Skill)
            State = PlayerState.Idle;
    }
}
