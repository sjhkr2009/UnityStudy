using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletWeapon : WeaponBase, IBulletCreator {
    public override ItemIndex Index => ItemIndex.WeaponAutoGun;
    public override Transform Transform { get; set; }
    
    public string bulletPrefabName = "Bullet01";

    private float fireInterval;
    public override float AttackSpeed {
        get => base.AttackSpeed;
        set {
            base.AttackSpeed = value;
            fireInterval = 1f / value;
        }
    }
    
    public float BulletSpeed { get; set; }
    public float Penetration { get; set; }

    protected float fireTimer;
    protected Scanner scanner;
    
    public override void Initialize(WeaponController controller) {
        Damage = 5;
        AttackSpeed = 2f;
        AttackRange = 10f;
        AttackCount = 1;
        BulletSpeed = 10f;
        Penetration = 2;
        Transform = controller.transform;
        scanner = controller.Scanner;
    }

    public override void OnUpdate(float deltaTime) {
        fireTimer += deltaTime;
        if (fireTimer > fireInterval) {
            fireTimer = 0f;
            Fire();
        }
    }

    public override void Upgrade() { }

    public override void Abandon() { }
    
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
