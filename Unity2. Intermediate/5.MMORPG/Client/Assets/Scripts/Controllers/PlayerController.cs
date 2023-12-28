using System.Collections;
using Google.Protobuf.Protocol;
using UnityEngine;

public class PlayerController : BaseController {
    protected Coroutine skillRoutine;
    protected WaitForSeconds basicAttackCooldown = new WaitForSeconds(0.3f);
    protected WaitForSeconds arrowAttackCooldown = new WaitForSeconds(0.3f);
    
    protected override void Init() {
        base.Init();
        transform.position = GridMap.CellToWorld(CellPos);
    }

    protected IEnumerator BasicAttack() {
        _rangedSkill = false;
        State = CreatureState.Skill;
        
        // 피격 판정은 서버에 위임 
        
        // 쿨타임은 클라/서버 양쪽에서 체크해야 한다. 서버는 신뢰성을 위해, 클라는 리소스 낭비 및 과도한 패킷 전송 방지를 위해서다.
        yield return basicAttackCooldown;
        State = CreatureState.Idle;
        skillRoutine = null;
        SendPacketIfDirty();
    }
    
    protected IEnumerator ArrowAttack() {
        _rangedSkill = true;
        State = CreatureState.Skill;
        
        // 화살 생성은 서버에서 수행

        yield return arrowAttackCooldown;
        State = CreatureState.Idle;
        skillRoutine = null;
        SendPacketIfDirty();
    }

    protected override void SetIdleAnimation() {
        switch (CurrentDir) {
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

    public void UseSkill(int skillId) {
        if (skillId == 1) {
            skillRoutine = StartCoroutine(nameof(BasicAttack));
        }
        else if (skillId == 2) {
            skillRoutine = StartCoroutine(nameof(ArrowAttack));
        }
    }
    
    protected virtual void SendPacketIfDirty() {
        // temp: 일반 플레이어는 패킷을 받아 연출을 보여주므로, 패킷 전송은 일반적으로 불필요함.
    }
}
