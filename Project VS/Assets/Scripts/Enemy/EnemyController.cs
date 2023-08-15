using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class EnemyController : EnemyControllerBase, IRepositionTarget, IAttackableCollider, IDamagableEntity {
    public AbilityBase ParentAbility { get; }
    public float Damage => Status.AttackDamage;

    public override void OnInitialize() {
        base.OnInitialize();
        view = new EnemyView(Status);
        if (moveComponent) {
            moveComponent.Initialize(Status);
            moveStrategy = moveComponent;
        } else {
            moveStrategy = new EnemyMoveStrategy(Status);
        }
        
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
        damagable?.OnAttacked(this, other.contacts[0].point);
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
    
    public AttackResult OnAttacked(IAttackableCollider attacker, Vector2 attackPos) {
        if (GameManager.IsPause || Status.IsDead) return AttackResult.None;
        if (attacker.Damage <= 0) return AttackResult.Hit;

        var data = new DamageData(attacker, this, attackPos);
        ApplyDamage(data, attackPos);
        return Status.IsDead ? AttackResult.Dead : AttackResult.Hit;
    }

    private bool CheckCritical() {
        var critPer = Random.Range(1, 101);

        if (GameManager.Player.Status.Critical >= critPer) {
            return true;
        }

        return false;
    }

    void ApplyDamage(DamageData data, Vector2 attackPos) {
        // TODO: 데미지 텍스트는 위치와 데미지만 전달하면 알아서 그 자리에서 연출하고 사라지게 수정
        /*var damageText = PoolManager.Get<DamageText>("Damage");
        
        if (CheckCritical()) {
            data.damage *= 2f;
            damageText.damageText.color = Color.red;
            damageText.critBG.SetActive(true);
        } else {
            damageText.damageText.color = Color.yellow;
            damageText.critBG.SetActive(false);
        }

        damageText.transform.position = attackPos;
        damageText.transform.DOMoveY(0.5f, 0.6f).SetRelative().OnComplete(() => PoolManager.Abandon(damageText.gameObject));
        damageText.damageText.text = Mathf.FloorToInt(data.damage).ToString();*/
        
        Status.Hp -= data.damage;
        GameBroadcaster.CallEnemyHit(data, Status);
        
        if (Status.Hp <= 0) {
            Dead();
        } else {
            view.OnHit(data.hitAbility);
            moveStrategy.OnHit(data.hitAbility);
        }
    }
}
