using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletWeapon : AbilityBase, IWeaponAbility, IBulletCreator {
    public override AbilityIndex Index => AbilityIndex.WeaponAutoGun;
    public Transform Transform { get; set; }
    
    public string bulletPrefabName = "Bullet01";
    
    public float Damage { get; set; }
    public float AttackInterval { get; set; }
    public float AttackRange { get; set; }
    public float BulletSpeed { get; set; }
    public float Penetration { get; set; }

    protected float fireTimer;
    protected Scanner scanner;
    
    public override void Initialize(AbilityController controller) {
        base.Initialize(controller);

        SetDataByLevel();
        Transform = controller.transform;
        scanner = controller.Scanner;
    }

    public void OnEveryFrame(float deltaTime) {
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
        Damage = Data.GetValue(AbilityValueType.Damage, Level);
        AttackInterval = Data.GetValue(AbilityValueType.AttackInterval, Level);
        AttackRange = Data.GetValue(AbilityValueType.AttackRange, Level);
        Penetration = Data.GetIntValue(AbilityValueType.Penetration, Level);
        BulletSpeed = Data.GetValue(AbilityValueType.ObjectSpeed, Level);
    }
    
    protected virtual void Fire() {
        var player = GameManager.Player;
        if (!player) return;

        if (!scanner.NearestTarget) return;

        var targetPos = scanner.NearestTarget.position;
        var dir = (targetPos - Transform.position).normalized;

        ProjectileSpawner.Spawn(bulletPrefabName, CreateParam(dir));
    }

    private ProjectileParam CreateParam(Vector3 direction) {
        var param = ProjectileParam.CreateStraightDefault(Transform.position);
        param.damage = Damage;
        param.range = AttackRange;
        param.penetration = Penetration;
        param.direction = direction;
        param.speed = BulletSpeed;

        return param;
    }
}
