using System;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ArrowController : BaseController {
    protected override Vector3 Offset => new Vector3(0.5f, -0.3f);
    public override float Speed { get; protected set; } = 15f;

    public override void SetDirection(MoveDir direction) {
        base.SetDirection(direction);
        
        spriteRenderer.flipY = false;
        spriteRenderer.flipX = false;
        transform.rotation = Quaternion.identity;
        
        switch (CurrentDir) {
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

        State = CreatureState.Moving;
    }

    protected override void UpdateAnimation() { }
}
