using Cysharp.Threading.Tasks;
using UnityEngine;

public class FireballSkill : AbilityBase, ISkillAbility {
    public override AbilityIndex Index => AbilityIndex.SkillFireball;
    
    public float OriginCooldown { get; } = 5f;
    public float Damage { get; protected set; }

    public override void Initialize(AbilityController controller) {
        base.Initialize(controller);
        Damage = Data.GetValue(AbilityValueType.Damage);
        
        GameManager.Player.SkillController.SetSkill1(this);
    }

    public UniTask Run(Vector2 usePoint, Vector2 direction) {
        var projectile = ProjectileSpawner.Spawn("Bullet01", CreateParam(usePoint, direction));
        projectile.transform.localScale = Vector3.one * 5f;
        return UniTask.CompletedTask;
    }

    ProjectileParam CreateParam(Vector2 point, Vector2 dir) {
        var param = ProjectileParam.CreateStraightDefault(this, point);
        param.direction = dir;
        param.damage = Damage  * GameManager.Player.Status.AttackPower;;
        return param;
    }
}
