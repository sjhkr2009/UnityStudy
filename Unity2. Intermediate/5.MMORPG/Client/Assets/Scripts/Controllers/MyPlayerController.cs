using System.Collections;
using UnityEngine;
using Google.Protobuf.Protocol;

public class MyPlayerController : PlayerController {
    private WaitForSeconds globalSkillCooldown = new WaitForSeconds(0.1f);
    private bool isSkillGlobalCooldown = false; 
    
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
        
        if (CurrentDir != MoveDir.None) {
            State = CreatureState.Moving;
        }
    }
    
    void UpdateBehaviorInput() {
        // TODO: 임시로 현재 테스트중인 스킬이 나가게 한다. 추후 입력 타입에 대해 수정 필요.
        if (!isSkillGlobalCooldown && Input.GetKey(KeyCode.Space)) {
            
            C_Skill skill = new C_Skill() { Info = new SkillInfo() };
            skill.Info.SkillId = 1;
            Director.Network.Send(skill);
            
            // 잦은 패킷전송 방지를 위해 일정 시간 인풋을 막는다.
            StartCoroutine(nameof(GlobalSkillCoolTime));
        }
    }

    IEnumerator GlobalSkillCoolTime() {
        isSkillGlobalCooldown = true;
        yield return globalSkillCooldown;
        isSkillGlobalCooldown = false;

    }

    protected override void MoveToNextPos() {
        Vector3Int deltaPos = GetDeltaPos(CurrentDir);
        if (deltaPos == Vector3Int.zero) {
            State = CreatureState.Idle;
        } else {
            State = CreatureState.Moving;
            
            // 갈 수 없는 영역이거나 다른 오브젝트가 있다면 이동 불가
            if (Director.Map.CanGo(CellPos + deltaPos) == false) return;
            if (ReferenceEquals(Director.Object.Find(CellPos + deltaPos), null) == false) return;

            CellPos += deltaPos;
        }

        SendPacketIfDirty();
    }

    protected override void SendPacketIfDirty() {
        if (!IsDirty) return;
        
        C_Move movePacket = new C_Move() {
            PosInfo = PositionInfo
        };
        Director.Network.Send(movePacket);
        IsDirty = false;
    }
}