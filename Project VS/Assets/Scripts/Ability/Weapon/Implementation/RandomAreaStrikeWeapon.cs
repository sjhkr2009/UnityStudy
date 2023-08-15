using Cysharp.Threading.Tasks;
using UnityEngine;

public class RandomAreaStrikeWeapon : AbilityBase, IWeaponAbility {
    public override AbilityIndex Index => AbilityIndex.WeaponRandomAreaStrike;
    public Transform Transform { get; set; }
    
    public float Damage { get; set; }
    public float AttackInterval { get; set; }
    public int AttackCount { get; set; }

    protected float attackTimer;
    
    public override void Initialize(AbilityController controller) {
        base.Initialize(controller);

        SetDataByLevel();
        attackTimer = AttackInterval - 1f;
    }

    public void OnEveryFrame(float deltaTime) {
        attackTimer += deltaTime * GameManager.Player.Status.AttackSpeed;
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
        AttackCount = Data.GetIntValue(AbilityValueType.AttackCount, Level);
    }

    private async UniTaskVoid Attack() {
        for (int i = 0; i < AttackCount; i++) {
            var targetPos = GameManager.Ability.GetRandomPositionInCameraView();
            ProjectileSpawner.Spawn("DropStrike00", CreateParam(targetPos));
            await UniTask.Delay(3000 / AttackCount);
        }
    }

    private ProjectileParam CreateParam(Vector3 targetPos) {
        var param = ProjectileParam.CreateOnceAttackDefault(this, targetPos);
        param.damage = Damage  * GameManager.Player.Status.AttackPower;
        param.attackDurationTime = 0.3f;
        param.startDelayTime = 0.8f;

        return param;
    }
}
