using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BaseController : MonoBehaviour {
    [SerializeField, AutoAssignComponent] protected Animator animator;
    [SerializeField, AutoAssignComponent] protected SpriteRenderer spriteRenderer;
    
    [ShowInInspector] public virtual float Speed { get; protected set; } = 5;
    [ReadOnly] public Vector3Int CellPos { get; set; } = Vector3Int.zero;

    protected virtual Vector3 Offset => Vector3.right * 0.5f;
    protected virtual Grid GridMap => Director.Map.CurrentGrid;
    public MoveDir CurrentDir { get; private set; } = MoveDir.None;
    public MoveDir LastDir { get; private set; } = MoveDir.None;
    
    protected CreatureState _state = CreatureState.Idle;
    public virtual CreatureState State {
        get => _state;
        protected set {
            if (_state == value) return;

            _state = value;
            UpdateAnimation();
        }
    }
        
    void Start() {
        Init();
    }

    protected virtual void Init() { }
    
    protected virtual void Update() {
        UpdateController();
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

        CurrentDir = direction;
        UpdateAnimation();

        if (direction != MoveDir.None)
            LastDir = direction;
    }

    public void SetLookDirection(Vector3Int lookPosition) => SetDirection(GetLookDirection(lookPosition));

    public Vector3Int GetFrontCellPos() {
        var cellPos = CellPos;

        switch (LastDir) {
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
        if (CurrentDir == MoveDir.None) {
            State = CreatureState.Idle;
            return;
        }

        Vector3Int deltaPos = GetDeltaPos(CurrentDir);
        State = CreatureState.Moving;
        
        // 갈 수 없는 영역이거나 다른 오브젝트가 있다면 이동 불가
        if (Director.Map.CanGo(CellPos + deltaPos) == false) return;
        if (ReferenceEquals(Director.Object.Find(CellPos + deltaPos), null) == false) return;

        CellPos += deltaPos;
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
        
        return MoveDir.None;
    }

    protected Vector3Int GetDeltaPos(MoveDir direction) {
        switch (CurrentDir) {
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
            
        Director.Object.Remove(gameObject);
        Director.Resource.Destroy(gameObject);
    }
}
