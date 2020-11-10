using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Define;

public abstract class BaseUnitController : MonoBehaviour
{
    protected float Delta(float value) => (value * Time.deltaTime);
    public ObjectType ObjectType { get; protected set; } = ObjectType.Unknown;
    public float MoveTime { get; private set; }

    public Stat MyStat { get; protected set; }
    protected Vector3 _destPos;
    [SerializeField, ReadOnly] protected GameObject _lockOnTarget;

    protected Animator anim;

    CreatureState _state = CreatureState.Idle;
    [ShowInInspector, ReadOnly]
    public virtual CreatureState State
    {
        get => _state;
        set
        {
            _state = value;

            switch (_state)
            {
                case CreatureState.Die:
                    break;
                case CreatureState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case CreatureState.Moving:
                    anim.CrossFade("WALK", 0.05f);
                    break;
                case CreatureState.Skill:
                    // TimeOffset을 인자로 받는 버전을 사용하면 Loop 체크가 되어 있지 않아도 애니메이션을 반복할 수 있다. (지정한 프레임으로 돌아가서 반복)
                    anim.CrossFade("ATTACK", 0.1f);
                    break;
                default:
                    break;
            }
        }
    }
    private void Start() => Init();
	protected virtual void Init()
	{
        anim = GetComponent<Animator>();
        MyStat = GetComponent<Stat>();
    }

    protected void Update()
    {
        switch (State)
        {
            case CreatureState.Die:
                UpdateOnDie();
                break;
            case CreatureState.Idle:
                UpdateOnIdle();
                break;
            case CreatureState.Moving:
                MoveTime = 0f;
                UpdateOnMoving();
                break;
            case CreatureState.Skill:
                UpdateOnSkill();
                break;
            default:
                break;
        }
    }


    protected virtual void UpdateOnDie() { }
    protected virtual void UpdateOnIdle() { }
    protected virtual void UpdateOnMoving() 
    {
        MoveTime += Time.deltaTime;
    }
    protected virtual void UpdateOnSkill()
	{
        if (_lockOnTarget == null || !_lockOnTarget.IsValid())
        {
            State = CreatureState.Idle;
            return;
        }

        Quaternion lookDir = Quaternion.LookRotation(_lockOnTarget.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookDir, Time.deltaTime * 20f);
    }

    protected IEnumerator StateSkillToIdle(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (State == CreatureState.Skill)
            State = CreatureState.Idle;
    }

}
