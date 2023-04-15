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

    public void Render(PlayerStatusHandler statusHandler) {
        if (statusHandler.InputVector == Vector2.zero) return;

        SpriteRenderer.flipX = statusHandler.InputVector.x < 0;
    }

    public void UpdateAnimator(PlayerStatusHandler statusHandler) {
        if (statusHandler.IsDead) {
            Animator.SetTrigger(Dead);
            return;
        }
        
        Animator.SetFloat(Speed, statusHandler.DeltaMove.magnitude);
    }
}
