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
        
        penetration = param.penetration;
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

    private void OnTriggerEnter2D(Collider2D other) {
        // 충돌로 관통력이 0이 되면 파괴됩니다. 원래부터 관통력이 0 이하면 무시합니다. 
        if (!other.CompareTag(Define.Tag.Enemy) || penetration <= 0) return;

        penetration--;
        if (penetration <= 0) PoolManager.Abandon(gameObject);
    }

    private void Update() {
        if (GameManager.IsPause) return;

        elapsedTimeFromSpawned += Time.deltaTime;
        if (elapsedTimeFromSpawned > lifeTime) {
            PoolManager.Abandon(gameObject);
        } else if (Vector2.Distance(startPoint, transform.position) > range) {
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
