using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;
using Define;

public class MonsterController : BaseUnitController
{
	[SerializeField, ReadOnly] GameObject player;
	protected NavMeshAgent nav;
	Vector3 originPos;

	protected override void Init()
	{
		base.Init();

		if (GetComponentInChildren<UI_HPBar>() == null)
			GameManager.UI.MakeWorldSpace<UI_HPBar>(transform);

		player = GameObject.FindGameObjectWithTag("Player");
		nav = gameObject.GetOrAddComponent<NavMeshAgent>();

		originPos = transform.position;

		//temp
		MyStat.AttackRange = 2.2f;

	}
	
	protected override void UpdateOnIdle()
	{
		// TODO: 매니저가 생기면 따로 처리할 것

		float distance = (player.transform.position - transform.position).magnitude;
		if(distance <= MyStat.AttackRange * 3f)
		{
			_lockOnTarget = player;
			State = Define.CreatureState.Moving;
			return;
		}
	}
	protected override void UpdateOnMoving()
	{
		base.UpdateOnMoving();

		if (_lockOnTarget != null)
		{
			_destPos = _lockOnTarget.transform.position;
			float dist = Vector3.Magnitude(_destPos - transform.position);
			if (dist <= MyStat.AttackRange)
			{
				nav.SetDestination(transform.position);
				State = CreatureState.Skill;
				return;
			}
			else if (dist > MyStat.AttackRange * 3f)
			{
				nav.SetDestination(transform.position);
				State = CreatureState.Idle;
				return;
			}
		}

		nav.SetDestination(_destPos);
		nav.speed = MyStat.MoveSpeed;

		Vector3 dir = _destPos - transform.position;

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 13f);

		if ((dir.magnitude < 0.01f) ||
			(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1f, LayerMask.GetMask("Block")) && !Input.GetMouseButton(0)))
		{
			nav.SetDestination(transform.position);
			State = CreatureState.Idle;
			return;
		}
	}

	protected override void UpdateOnSkill()
	{
		base.UpdateOnSkill();
	}

	protected override void UpdateOnDie()
	{
		
	}

	void OnHitEvent()
	{
		StartCoroutine(StateSkillToIdle(0.5f));

		if (_lockOnTarget == null)
		{
			State = CreatureState.Idle;
			return;
		}

		Stat targetStat = _lockOnTarget.GetComponent<Stat>();
		int damage = Mathf.Max(0, MyStat.Attack - targetStat.Defense);

		targetStat.Hp -= damage;

		if (targetStat.Hp > 0)
		{
			State = (_lockOnTarget.transform.position - transform.position).magnitude <= MyStat.AttackRange ?
				CreatureState.Skill : CreatureState.Moving;
		}
		else
		{
			State = CreatureState.Idle;
		}
	}
}
