using UnityEngine;

public class RandomAreaStrikeWeapon : AbilityBase, IWeaponAbility {
    public override AbilityIndex Index => AbilityIndex.WeaponAreaStrike;
    public Transform Transform { get; set; }
    
    public float Damage { get; set; }
    public float AttackInterval { get; set; }
    public float AttackRange { get; set; }
    public float AttackDelay { get; set; }

    protected float attackTimer;
    protected Scanner scanner;
    
    public override void Initialize(AbilityController controller) {
        base.Initialize(controller);

        SetDataByLevel();
        Transform = controller.transform;
        scanner = controller.Scanner;
    }

    public void OnEveryFrame(float deltaTime) {
        attackTimer += deltaTime;
        if (attackTimer > AttackInterval) {
            attackTimer = 0f;
            Attack();
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
        AttackDelay = Data.GetValue(AbilityValueType.ObjectSpeed, Level);
    }
    
    protected virtual void Attack() {
        var player = GameManager.Player;
        if (!player) return;

        var target = scanner.RandomTarget;
        Vector3 targetPos = target ? target.position : new Vector3(Random.value, Random.value) * 5f;

        ProjectileSpawner.Spawn("AreaStrike01", CreateParam(targetPos));
    }

    private ProjectileParam CreateParam(Vector3 targetPos) {
        var param = ProjectileParam.CreateOnceAttackDefault(targetPos);
        param.damage = Damage;
        param.range = AttackRange;

        return param;
    }
}
