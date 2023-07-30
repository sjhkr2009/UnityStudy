using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class SkillBase : AbilityBase {
    public abstract float OriginCooldown { get; }
    public abstract UniTask Run(Vector2 usePoint, Vector2 direction);
}
