using Cysharp.Threading.Tasks;
using UnityEngine;

public class FireballSkill : SkillBase {
    public override AbilityIndex Index => AbilityIndex.SkillFireball;
    
    public override float OriginCooldown { get; } = 3f;

    public override UniTask Run(Vector2 usePoint, Vector2 direction) {
        var dir = direction;
        
        var bulletTr = PoolManager.Get("Bullet01").transform;
        bulletTr.position = usePoint;
        bulletTr.rotation = Quaternion.FromToRotation(Vector3.up, dir); // y축을 기준으로 dir을 바라봄
        var param = new ProjectileParam {
            damage = Data.GetValue(AbilityValueType.Damage),
            range = Data.GetValue(AbilityValueType.AttackRange),
            penetration = Data.GetValue(AbilityValueType.Penetration),
            direction = direction,
            speed = Data.GetValue(AbilityValueType.MoveSpeed),
            startPoint = usePoint
        };
        bulletTr.GetComponent<Bullet>().Initialize(param);
        return UniTask.CompletedTask;
    }
}
