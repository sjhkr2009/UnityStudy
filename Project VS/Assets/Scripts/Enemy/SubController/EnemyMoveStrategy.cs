using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveStrategy : IEnemyMoveStrategy, ITargetTracker {
    private EnemyStatusHandler StatusHandler { get; }
    private Rigidbody2D Rigidbody { get; }
    public Rigidbody2D Target { get; private set; }
    private float Speed => StatusHandler.Speed;
    
    public EnemyMoveStrategy(EnemyStatusHandler statusHandler) {
        StatusHandler = statusHandler;
        Rigidbody = statusHandler.GameObject.GetComponent<Rigidbody2D>();
    }

    public void Update() {
        if (StatusHandler.IsDead) return;
        if (!Target) {
            Debugger.Error("[EnemyMoveController.Update] ITargetTracker.Target is null!!");
            return;
        }

        Vector2 dirVec = Target.position - Rigidbody.position;
        Vector2 deltaVector = dirVec.normalized * (Speed * Time.deltaTime);
        
        Rigidbody.MovePosition(Rigidbody.position + deltaVector);
        Rigidbody.velocity = Vector2.zero; //

        UpdateDirection(StatusHandler);
    }

    void UpdateDirection(EnemyStatusHandler statusHandler) {
        if (Target.position.x < Rigidbody.position.x) statusHandler.CurrentDirection = Direction.Right;
        else if (Target.position.x > Rigidbody.position.x) statusHandler.CurrentDirection = Direction.Left;
    }

    public void SetTarget(Rigidbody2D target) => Target = target;
}
