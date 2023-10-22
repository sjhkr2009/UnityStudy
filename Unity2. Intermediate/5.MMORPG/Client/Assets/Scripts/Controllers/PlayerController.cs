using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

public class PlayerController : BaseController {
    private Coroutine skillRoutine;
    private WaitForSeconds basicAttackCooldown = new WaitForSeconds(0.3f);
    private WaitForSeconds arrowAttackCooldown = new WaitForSeconds(0.3f);
    
    protected override void Init() {
        base.Init();
        transform.position = GridMap.CellToWorld(CellPos);
    }

    protected override void Update() {
        CheckInput();
        UpdateController();
    }

    private void LateUpdate() {
        // TODO: 테스트용 코드. 카메라 관리 스크립트 따로 만들 것.
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void CheckInput() {
        switch (State) {
            case CreatureState.Idle:
                UpdateDirectionInput();
                UpdateBehaviorInput();
                break;
            case CreatureState.Moving:
                UpdateDirectionInput();
                UpdateBehaviorInput();
                break;
            case CreatureState.Skill:
                break;
            case CreatureState.Die:
                break;
        }
    }

    void UpdateDirectionInput() {
        if (Input.GetKey(KeyCode.W)) {
            SetDirection(MoveDir.Up);
        } else if (Input.GetKey(KeyCode.S)) {
            SetDirection(MoveDir.Down);
        } else if (Input.GetKey(KeyCode.D)) {
            SetDirection(MoveDir.Right);
        } else if (Input.GetKey(KeyCode.A)) {
            SetDirection(MoveDir.Left);
        } else {
            SetDirection(MoveDir.None);
        }
    }
    
    void UpdateBehaviorInput() { 
        // TODO: 임시로 현재 테스트중인 스킬이 나가게 한다. 추후 입력 타입에 대해 수정 필요.
        if (Input.GetKey(KeyCode.Space)) {
            skillRoutine = StartCoroutine(nameof(ArrowAttack));
        }
    }

    IEnumerator BasicAttack() {
        _rangedSkill = false;
        State = CreatureState.Skill;
        var obj = Director.Object.Find(GetFrontCellPos());
        if (obj) {
            Debug.Log($"Attack: {obj.name}");
        }
        
        yield return basicAttackCooldown;
        State = CreatureState.Idle;
        skillRoutine = null;
    }
    
    IEnumerator ArrowAttack() {
        _rangedSkill = true;
        State = CreatureState.Skill;
        var arrow = Director.Resource.Instantiate("Arrow").GetComponent<ArrowController>();
        arrow.SetDirection(LastDir);
        arrow.SetPositionInstant(CellPos);

        yield return arrowAttackCooldown;
        State = CreatureState.Idle;
        skillRoutine = null;
    }

    protected override void SetIdleAnimation() {
        switch (LastDir) {
            case MoveDir.Up:
                animator.Play("idle_back");
                break;
            case MoveDir.Down:
                animator.Play("idle_front");
                break;
            case MoveDir.Right:
                animator.Play("idle_right");
                spriteRenderer.flipX = false;
                break;
            case MoveDir.Left:
                animator.Play("idle_right");
                spriteRenderer.flipX = true;
                break;
            default:
                animator.Play("idle_front");
                break;
        }
    }

    protected override void SetMovingAnimation() {
        switch (CurrentDir) {
            case MoveDir.Up:
                animator.Play("walk_back");
                break;
            case MoveDir.Down:
                animator.Play("walk_front");
                break;
            case MoveDir.Right:
                animator.Play("walk_right");
                spriteRenderer.flipX = false;
                break;
            case MoveDir.Left:
                animator.Play("walk_right");
                spriteRenderer.flipX = true;
                break;
        }
    }
    // TODO: 각 스킬의 Coroutine와 애니메이션 정보, 타입 등은 스킬별로 클래스를 만들어 분리
    private bool _rangedSkill = false;
    protected override void SetSkillAnimation() {
        switch (CurrentDir) {
            case MoveDir.Up:
                animator.Play(_rangedSkill ? "attack_weapon_back" : "attack_back");
                break;
            case MoveDir.Down:
                animator.Play(_rangedSkill ? "attack_weapon_front" : "attack_front");
                break;
            case MoveDir.Right:
                animator.Play(_rangedSkill ? "attack_weapon_right" : "attack_right");
                spriteRenderer.flipX = false;
                break;
            case MoveDir.Left:
                animator.Play(_rangedSkill ? "attack_weapon_right" : "attack_right");
                spriteRenderer.flipX = true;
                break;
        }
    }
}
