using Cysharp.Threading.Tasks;
using UnityEngine;

public class PeriodicLaserSkill : AbilityBase, ISkillAbility {
    public override AbilityIndex Index => AbilityIndex.SkillPeriodicLaser;
    
    public float OriginCooldown { get; } = 6f;
    public float Damage { get; protected set; }
    public float AttackDuration { get; protected set; }
    public float AttackInterval { get; protected set; }

    public override void Initialize(AbilityController controller) {
        base.Initialize(controller);
        Damage = Data.GetValue(AbilityValueType.Damage);
        AttackDuration = Data.GetValue(AbilityValueType.AttackDuration);
        AttackInterval = Data.GetValue(AbilityValueType.AttackInterval);
        
        GameManager.Player.SkillController.SetSkill1(this);
    }

    public UniTask Run(Vector2 usePoint, Vector2 direction) {
        var projectile = ProjectileSpawner.Spawn("LaserAttack00", CreateParam(usePoint, direction));
        projectile.transform.SetParent(GameManager.Player.View.SpiritTransform);
        return UniTask.CompletedTask;
    }

    ProjectileParam CreateParam(Vector2 point, Vector2 dir) {
        var param = ProjectileParam.CreatePeriodicAttackDefault(this, point);
        param.attackDurationTime = AttackDuration;
        param.direction = dir;
        param.damage = Damage;
        param.lifeTime = AttackDuration;
        param.attackInterval = AttackInterval;
        return param;
    }
}
