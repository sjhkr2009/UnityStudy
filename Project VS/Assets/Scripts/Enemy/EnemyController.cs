using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class EnemyController : EnemyControllerBase, IRepositionTarget {
    public Rigidbody2D target;

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
            if (!target) target = GameManager.Player.GetComponent<Rigidbody2D>();
            tracker.SetTarget(target);
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
        if (!other.CompareTag(Define.Tag.Projectile) || Status.IsDead) return;
        if (GameManager.IsPause) return;
        
        var weapon = other.GetComponent<IWeaponCollider>();
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
}
