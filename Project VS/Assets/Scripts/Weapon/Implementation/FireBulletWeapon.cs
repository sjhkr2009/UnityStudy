using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletWeapon : WeaponBase, IBulletCreator {
    public override ItemIndex Index => ItemIndex.WeaponAutoGun;
    public override Transform Transform { get; set; }
    
    public string bulletPrefabName = "Bullet01";
    
    public float BulletSpeed { get; set; }
    public float Penetration { get; set; }

    protected float fireTimer;
    protected Scanner scanner;
    
    public override void Initialize(ItemController controller) {
        base.Initialize(controller);

        SetDataByLevel();
        Transform = controller.transform;
        scanner = controller.Scanner;
    }

    public override void OnEveryFrame(float deltaTime) {
        fireTimer += deltaTime;
        if (fireTimer > AttackInterval) {
            fireTimer = 0f;
            Fire();
        }
    }

    public override void Upgrade() {
        base.Upgrade();
        SetDataByLevel();
    }
    
    private void SetDataByLevel() {
        var data = Data.GetValue(Level);
        Damage = data.damage;
        AttackInterval = data.attackInterval;
        AttackRange = data.attackRange;
        AttackCount = data.attackCount;

        Penetration = Level + 1;
        BulletSpeed = 5f + (1f / AttackInterval);
    }
    
    protected virtual void Fire() {
        var player = GameManager.Player;
        if (!player) return;

        if (!scanner.NearestTarget) return;

        var targetPos = scanner.NearestTarget.position;
        var dir = (targetPos - Transform.position).normalized;
        
        var bulletTr = PoolManager.Get(bulletPrefabName).transform;
        bulletTr.position = Transform!.position;
        bulletTr.rotation = Quaternion.FromToRotation(Vector3.up, dir); // y축을 기준으로 dir을 바라봄
        
        bulletTr.GetComponent<Bullet>().Initialize(CreateParam(dir));
    }

    private ProjectileParam CreateParam(Vector3 direction) {
        var param = new ProjectileParam {
            damage = Damage,
            range = AttackRange,
            penetration = Penetration,
            direction = direction,
            speed = BulletSpeed,
            startPoint = Transform.position
        };

        return param;
    }
}
