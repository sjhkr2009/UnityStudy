using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class EnemyController : EnemyControllerBase, IRepositionTarget, IAttackableCollider, IDamagableEntity {
    private Collider2D _collider;
    public Collider2D GetCollider => _collider;

    protected override void Awake() {
        _collider = GetComponent<Collider2D>();
        base.Awake();
    }

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

    private void OnCollisionStay2D(Collision2D other) {
        if (!other.gameObject.CompareTag(Define.Tag.Player)) return;

        var damagable = other.gameObject.GetComponent<IDamagableEntity>();
        damagable?.OnAttacked(this);
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

    public DamageData GetDamageData() {
        return new DamageData(Status.AttackDamage);
    }
    
    public AttackResult OnAttacked(IAttackableCollider attacker) {
        if (isPaused || Status.IsDead) return AttackResult.None;
        
        ApplyDamage(attacker.GetDamageData());
        return Status.IsDead ? AttackResult.Dead : AttackResult.Hit;
    }

    void ApplyDamage(DamageData data) {
        Status.Hp -= data.damage;
        
        if (Status.Hp <= 0) {
            Dead();
        } else {
            view.OnHit();
            moveStrategy.OnHit();
        }
    }
}
