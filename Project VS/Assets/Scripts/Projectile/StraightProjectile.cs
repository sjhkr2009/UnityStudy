using UnityEngine;

/** 직선 형태로 나아가는 투사체. 관통력이 소진되거나 일정 거리만큼 벌어지면 사라진다. */
public class StraightProjectile : Projectile {
    private float range;
    private float lifeTime;
    private float penetration;
    private Rigidbody2D rigid;
    private Vector3 startPoint;
    
    private float elapsedTimeFromSpawned;
    private Vector2 tempVelocity = Vector2.zero;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }
    
    /// <param name="penetration">value lower 0 means infinite penetration</param>
    public override void Initialize(ProjectileParam param) {
        base.Initialize(param);
        
        penetration = param.penetration <= 0 ? 9999 : param.penetration;
        range = param.range;
        lifeTime = param.lifeTime;
        startPoint = param.startPoint;
        transform.position = startPoint;
        rigid.velocity = param.direction * param.speed;

        if (range <= 0f || lifeTime <= 0f) {
            Debugger.Error($"[StraightProjectile.Initialize] Param is invalid! (Range: {range} | LifeTime: {lifeTime})");
            if (range <= 0f) range = 10f;
            if (lifeTime <= 0f) lifeTime = 10f;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        // 충돌로 관통력이 0 미만이 되면 파괴됩니다.
        if (penetration <= 0) return;
        if (!IsValidTarget(other.gameObject)) return;

        var damageHandler = other.GetComponent<IDamagableEntity>();
        if (damageHandler == null) return;
        var attackPos = other.bounds.ClosestPoint(transform.position);
        var result = damageHandler.OnAttacked(this, attackPos);
        OnAttack?.Invoke(attackPos);
        ShowHitEffect();

        if (!result.isHit) return;
        
        penetration--;
        if (penetration <= 0) PoolManager.Abandon(gameObject);
    }

    private void Update() {
        if (GameManager.IsPause) return;

        elapsedTimeFromSpawned += Time.deltaTime;
        if (elapsedTimeFromSpawned > lifeTime) {
            OnAttack?.Invoke(transform.position);
            PoolManager.Abandon(gameObject);
        } else if (Vector2.Distance(startPoint, transform.position) > range) {
            OnAttack?.Invoke(transform.position);
            PoolManager.Abandon(gameObject);
        }
    }

    public override void OnPauseGame() {
        if (!gameObject.activeSelf) return;
        
        tempVelocity = rigid.velocity;
        rigid.velocity = Vector2.zero;
    }

    public override void OnResumeGame() {
        if (!gameObject.activeSelf) return;
        
        rigid.velocity = tempVelocity;
    }

    public override void OnInitialize() {
        base.OnInitialize();
        elapsedTimeFromSpawned = 0f;
    }

    public override void OnRelease() {
        rigid.velocity = Vector2.zero;
        tempVelocity = Vector2.zero;
    }
}
