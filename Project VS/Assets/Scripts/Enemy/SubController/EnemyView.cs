using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyView : IEnemyView {
    private SpriteRenderer SpriteRenderer { get; }
    private Animator Animator { get; }
    private EnemyStatus Status { get; }
    
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Dead = Animator.StringToHash("Dead");

    public EnemyView(EnemyStatus status) {
        Status = status;
        SpriteRenderer = Status.GameObject.GetComponent<SpriteRenderer>();
        Animator = Status.GameObject.GetComponent<Animator>();
    }

    public void OnCreate() {
        SpriteRenderer.sortingOrder = 2;
        Animator.SetBool(Dead, false);
    }

    public void Update() {
        if (Status.CurrentDirection == Direction.Right) SpriteRenderer.flipX = true;
        else if (Status.CurrentDirection == Direction.Left) SpriteRenderer.flipX = false;
    }

    public void OnHit() {
        Animator.SetTrigger(Hit);
    }

    public void OnDead() {
        SpriteRenderer.sortingOrder = 1;
        Animator.SetBool(Dead, true);
        DeadWithAnimation().Forget();
    }

    async UniTaskVoid DeadWithAnimation() {
        await UniTask.Delay(500);
        PoolManager.Abandon(Status.GameObject);
    }
}
