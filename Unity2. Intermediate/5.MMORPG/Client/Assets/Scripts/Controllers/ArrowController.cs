using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class ArrowController : BaseController {
    protected override Vector3 Offset => new Vector3(0.5f, -0.3f);
    
    public override void SetDirection(MoveDir direction) {
        base.SetDirection(direction);
        
        spriteRenderer.flipY = false;
        spriteRenderer.flipX = false;
        transform.rotation = Quaternion.identity;
        
        switch (CurrentDir) {
            case MoveDir.Up:
                break;
            case MoveDir.Down:
                spriteRenderer.flipY = true;
                break;
            case MoveDir.Right:
                transform.rotation = Quaternion.Euler(Vector3.forward * -90f);
                break;
            case MoveDir.Left:
                transform.rotation = Quaternion.Euler(Vector3.forward * 90f);
                break;
        }
    }

    protected override void UpdateAnimation() { }

    protected override void UpdateOnIdle() {
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
        
        // override: 충돌 시 이벤트 전달 및 파괴처리
        bool canGoArea = Director.Map.CanGo(CellPos + deltaPos);
        var frontObj = Director.Object.Find(CellPos + deltaPos);
        bool hasNoObject = ReferenceEquals(frontObj, null);

        if (!hasNoObject) {
            Debug.Log($"Arrow Attacked : {frontObj.name}");
            var monster = frontObj.GetComponent<MonsterController>();
            if (monster) monster.OnDamaged();
        }

        if (!canGoArea || !hasNoObject) {
            Director.Resource.Destroy(gameObject);
            return;
        }

        CellPos += deltaPos;
    }
}
