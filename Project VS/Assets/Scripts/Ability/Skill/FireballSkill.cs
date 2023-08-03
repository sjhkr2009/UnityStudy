using Cysharp.Threading.Tasks;
using UnityEngine;

public class FireballSkill : AbilityBase, ISkillAbility {
    public override AbilityIndex Index => AbilityIndex.SkillFireball;
    
    public float OriginCooldown { get; } = 3f;

    public UniTask Run(Vector2 usePoint, Vector2 direction) {
        var projectile = ProjectileSpawner.SpawnStraight("Bullet01", Data.GetValue(AbilityValueType.Damage), usePoint, direction);
        projectile.transform.localScale = Vector3.one * 5f;
        return UniTask.CompletedTask;
    }
}
