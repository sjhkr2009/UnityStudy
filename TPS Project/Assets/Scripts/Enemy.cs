using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : LivingEntity
{
    private enum State
    {
        Patrol,
        Tracking,
        AttackBegin,
        Attacking
    }
    
    [SerializeField] private State state;
    
    private NavMeshAgent agent;
    private Animator animator;

    public Transform attackRoot;    // 공격 포인트
    public Transform eyeTransform;  // 시야 기준점
    
    private AudioSource audioPlayer;
    public AudioClip hitClip;
    public AudioClip deathClip;
    
    private Renderer skinRenderer;

    public float _patrolSpeed = 3f;  // 평시 속도
    public float _runSpeed = 10f;    // 추적 시 속도

    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    
    public float _damage = 30f;
    public float attackRadius = 2f; // 공격 범위
    //private float attackDistance;
    
    public float fieldOfView = 50f; // 시야 각도
    public float viewDistance = 10f;// 시야 거리
    
    [HideInInspector] public LivingEntity targetEntity;   // 공격 대상
    public LayerMask whatIsTarget;

    // 공격에 맞은 대상을 가져올 배열 (범위 공격이므로 배열 형태)
    private RaycastHit[] hits = new RaycastHit[10];
    // 최근 공격한 대상. 중복 공격 방지를 위해 이 오브젝트들은 현재 프레임에서 데미지 처리를 하지 않음.
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();
    
    private bool HasTarget => targetEntity != null && !targetEntity.IsDead;


#if UNITY_EDITOR

    // 에디터에서 이 오브젝트가 선택된 경우 매 프레임 실행된다.
    private void OnDrawGizmosSelected()
    {
        if (attackRoot == null || eyeTransform == null)
            return;

        // 공격 포인트를 중심으로, 공격범위만큼 붉은 원으로 표시
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(attackRoot.position, attackRadius);

        // 서 있는 방향(y축)을 기준으로, 시야각의 왼쪽 절반만큼 회전한 각도를 생성한다.
        Quaternion leftEyeRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        // 오브젝트 앞쪽 방향에서 해당 각도만큼 회전한 방향 벡터를 구한다.
        // (Quaternion a * Vector3 b (연산자 오버로딩) : b벡터를 a만큼 회전한 벡터값을 반환)
        Vector3 leftRayDirection = leftEyeRotation * transform.forward;

        // Handles: 3D 기즈모를 그리기 위한 클래스
        Handles.color = new Color(1f, 1f, 1f, 0.25f);
        // Handles.DrawSolidArc: 호를 그린다. 호의 중심, 수직 방향(normal,호를 볼 위치), 시작 방향, 각도, 호의 반지름을 인자로 받는다.
        Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
    }
    
#endif
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        skinRenderer = GetComponentInChildren<Renderer>();

        // 공격 포인트를 기준으로 공격 범위 내에 들어올 경우, NavMeshAgent가 이동을 멈추게(=공격을 시작하게) 설정한다.
        Vector3 attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        agent.stoppingDistance = attackRadius + Vector3.Distance(transform.position, attackPivot);

        agent.speed = _patrolSpeed;
    }

    // 스펙 설정. 소환 시 소환을 담당하는 클래스에서 설정한다.
    public void Setup(float health, float damage, float runSpeed, float patrolSpeed, Color skinColor)
    {
        startingHealth = Health = health;
        _damage = damage;
        _runSpeed = runSpeed;
        _patrolSpeed = patrolSpeed;
        skinRenderer.material.color = skinColor;

        agent.speed = _patrolSpeed;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (IsDead)
            return;

        if(state == State.Tracking &&
            Vector3.Distance(targetEntity.transform.position, transform.position) <= attackRadius)
		{
            BeginAttack();
		}

        // NavMeshAgent.desiredVelocity : 컴포넌트가 의도한 이동속도 벡터. 벽에 막혀서 못 움직여도 이동을 시도하고 있다면 해당 속도의 벡터가 들어온다.
        animator.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (IsDead)
            return;

        // 공격 시작 또는 공격 중일 경우, 대상을 향해 Y축을 기준으로 회전시킨다.
        if (state == State.AttackBegin || state == State.Attacking)
		{
            float targetAngleY = Quaternion.LookRotation(targetEntity.transform.position - transform.position).eulerAngles.y;

            targetAngleY = Mathf.SmoothDamp(transform.eulerAngles.y, targetAngleY, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;
		}

        // 공격 중이라면, 공격범위 내 적들을 감지하여 공격 대상일 경우 데미지 처리를 한다.
        if(state == State.Attacking)
		{
            Vector3 dir = transform.forward;
            float deltaDistance = agent.velocity.magnitude * Time.deltaTime;

            // Physics.OverlapSphere는 해당 범위가 너무 빨리 움직일 경우 이동 경로에 있는 오브젝트가 감지되지 않는 경우가 있다.
            // SphereCast 사용 시 시작 지점과 끝 방향을 입력하여, 해당 경로로 구(sphere)가 이동한다고 가정하여 경로상의 모든 오브젝트를 감지한다.
            //  * NonAlloc: 감지된 물체의 RaycastHit 배열을 반환하지 않고 우리가 입력해준 배열을 사용한다. 배열에 입력된 원소 개수를 반환한다.
            //      ㄴ 매 프레임 배열을 새로 만드는 작업을 하지 않아도 되어 메모리에 이점이 있다. 참조 타입인 배열을 입력하므로 ref는 필요없음.
            int hitCount = Physics.SphereCastNonAlloc(attackRoot.position, attackRadius, dir, hits, deltaDistance, whatIsTarget);

			for (int i = 0; i < hitCount; i++)
			{
                LivingEntity hitEntity = hits[i].collider.GetComponent<LivingEntity>();

                if(hitEntity != null && !lastAttackedTargets.Contains(hitEntity))
				{
                    DamageMessage dmg = new DamageMessage();
                    dmg.amount = _damage;
                    dmg.attacker = gameObject;
                    dmg.hitNormal = hits[i].normal;

                    // SphereCast가 시작되는 프레임에서 바로 겹쳐있는 대상이 있다면, 거리 및 위치값이 0으로 나온다.
                    // 따라서 이런 경우 타격 지점은 0이 아니라 이 오브젝트의 공격 지점으로 설정해준다.
                    dmg.hitPoint = (hits[i].distance <= 0f) ? attackRoot.position : hits[i].point;

                    hitEntity.ApplyDamage(dmg);
                    lastAttackedTargets.Add(hitEntity);
                    break;
				}
			}
		}
    }

    private IEnumerator UpdatePath()
    {
        while (!IsDead)
        {
            if (HasTarget)
            {
                if(state == State.Patrol)
				{
                    state = State.Tracking;
                    agent.speed = _runSpeed;
				}
                
                agent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                if (targetEntity != null)
                    targetEntity = null;

                if(state != State.Patrol)
				{
                    state = State.Patrol;
                    agent.speed = _patrolSpeed;
				}
                //NavMeshAgent.remainingDistance : 목적지까지 남은 거리
                if (agent.remainingDistance < 1f)
				{
                    Vector3 patrolTargetPos = Utility.GetRandomPointOnNavMesh(transform.position, viewDistance * 2f);
                    agent.SetDestination(patrolTargetPos);
                }

                // 시야 범위 내 모든 적 레이어의 오브젝트를 가져와서, 시야각 내에 적이 있다면 목표로 설정한다.
                Collider[] colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);
				foreach (Collider col in colliders)
				{
                    if (!IsTargetOnSight(col.transform))
                        continue;

                    LivingEntity enemy = col.GetComponent<LivingEntity>();
                    if(enemy != null && !enemy.IsDead)
					{
                        targetEntity = enemy;
                        break;
					}
				}
            }
            
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage))
            return false;

        if(targetEntity == null)
            targetEntity = damageMessage.attacker.GetComponent<LivingEntity>();

        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        audioPlayer.PlayOneShot(hitClip);

        return true;
    }

    public void BeginAttack()
    {
        state = State.AttackBegin;

        agent.isStopped = true;
        animator.SetTrigger("Attack");
    }

    // Animation Event
    public void EnableAttack()
    {
        state = State.Attacking;
        
        lastAttackedTargets.Clear();
    }
    // Animation Event
    public void DisableAttack()
    {
        state = HasTarget ? State.Tracking : State.Patrol;
        
        agent.isStopped = false;
    }

    private bool IsTargetOnSight(Transform target)
    {
        Vector3 dir = target.position - eyeTransform.position;

        // 대상의 방향이 시야각 밖에 있다면(= y축을 고려하지 않은 대상 위치와 눈 위치의 각도가 시야각의 절반을 초과하면) false 반환한다.
        if (Vector3.Angle(new Vector3(dir.x, eyeTransform.forward.y, dir.z), eyeTransform.forward) > fieldOfView * 0.5f)
            return false;

        // 대상 방향으로 시야 거리만큼 Ray를 쏴서, 감지된 대상이 target과 일치하면 false를 반환한다.
        // 감지된 대상이 없으면 false를 반환한다.
        RaycastHit hit;
        if (Physics.Raycast(eyeTransform.position, dir, out hit, viewDistance, whatIsTarget))
            return (hit.transform == target);
        else
            return false;
    }
    
    public override void Die()
    {
        base.Die();

        GetComponent<Collider>().enabled = false;

        // agent.isStopped = true 로 처리해도 이동하지는 않지만, 다른 적들이 이 오브젝트를 피해 가며 이동하려 하는 문제점이 있다.
        agent.enabled = false;

        animator.applyRootMotion = true;
        animator.SetTrigger("Die");

        audioPlayer.PlayOneShot(deathClip);
    }
}