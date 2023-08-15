using Cysharp.Threading.Tasks;
using UnityEngine;

public class ChargeExplosionSkill : AbilityBase, ISkillAbility {
    public override AbilityIndex Index => AbilityIndex.SkillChargeExplosion;
    
    public float OriginCooldown { get; } = 30f;
    public float Damage { get; protected set; }

    public override void Initialize(AbilityController controller) {
        base.Initialize(controller);
        Damage = Data.GetValue(AbilityValueType.Damage);
        
        GameManager.Player.SkillController.SetSkill2(this);
    }

    public async UniTask Run(Vector2 usePoint, Vector2 direction) {
        var spiritTr = GameManager.Player.View?.SpiritTransform;
        if (!spiritTr) return;
        PoolManager.Get("Charge01", spiritTr);
        await UniTask.Delay(1700);
        
        ProjectileSpawner.Spawn("Explosion03", CreateParam(spiritTr.position));
    }

    ProjectileParam CreateParam(Vector2 point) {
        var param = ProjectileParam.CreateOnceAttackDefault(this, point);
        param.startDelayTime = 0.25f;
        param.lifeTime = 2f;
        param.damage = Damage * GameManager.Player.Status.AttackPower;
        return param;
    }
}
