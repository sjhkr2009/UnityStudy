using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Sirenix.OdinInspector;
using UnityEngine;

public class BaseController : MonoBehaviour {
    [BoxGroup("Components"), SerializeField, AutoAssignComponent] protected Animator animator;
    [BoxGroup("Components"), SerializeField, AutoAssignComponent] protected SpriteRenderer spriteRenderer;
    
    [ShowInInspector] public virtual float Speed { get; protected set; } = 5;
    [ReadOnly] public Vector3Int CellDestinationPos { get; protected set; } = Vector3Int.zero;
    
    protected virtual Grid GridMap => Director.Map.CurrentGrid;
    public MoveDir CurrentDir { get; private set; } = MoveDir.None;
    public MoveDir PrevDir { get; private set; } = MoveDir.None;
    
    private CreatureState _state = CreatureState.Idle;
    public CreatureState State {
        get => _state;
        set {
            if (_state == value) return;

            _state = value;
            UpdateAnimation();
        }
    }
        
    void Start() {
        Init();
    }

    protected virtual void Init() {
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPositionInstant(Vector3Int destPos) {
        CellDestinationPos = destPos;
        transform.position = GridMap.CellToWorld(CellDestinationPos) + (Vector3.right * 0.5f);
    }

    protected virtual void UpdateController() {
        UpdateDestination();
        UpdatePosition();
    }

    protected virtual void SetDirection(MoveDir direction) {
        if (CurrentDir == direction) return;

        CurrentDir = direction;
        UpdateAnimation();

        if (direction != MoveDir.None)
            PrevDir = direction;
    }

    protected void UpdateAnimation() {
        switch (State) {
            case CreatureState.Idle:
                UpdateIdleAnimation();
                break;
            case CreatureState.Moving:
                UpdateMovingAnimation();
                break;
            case CreatureState.Skill:
                UpdateSkillAnimation();
                break;
            case CreatureState.Die:
                UpdateDieAnimation();
                break;
        }
    }
    
    protected virtual void UpdateIdleAnimation(){}
    protected virtual void UpdateMovingAnimation(){}
    protected virtual void UpdateSkillAnimation(){}
    protected virtual void UpdateDieAnimation(){}

    void UpdateDestination() {
        if (State != CreatureState.Idle || CurrentDir == MoveDir.None) return;

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
        if (Director.Map.CanGo(CellDestinationPos + deltaPos) == false) return;
        if (ReferenceEquals(Director.Object.Find(CellDestinationPos + deltaPos), null) == false) return;

        CellDestinationPos += deltaPos;
    }

    void UpdatePosition() {
        if (State != CreatureState.Moving) return;

        Vector3 destPos = GridMap.CellToWorld(CellDestinationPos) + (Vector3.right * 0.5f);
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
