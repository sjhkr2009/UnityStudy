using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BaseController : MonoBehaviour {
    [ShowInInspector, ReadOnly] public int Id { get; set; }
    
    [SerializeField, AutoAssignComponent] protected Animator animator;
    [SerializeField, AutoAssignComponent] protected SpriteRenderer spriteRenderer;
    
    [ShowInInspector] public virtual float Speed { get; protected set; } = 5;
    [ShowInInspector] protected bool IsDirty { get; set; } = false;

    [ReadOnly]
    public Vector3Int CellPos {
        get => new Vector3Int(PositionInfo.PosX, PositionInfo.PosY, 0);
        set {
            if (PositionInfo.PosX == value.x && PositionInfo.PosY == value.y) return;
            PositionInfo.PosX = value.x;
            PositionInfo.PosY = value.y;
            IsDirty = true;
        }
    }

    private PositionInfo _positionInfo = new PositionInfo();

    public PositionInfo PositionInfo {
        get => _positionInfo;
        set {
            if (_positionInfo.Equals(value)) return;

            CellPos = new Vector3Int(value.PosX, value.PosY, 0);
            State = value.State;
            SetDirection(value.MoveDir);
        }
    }

    protected virtual Vector3 Offset => Vector3.right * 0.5f;
    protected virtual Grid GridMap => Director.Map.CurrentGrid;

    public MoveDir CurrentDir => PositionInfo.MoveDir;
    
    public virtual CreatureState State {
        get => PositionInfo.State;
        protected set {
            if (PositionInfo.State == value) return;

            PositionInfo.State = value;
            UpdateAnimation();
            IsDirty = true;
        }
    }
        
    void Start() {
        Init();
    }

    protected virtual void Init() {
        // 서버에서 PositionInfo값을 받지 않아도 기본 상태 세팅. (서버에서 디폴트값과 동일한 패킷을 전송하면 클라에서 빈 값으로 인식할 수 있음) 
        State = CreatureState.Idle;
        CellPos = Vector3Int.zero;
        SetDirection(MoveDir.Down);
        UpdateAnimation();
    }
    
    protected virtual void Update() {
        UpdateController();
    }

    public void SyncPositionInfo() {
        transform.position = GridMap.CellToWorld(CellPos) + Offset;
    }

    public virtual void SetPositionInstant(Vector3Int destPos) {
        CellPos = destPos;
        transform.position = GridMap.CellToWorld(CellPos) + Offset;
    }

    protected virtual void UpdateController() {
        switch (State) {
            case CreatureState.Idle:
                UpdateOnIdle();
                break;
            case CreatureState.Moving:
                UpdateOnMoving();
                break;
            case CreatureState.Skill:
                UpdateOnSkill();
                break;
            case CreatureState.Die:
                UpdateOnDead();
                break;
        }
    }

    public virtual void SetDirection(MoveDir direction) {
        if (CurrentDir == direction) return;

        PositionInfo.MoveDir = direction;
        UpdateAnimation();
        
        IsDirty = true;
    }

    public void SetLookDirection(Vector3Int lookPosition) => SetDirection(GetLookDirection(lookPosition));

    public Vector3Int GetFrontCellPos() {
        var cellPos = CellPos;

        switch (CurrentDir) {
            case MoveDir.Up:
                cellPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                cellPos += Vector3Int.down;
                break;
            case MoveDir.Right:
                cellPos += Vector3Int.right;
                break;
            case MoveDir.Left:
                cellPos += Vector3Int.left;
                break;
        }

        return cellPos;
    }

    protected virtual void UpdateAnimation() {
        switch (State) {
            case CreatureState.Idle:
                SetIdleAnimation();
                break;
            case CreatureState.Moving:
                SetMovingAnimation();
                break;
            case CreatureState.Skill:
                SetSkillAnimation();
                break;
            case CreatureState.Die:
                SetDieAnimation();
                break;
        }
    }
    
    protected virtual void SetIdleAnimation(){}
    protected virtual void SetMovingAnimation(){}
    protected virtual void SetSkillAnimation(){}
    protected virtual void SetDieAnimation(){}

    protected virtual void UpdateOnIdle() { }

    protected virtual void UpdateOnMoving() {
        UpdatePosition();
    }
    protected virtual void UpdateOnSkill(){}
    protected virtual void UpdateOnDead(){}

    protected virtual void MoveToNextPos() {
        
    }
    
    protected virtual MoveDir GetLookDirection(Vector3Int lookPosition) {
        var dir = lookPosition - CellPos;
        var distX = Mathf.Abs(lookPosition.x - CellPos.x);
        var distY = Mathf.Abs(lookPosition.y - CellPos.y);

        if (distX > distY) {
            // right or left
            if (dir.x > 0) return MoveDir.Right;
            if (dir.x < 0) return MoveDir.Left;
        } else if (distX < distY) {
            // up or down
            if (dir.y > 0) return MoveDir.Up;
            if (dir.y < 0) return MoveDir.Down;
        }
        
        return MoveDir.Down;
    }

    protected Vector3Int GetDeltaPos(MoveDir direction) {
        switch (direction) {
            case MoveDir.Up: return Vector3Int.up;
            case MoveDir.Down: return Vector3Int.down;
            case MoveDir.Right: return Vector3Int.right;
            case MoveDir.Left: return Vector3Int.left;
            default: return Vector3Int.zero;
        }
    }

    void UpdatePosition() {
        Vector3 destPos = GridMap.CellToWorld(CellPos) + Offset;
        Vector3 targetVector = destPos - transform.position;
        float moveDistanceInFrame = Speed * Time.deltaTime;
        
        // 움직일 거리가 1프레임에 이동하는 거리보다 짧다면 도착한 것으로 간주한다.
        if (targetVector.magnitude < moveDistanceInFrame) {
            transform.position = destPos;
            MoveToNextPos();
        } else {
            transform.position += targetVector.normalized * moveDistanceInFrame;
        }
    }

    // TODO: 공격 및 피격자에 따라 다르게 처리할 수 있도록, 공격/피격 인터페이스 및 데미지 정보 추가할 것
    public virtual void OnDamaged() {
        var eff = Director.Resource.Instantiate("Effect/DieEffect");
        eff.transform.position = transform.position;
        eff.GetComponent<Animator>().Play("default");
        Director.Resource.Destroy(eff, 0.5f);
            
        Director.Object.Remove(Id);
        Director.Resource.Destroy(gameObject);
    }
}
