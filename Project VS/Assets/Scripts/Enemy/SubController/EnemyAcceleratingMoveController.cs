using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyAcceleratingMoveController : EnemyMoveComponentBase, ITargetTracker {
    [SerializeField] private float accelRatio = 2f;
    
    private bool IsAccelerating { get; set; }
    public Rigidbody2D Target { get; protected set; }

    public override void Initialize(EnemyStatus status) {
        base.Initialize(status);
        UpdateAcceleratingState().Forget();
    }
    
    private async UniTaskVoid UpdateAcceleratingState() {
        while (!Status.IsDead) {
            IsAccelerating = true;
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            IsAccelerating = false;
            await UniTask.Delay(TimeSpan.FromSeconds(4));
        }
    }

    public override void OnDead() {
        IsAccelerating = false;
    }

    public override void OnUpdate(float deltaTime) {
        if (Status.IsDead) return;
        if (!Status.IsMovable) return;
        
        if (!Target) {
            Debugger.Error("[EnemyAcceleratingMoveController.Update] ITargetTracker.Target is null!!");
            return;
        }

        Vector2 dirVec = Target.position - Rigidbody.position;
        float currentSpeed = IsAccelerating ? Speed * accelRatio : Speed;
        Vector2 deltaVector = dirVec.normalized * (currentSpeed * deltaTime);
        
        Rigidbody.MovePosition(Rigidbody.position + deltaVector);
        Rigidbody.velocity = Vector2.zero; //

        UpdateDirection(Status);
    }
    
    protected void UpdateDirection(EnemyStatus status) {
        if (Target.position.x < Rigidbody.position.x) status.CurrentDirection = Direction.Right;
        else if (Target.position.x > Rigidbody.position.x) status.CurrentDirection = Direction.Left;
    }

    public void SetTarget(Rigidbody2D target) => Target = target;
}
