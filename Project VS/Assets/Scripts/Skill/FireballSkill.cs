using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FireballSkill : SkillBase {
    public override float OriginCooldown { get; } = 3f;

    public override UniTask Run(Vector2 usePoint, Vector2 direction) {
        var dir = direction;
        
        var bulletTr = PoolManager.Get("Bullet01").transform;
        bulletTr.position = usePoint;
        bulletTr.rotation = Quaternion.FromToRotation(Vector3.up, dir); // y축을 기준으로 dir을 바라봄
        var param = new ProjectileParam {
            damage = 3,
            range = 10,
            penetration = 1,
            direction = direction,
            speed = 8,
            startPoint = usePoint
        };
        bulletTr.GetComponent<Bullet>().Initialize(param);
        return UniTask.CompletedTask;
    }
}
