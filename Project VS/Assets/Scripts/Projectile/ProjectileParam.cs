using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileParam {
    public AbilityBase ability;
    public IDamagableEntity[] ignoreTargets;
    public Vector3 startPoint;
    public Vector3 direction;
    public Action<Vector3> onAttack;
    public float damage;
    public float speed;
    public float range;
    public float size;
    public float attackInterval;
    public float lifeTime;
    public float startDelayTime;
    public float attackDurationTime;
    public float penetration;

    public static ProjectileParam CreateStraightDefault(AbilityBase ability, Vector3 startPoint) {
        return new ProjectileParam() {
            ability = ability,
            startPoint = startPoint,
            size = 1f,
            range = 20f,
            lifeTime = 10f,
            penetration = -1,
            speed = 10f
        };
    }
    
    public static ProjectileParam CreateOnceAttackDefault(AbilityBase ability, Vector3 startPoint) {
        return new ProjectileParam() {
            ability = ability,
            startPoint = startPoint,
            size = 1f,
            range = 1f,
            lifeTime = 1f,
            startDelayTime = 0f,
            attackDurationTime = 2f,
        };
    }
    
    public static ProjectileParam CreatePeriodicAttackDefault(AbilityBase ability, Vector3 startPoint) {
        return new ProjectileParam() {
            ability = ability,
            startPoint = startPoint,
            size = 1f,
            range = 1f,
            lifeTime = 2f,
            startDelayTime = 0f,
            attackDurationTime = 2f,
            attackInterval = 0.5f
        };
    }
}
