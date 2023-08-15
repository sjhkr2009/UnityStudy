using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyView : IEnemyView {
    private SortingGroup SortingGroup { get; }
    private Transform ViewRoot { get; }
    private Animator Animator { get; }
    private EnemyStatus Status { get; }
    
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Dead = Animator.StringToHash("Dead");

    public EnemyView(EnemyStatus status) {
        Status = status;
        SortingGroup = Status.GameObject.GetComponent<SortingGroup>();
        ViewRoot = Status.GameObject.transform;
        Animator = Status.GameObject.GetComponent<Animator>();
    }

    public void OnCreate() {
        SortingGroup.sortingOrder = 2;
        Animator.SetBool(Dead, false);
    }

    public void OnUpdate(float deltaTime) {
        if (Status.CurrentDirection == Direction.Right) ViewRoot.localScale = Vector3.one;
        else if (Status.CurrentDirection == Direction.Left) ViewRoot.localScale = new Vector3(-1f, 1f, 1f);
    }

    public void OnHit(AbilityBase hitAbility) {
        Animator.SetTrigger(Hit);
    }

    public void OnDead() {
        SortingGroup.sortingOrder = 1;
        Animator.SetBool(Dead, true);
        DeadWithAnimation().Forget();
    }

    async UniTaskVoid DeadWithAnimation() {
        await UniTask.Delay(500);
        PoolManager.Abandon(Status.GameObject);
    }
}
