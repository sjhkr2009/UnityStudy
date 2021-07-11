using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [ShowInInspector] private Grid gridMap;
    [ShowInInspector] public float Speed { get; private set; } = 5;
    [ShowInInspector, ReadOnly] Vector3Int targetCellPos = Vector3Int.zero;
    
    private bool isMoving = false;
    private MoveDir currentDir = MoveDir.None;
        
    void Start() {
        transform.position = gridMap.CellToWorld(targetCellPos);
    }

    private void Update() {
        UpdateDirInput();
        UpdateTargetPos();
        UpdatePosition();
    }

    void UpdateDirInput()
    {
        if (Input.GetKey(KeyCode.W)) {
            currentDir = MoveDir.Up;
        } else if (Input.GetKey(KeyCode.S)) {
            currentDir = MoveDir.Down;
        } else if (Input.GetKey(KeyCode.D)) {
            currentDir = MoveDir.Right;
        } else if (Input.GetKey(KeyCode.A)) {
            currentDir = MoveDir.Left;
        } else {
            currentDir = MoveDir.None;
        }
    }

    void UpdateTargetPos() {
        if (isMoving || currentDir == MoveDir.None) return;
        
        switch (currentDir) {
            case MoveDir.Up:
                targetCellPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                targetCellPos += Vector3Int.down;
                break;
            case MoveDir.Right:
                targetCellPos += Vector3Int.right;
                break;
            case MoveDir.Left:
                targetCellPos += Vector3Int.left;
                break;
            default:
                throw new ArgumentException($"Invalid Position: {currentDir}");
        }
        isMoving = true;
    }

    void UpdatePosition() {
        if (!isMoving) return;

        Vector3 destPos = gridMap.CellToWorld(targetCellPos);
        Vector3 targetVector = destPos - transform.position;
        float moveDisatanceInFrame = Speed * Time.deltaTime;
        
        // 움직일 거리가 1프레임에 이동하는 거리보다 짧다면 도착한 것으로 간주한다.
        if (targetVector.magnitude < moveDisatanceInFrame) {
            transform.position = destPos;
            isMoving = false;
        }
        else {
            transform.position += targetVector.normalized * moveDisatanceInFrame;
        }
    }
}
