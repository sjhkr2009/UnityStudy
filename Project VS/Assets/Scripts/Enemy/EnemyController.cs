using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class EnemyController : EnemyControllerBase, IRepositionTarget, IAttackableCollider {
    public override void OnInitialize() {
        base.OnInitialize();
        view = new EnemyView(Status);
        moveStrategy = new EnemyMoveStrategy(Status);
        
        view.OnCreate();
        moveStrategy.OnCreate();
        
        SetTarget();
    }

    void SetTarget() {
        if (moveStrategy is ITargetTracker tracker) {
            tracker.SetTarget(GameManager.Player.GetComponent<Rigidbody2D>());
        }
    }

    public virtual void Reposition(Transform pivotTransform) {
        if (Status.IsDead) return;
        
        var playerDir = GameManager.Player.Status.InputVector;
        
        var randomVector = CustomUtility.GetRandomVector(-3f, 3f);
        var moveDelta = (playerDir * Define.EnvironmentSetting.TileMapSize) + randomVector;
        
        transform.Translate(moveDelta);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (isPaused || Status.IsDead) return;
        
        var weapon = other.GetComponent<IAttackableCollider>();
        if (weapon == null || !weapon.IsValidTarget(gameObject)) return;

        Status.Hp -= weapon.Damage;
        
        if (Status.Hp <= 0) {
            Dead();
        } else {
            view.OnHit();
            moveStrategy.OnHit();
        }
    }

    public override void OnPauseGame() {
        base.OnPauseGame();
        moveStrategy?.OnPauseGame();
    }

    public override void OnResumeGame() {
        base.OnResumeGame();
        moveStrategy?.OnResumeGame();
    }

    void Dead() {
        Status.IsDead = true;
        view.OnDead();
        moveStrategy.OnDead();
        GameManager.Controller?.CallEnemyDead(Status);
    }

    public bool IsValidTarget(GameObject target) {
        return target.CompareTag(Define.Tag.Player);
    }

    public float Damage => Status.AttackDamage;
}
