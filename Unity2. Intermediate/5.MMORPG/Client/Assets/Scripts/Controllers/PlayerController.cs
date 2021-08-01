using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

public class PlayerController : MonoBehaviour {
    [BoxGroup("Components"), SerializeField] private Animator animator;
    [BoxGroup("Components"), SerializeField] private SpriteRenderer spriteRenderer;
    [ShowInInspector] public float Speed { get; private set; } = 5;
    [ShowInInspector, ReadOnly] Vector3Int cellPos = Vector3Int.zero;
    
    private Grid GridMap => Director.Map.CurrentGrid;
    private bool isMoving = false;
    private MoveDir currentDir = MoveDir.None;
        
    void Start() {
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = GridMap.CellToWorld(cellPos);
    }

    private void Update() {
        UpdateDirInput();
        UpdateTargetPos();
        UpdatePosition();
    }

    private void LateUpdate() {
        // TODO: 테스트용 코드. 카메라 관리 스크립트 따로 만들 것.
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void UpdateDirInput()
    {
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

    void SetDirection(MoveDir direction) {
        if (currentDir == direction) return;
        
        spriteRenderer.flipX = false;
        switch (direction) {
            case MoveDir.Up:
                animator.Play("walk_back");
                break;
            case MoveDir.Down:
                animator.Play("walk_front");
                break;
            case MoveDir.Right:
                animator.Play("walk_right");
                break;
            case MoveDir.Left:
                animator.Play("walk_right");
                spriteRenderer.flipX = true;
                break;
            case MoveDir.None:
                if (currentDir == MoveDir.Up) animator.Play("idle_back");
                else if (currentDir == MoveDir.Down) animator.Play("idle_front");
                else animator.Play("idle_right");
                
                if (currentDir == MoveDir.Left) spriteRenderer.flipX = true;
                break;
        }
        
        currentDir = direction;
    }

    void UpdateTargetPos() {
        if (isMoving || currentDir == MoveDir.None) return;

        Vector3Int deltaPos = Vector3Int.zero;
        switch (currentDir) {
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
                throw new ArgumentException($"Invalid Position: {currentDir}");
        }

        if (Director.Map.CanGo(cellPos + deltaPos) == false) return;

        cellPos += deltaPos;
        isMoving = true;
    }

    void UpdatePosition() {
        if (!isMoving) return;

        Vector3 destPos = GridMap.CellToWorld(cellPos) + (Vector3.right * 0.5f);
        Vector3 targetVector = destPos - transform.position;
        float moveDistanceInFrame = Speed * Time.deltaTime;
        
        // 움직일 거리가 1프레임에 이동하는 거리보다 짧다면 도착한 것으로 간주한다.
        if (targetVector.magnitude < moveDistanceInFrame) {
            transform.position = destPos;
            isMoving = false;
        }
        else {
            transform.position += targetVector.normalized * moveDistanceInFrame;
        }
    }
}
