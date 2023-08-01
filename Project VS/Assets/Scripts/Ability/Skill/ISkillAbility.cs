using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISkillAbility {
    float OriginCooldown { get; }
    UniTask Run(Vector2 usePoint, Vector2 direction);
}
