using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyMoveStrategy : IEnemyMoveStrategy, ITargetTracker {
    private EnemyStatus Status { get; }
    private Transform Transform { get; }
    private Rigidbody2D Rigidbody { get; }
    private Collider2D Collider { get; }
    public Rigidbody2D Target { get; private set; }
    private float Speed => Status.Speed;
    
    public EnemyMoveStrategy(EnemyStatus status) {
        Status = status;
        Transform = status.GameObject.transform;
        Collider = status.GameObject.GetComponent<Collider2D>();
        Rigidbody = status.GameObject.GetComponent<Rigidbody2D>();
    }

    public void OnCreate() {
        Collider.enabled = true;
        Rigidbody.simulated = true;
    }

    public void OnUpdate(float deltaTime) {
        if (Status.IsDead) return;
        if (!Status.IsMovable) return;
        
        if (!Target) {
            Debugger.Error("[EnemyMoveController.Update] ITargetTracker.Target is null!!");
            return;
        }

        Vector2 dirVec = Target.position - Rigidbody.position;
        Vector2 deltaVector = dirVec.normalized * (Speed * deltaTime);
        
        Rigidbody.MovePosition(Rigidbody.position + deltaVector);
        Rigidbody.velocity = Vector2.zero; //

        UpdateDirection(Status);
    }

    public void OnHit(AbilityBase hitAbility) {
        if (hitAbility != null && hitAbility.IgnoreKnockBack) return;
        KnockBack(3f).Forget();
    }

    public void OnDead() {
        Collider.enabled = false;
        Rigidbody.simulated = false;
    }

    public void Stop() {
        Rigidbody.velocity = Vector2.zero;
    }

    void UpdateDirection(EnemyStatus status) {
        if (Target.position.x < Rigidbody.position.x) status.CurrentDirection = Direction.Right;
        else if (Target.position.x > Rigidbody.position.x) status.CurrentDirection = Direction.Left;
    }

    public void SetTarget(Rigidbody2D target) => Target = target;
    
    async UniTaskVoid KnockBack(float power) {
        await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);

        Status.IsMovable = false;
        var playerPos = GameManager.Player.transform.position;
        var dir = (Transform.position - playerPos).normalized;
        
        Rigidbody.AddForce(dir * power, ForceMode2D.Impulse);
        await UniTask.Delay(150);
        Status.IsMovable = true;
    }

    public void OnPauseGame() {
        Stop();
    }

    public void OnResumeGame() {
        
    }
}
