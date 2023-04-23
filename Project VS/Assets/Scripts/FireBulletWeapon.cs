using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletWeapon : WeaponBase {
    public string bulletPrefabName = "Bullet01";
    public float fireInterval = 0.2f;
    public int fireCount = 1;
    public int firePenetration = 2;

    protected float fireTimer;
    protected EnemyScanner enemyScanner;
    
    public override void Initialize(WeaponController controller) {
        damage = 5;
        enemyScanner = controller.EnemyScanner;
    }

    public override void OnUpdate(float deltaTime) {
        fireTimer += deltaTime;
        if (fireTimer > fireInterval) {
            fireTimer = 0f;
            Fire();
        }
    }

    public override void OnUpgrade() { }

    public override void Abandon() { }
    
    protected virtual void Fire() {
        var player = GlobalCachedData.Player;
        if (!player) return;

        if (!enemyScanner.nearestTarget) return;

        var targetPos = enemyScanner.nearestTarget.position;
        var dir = (targetPos - transform.position).normalized;
        
        var bulletTr = PoolManager.Get(bulletPrefabName).transform;
        bulletTr.position = transform.position;
        bulletTr.rotation = Quaternion.FromToRotation(Vector3.up, dir); // y축을 기준으로 dir을 바라봄
        
        bulletTr.GetComponent<Projectile>().Initialize(damage, firePenetration, dir);
    }
}
