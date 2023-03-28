using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator {
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private SpriteRenderer SpriteRenderer { get; }
    private Animator Animator { get; }
    
    public PlayerAnimator(GameObject target) {
        SpriteRenderer = target.GetOrAddComponent<SpriteRenderer>();
        Animator = target.GetOrAddComponent<Animator>();
    }

    public void Render(PlayerStatus status) {
        if (status.inputVector == Vector2.zero) return;

        SpriteRenderer.flipX = status.inputVector.x < 0;
    }

    public void UpdateAnimator(PlayerStatus status) {
        if (status.isDead) {
            Animator.SetTrigger(Dead);
            return;
        }
        
        Animator.SetFloat(Speed, status.deltaMove.magnitude);
    }
}
