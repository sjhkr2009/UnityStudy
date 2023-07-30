using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class SkillBase {
    public abstract float OriginCooldown { get; }
    public abstract UniTask Run(Vector2 usePoint, Vector2 direction);
}
