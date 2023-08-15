using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class EnemyMoveComponentBase : MonoBehaviour, IEnemyMoveStrategy {
    public virtual void Initialize(EnemyStatus status) {
        Status = status;
        Transform = status.GameObject.transform;
        Collider = status.GameObject.GetComponent<Collider2D>();
        Rigidbody = status.GameObject.GetComponent<Rigidbody2D>();
    }
    protected EnemyStatus Status { get; set; }
    protected Transform Transform { get; set; }
    protected Rigidbody2D Rigidbody { get; set; }
    protected Collider2D Collider { get; set; }
    protected float Speed => Status.Speed;

    public virtual void OnCreate() {
        Collider.enabled = true;
        Rigidbody.simulated = true;
    }

    public abstract void OnUpdate(float deltaTime);

    public virtual void OnHit(AbilityBase hitAbility) {
        if (hitAbility != null && hitAbility.IgnoreKnockBack) return;
        KnockBack(3f).Forget();
    }

    public virtual void OnDead() {
        Collider.enabled = false;
        Rigidbody.simulated = false;
    }

    public virtual void Stop() {
        Rigidbody.velocity = Vector2.zero;
    }
    
    protected virtual async UniTaskVoid KnockBack(float power) {
        await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);

        Status.IsMovable = false;
        var playerPos = GameManager.Player.transform.position;
        var dir = (Transform.position - playerPos).normalized;
        
        Rigidbody.AddForce(dir * power, ForceMode2D.Impulse);
        await UniTask.Delay(150);
        Status.IsMovable = true;
    }

    public virtual void OnPauseGame() {
        Stop();
    }

    public virtual void OnResumeGame() {
        
    }
}
