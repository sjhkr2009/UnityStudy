using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView {
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private Transform RootTr { get; }
    public Transform SpiritTransform { get; }
    public Transform SpiritDestination { get; }
    private Animator Animator { get; }
    private PlayerStatus Status { get; }
    
    public PlayerView(PlayerController.ComponentHolder componentHolder, PlayerStatus status) {
        RootTr = componentHolder.modelTransform;
        Animator = componentHolder.animator;
        SpiritTransform = componentHolder.spiritTransform;
        SpiritDestination = componentHolder.spiritDestination;
        Status = status;
    }

    public void Render() {
        if (Status.InputVector == Vector2.zero) return;

        bool lookRight = Status.InputVector.x > 0;
        
        RootTr.localScale = new Vector3(lookRight ? 1 : -1, 1, 1);
    }

    public void UpdateAnimator() {
        if (Status.IsDead) {
            Animator.SetTrigger(Dead);
            return;
        }
        
        Animator.SetFloat(Speed, Status.DeltaMove.magnitude);
    }
}
