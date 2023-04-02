using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView {
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private SpriteRenderer SpriteRenderer { get; }
    private Animator Animator { get; }
    
    public PlayerView(GameObject target) {
        SpriteRenderer = target.GetOrAddComponent<SpriteRenderer>();
        Animator = target.GetOrAddComponent<Animator>();
    }

    public void Render(PlayerStatus status) {
        if (status.InputVector == Vector2.zero) return;

        SpriteRenderer.flipX = status.InputVector.x < 0;
    }

    public void UpdateAnimator(PlayerStatus status) {
        if (status.IsDead) {
            Animator.SetTrigger(Dead);
            return;
        }
        
        Animator.SetFloat(Speed, status.DeltaMove.magnitude);
    }
}
