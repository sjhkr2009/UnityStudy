using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Define;

public class PlayerController : BaseUnitController
{
    PlayerStat _stat;
    [SerializeField, ReadOnly] float currentSpeed = 0f;

    Vector3 _prevpos;

    protected override void Init()
    {
        base.Init();
        ObjectType = ObjectType.Player;

        _stat = (PlayerStat)MyStat;

        GameManager.Input.MouseAction -= OnMouseEvent;
        GameManager.Input.MouseAction += OnMouseEvent;

        GameManager.UI.MakeWorldSpace<UI_HPBar>(transform);

        State = CreatureState.Idle;
    }



    protected override void UpdateOnIdle()
	{
        if (_lockOnTarget != null && Vector3.Magnitude(_lockOnTarget.transform.position - transform.position) < _stat.AttackRange)
            State = CreatureState.Skill;
    }
    protected override void UpdateOnMoving()
    {
        base.UpdateOnMoving();

        if (_lockOnTarget != null)
        {
            _destPos = _lockOnTarget.transform.position;
            if (Vector3.Magnitude(_destPos - transform.position) <= MyStat.AttackRange)
            {
                State = CreatureState.Skill;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;

        float moveDist = Mathf.Clamp(Delta(_stat.MoveSpeed), 0f, dir.magnitude);
        transform.position += dir.normalized * moveDist;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 13f);

        currentSpeed = Mathf.Max(0.01f, Vector3.Distance(_prevpos, transform.position));
        _prevpos = transform.position;

        if ((dir.magnitude < 0.01f) ||
            (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1f, LayerMask.GetMask("Block")) && !Input.GetMouseButton(0)) ||
            ((MoveTime < 1f) && (currentSpeed < Delta(_stat.MoveSpeed / 8f)) && !Input.GetMouseButton(0)))
        {
            State = CreatureState.Idle;
            return;
        }

        anim.SetFloat("speed", currentSpeed);
    }

    protected override void UpdateOnSkill()
	{
        base.UpdateOnSkill();
    }
    protected override void UpdateOnDie() { }



    int _mask = (1 << (int)Layer.Ground) | (1 << (int)Layer.Monster);
    //bool _stopSkill = false;
    void OnMouseEvent(MouseEvent evt)
	{
        if (State == CreatureState.Die)
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 30f, _mask);

        if (!raycastHit && _lockOnTarget == null)
            return;

        switch (evt)
		{
			case MouseEvent.Press:
                _destPos = (_lockOnTarget == null) ? hit.point : _lockOnTarget.transform.position;
				break;
			case MouseEvent.PointerDown:
                if (raycastHit)
                    OnPointerDown(hit);
                break;
			case MouseEvent.PointerUp:
                //if (State == CreatureState.Skill) _stopSkill = true;
                break;
			case MouseEvent.Click:
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
            case (int)Layer.Ground:
                _lockOnTarget = null;
                State = CreatureState.Moving;
                break;

            case (int)Layer.Monster:
                _lockOnTarget = hit.collider.gameObject;
                if (State == CreatureState.Skill && (Vector3.Magnitude(_destPos - transform.position) <= _stat.AttackRange))
                    return;
                
                State = CreatureState.Moving;
                break;

            default:
                _lockOnTarget = null;
                break;
        }
    }

    void OnHitEvent()
	{
        StartCoroutine(StateSkillToIdle(0.3f));

        if(_lockOnTarget == null)
		{
            State = CreatureState.Idle;
            return;
        }

        Stat targetStat = _lockOnTarget.GetComponent<Stat>();
        targetStat.OnDamaged(MyStat);

        if (targetStat.Hp <= 0)
            GameManager.Game.Despawn(targetStat.gameObject);
    }

    // temp
	private void OnDisable()
	{
        Invoke(nameof(ReSpawn), 3f);
        State = CreatureState.Die;
    }
    void ReSpawn()
	{
        if (gameObject.activeSelf)
            return;

        GameManager.Pool.Pop(gameObject);
        transform.position = Vector3.zero;
        _stat.Hp = _stat.MaxHp;
        State = CreatureState.Idle;
    }
}
