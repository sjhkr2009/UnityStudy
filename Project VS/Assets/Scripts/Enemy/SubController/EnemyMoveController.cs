using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveController : IEnemyMoveController, ITargetTracker {
    private Rigidbody2D Rigidbody { get; }
    
    public Rigidbody2D Target { get; private set; }
    public float Speed { get; set; }
    
    public EnemyMoveController(GameObject myObject, float speed = 1f) {
        Rigidbody = myObject.GetComponent<Rigidbody2D>();
        Speed = speed;
    }

    public void Update(EnemyStatus status) {
        if (status.IsDead) return;
        if (!Target) {
            Debug.LogError("[EnemyMoveController.Update] ITargetTracker.Target is null!!");
            return;
        }

        Vector2 dirVec = Target.position - Rigidbody.position;
        Vector2 deltaVector = dirVec.normalized * (Speed * Time.deltaTime);
        
        Rigidbody.MovePosition(Rigidbody.position + deltaVector);
        Rigidbody.velocity = Vector2.zero; //

        UpdateDirection(status);
    }

    void UpdateDirection(EnemyStatus status) {
        if (Target.position.x < Rigidbody.position.x) status.CurrentDirection = Direction.Right;
        else if (Target.position.x > Rigidbody.position.x) status.CurrentDirection = Direction.Left;
    }

    public void SetTarget(Rigidbody2D target) => Target = target;
}
