using System.Collections;
using System.Collections.Generic;
using Define;
using DG.Tweening;
using Google.Protobuf.Protocol;
using UnityEngine;

public class MonsterController : BaseController {
    private Coroutine aiPatrol;
    private Coroutine aiSearch;
    private Coroutine aiAttack;
    
    WaitForSeconds searchInterval = new WaitForSeconds(1f);
    WaitForSeconds attackInterval = new WaitForSeconds(0.5f);

    private Vector3Int destPos;
    private BaseController target;
    private float searchRange = 5f;
    private float skillRange = 5f;

    private bool hasRangedSkill = false;
    
    public override float Speed { get; protected set; } = 2f;

    public override CreatureState State {
        get => PositionInfo.State;
        protected set {
            if (PositionInfo.State == value) return;

            PositionInfo.State = value;
            UpdateAnimation();
            StopAiCoroutines();
        }
    }

    protected override void Init() {
        base.Init();

        hasRangedSkill = Random.value < 0f;
        skillRange = hasRangedSkill ? 7f : 1f;
        attackInterval = new WaitForSeconds(hasRangedSkill ? 1f : 0.7f);
    }

    protected override void UpdateOnIdle() {
        base.UpdateOnIdle();

        if (aiPatrol == null) {
            aiPatrol = StartCoroutine(nameof(AiPatrol));
        }
        
        if (aiSearch == null) {
            aiSearch = StartCoroutine(nameof(AiSearch));
        }
    }

    protected override void MoveToNextPos() {
        if (target) {
            destPos = target.CellPos;
            
            // 타겟과의 거리가 일정 미만이고, X,Y축 중 하나가 동일할 경우 공격
            var dir = (destPos - CellPos);
            if (dir.magnitude <= skillRange && dir.x * dir.y == 0) {
                SetLookDirection(destPos);
                State = CreatureState.Skill;
                aiAttack = StartCoroutine(hasRangedSkill ? nameof(AiRangeAttack) : nameof(AiMeleeAttack));
                return;
            }
        }

        var path = Director.Map.FindPath(CellPos, destPos, true);
        // 길을 못 찾았거나, 대상이 너무 멀어지면 중단
        if (path.Count <= 1 || path.Count > 10) {
            target = null;
            State = CreatureState.Idle;
            return;
        }
        
        SetLookDirection(path[1]);
        if (CurrentDir == MoveDir.None) {
            State = CreatureState.Idle;
            return;
        }

        var deltaPos = GetDeltaPos(CurrentDir);
        var nextPos = CellPos + deltaPos;
        
        if (!Director.Map.CanGo(nextPos) || !ReferenceEquals(Director.Object.Find(nextPos), null)) {
            State = CreatureState.Idle;
            return;
        }

        CellPos = nextPos;
    }

    IEnumerator AiPatrol() {
        int waitSeconds = Random.Range(1, 4);
        yield return new WaitForSeconds(waitSeconds);

        for (int i = 0; i < 10; i++) {
            int xRange = Random.Range(-3, 4);
            int yRange = Random.Range(-3, 4);
            var randPos = CellPos + new Vector3Int(xRange, yRange, 0);

            if (Director.Map.CanGo(randPos) && Director.Object.Find(randPos) == null) {
                destPos = randPos;
                State = CreatureState.Moving;
                yield break;
            }
        }

        State = CreatureState.Idle;
    }

    IEnumerator AiSearch() {
        while (true) {
            yield return searchInterval;

            if (target) continue;

            target = Director.Object.FindIf<PlayerController>(controller => {
                var distance = (controller.CellPos - CellPos).magnitude;
                return distance < searchRange;
            });
        }
    }
    
    IEnumerator AiMeleeAttack() {
        var attackTarget = Director.Object.Find<BaseController>(GetFrontCellPos());
        if (attackTarget) {
            attackTarget.OnDamaged();
        }

        yield return attackInterval;
        State = CreatureState.Moving;
    }
    
    IEnumerator AiRangeAttack() {
        var arrow = Director.Resource.Instantiate("Arrow").GetComponent<ArrowController>();
        arrow.SetDirection(LastDir);
        arrow.SetPositionInstant(CellPos);

        yield return attackInterval;
        State = CreatureState.Moving;
    }

    protected override void SetSkillAnimation() {
        base.SetSkillAnimation();

        transform.DOShakePosition(0.3f, 0.5f, 4, 10f);
    }

    void StopAiCoroutines() {
        if (aiPatrol != null) {
            StopCoroutine(aiPatrol);
            aiPatrol = null;
        }
        
        if (aiSearch != null) {
            StopCoroutine(aiSearch);
            aiSearch = null;
        }

        if (aiAttack != null) {
            StopCoroutine(aiAttack);
            aiAttack = null;
        }
    }
}
