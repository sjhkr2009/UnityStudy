using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView {
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private SpriteRenderer SpriteRenderer { get; }
    private Animator Animator { get; }
    private PlayerStatus Status { get; }
    
    public PlayerView(PlayerStatus status) {
        var target = status.GameObject;
        SpriteRenderer = target.GetOrAddComponent<SpriteRenderer>();
        Animator = target.GetOrAddComponent<Animator>();
        Status = status;
    }

    public void Render() {
        if (Status.InputVector == Vector2.zero) return;

        SpriteRenderer.flipX = Status.InputVector.x < 0;
    }

    public void UpdateAnimator() {
        if (Status.IsDead) {
            Animator.SetTrigger(Dead);
            return;
        }
        
        Animator.SetFloat(Speed, Status.DeltaMove.magnitude);
    }
}
