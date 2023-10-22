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
    public MoveDir CurrentDir { get; set; } = MoveDir.None;
    public MoveDir LastDir { get; protected set; } = MoveDir.None;
    
    private CreatureState _state = CreatureState.Idle;
    public CreatureState State {
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

        if (direction != MoveDir.None)
            LastDir = direction;
    }

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

    protected virtual void UpdateOnIdle() {
        UpdateDestination();
    }

    protected virtual void UpdateOnMoving() {
        UpdatePosition();
    }
    protected virtual void UpdateOnSkill(){}
    protected virtual void UpdateOnDead(){}

    protected void UpdateDestination() {
        if (CurrentDir == MoveDir.None) return;

        Vector3Int deltaPos = Vector3Int.zero;
        switch (CurrentDir) {
            case MoveDir.Up:
                deltaPos = Vector3Int.up;
                break;
            case MoveDir.Down:
                deltaPos = Vector3Int.down;
                break;
            case MoveDir.Right:
                deltaPos = Vector3Int.right;
                break;
            case MoveDir.Left:
                deltaPos = Vector3Int.left;
                break;
            default:
                throw new ArgumentException($"Invalid Position: {CurrentDir}");
        }
        
        State = CreatureState.Moving;
        
        // 갈 수 없는 영역이거나 다른 오브젝트가 있다면 이동 불가
        if (Director.Map.CanGo(CellPos + deltaPos) == false) return;
        if (ReferenceEquals(Director.Object.Find(CellPos + deltaPos), null) == false) return;

        CellPos += deltaPos;
    }

    void UpdatePosition() {
        Vector3 destPos = GridMap.CellToWorld(CellPos) + Offset;
        Vector3 targetVector = destPos - transform.position;
        float moveDistanceInFrame = Speed * Time.deltaTime;
        
        // 움직일 거리가 1프레임에 이동하는 거리보다 짧다면 도착한 것으로 간주한다.
        if (targetVector.magnitude < moveDistanceInFrame) {
            transform.position = destPos;
            State = CreatureState.Idle;
        } else {
            transform.position += targetVector.normalized * moveDistanceInFrame;
        }
    }
}
