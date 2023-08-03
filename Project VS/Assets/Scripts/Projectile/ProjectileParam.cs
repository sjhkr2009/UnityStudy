using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileParam {
    public Vector3 startPoint;
    public Vector3 direction;
    public float damage;
    public float speed;
    public float range;
    public float lifeTime;
    public float penetration;

    public static ProjectileParam CreateStraightDefault(Vector3 startPoint) {
        return new ProjectileParam() {
            startPoint = startPoint,
            range = 20f,
            lifeTime = 10f,
            penetration = -1,
            speed = 10f
        };
    }
    
    public static ProjectileParam CreateOnceAttackDefault(Vector3 startPoint) {
        return new ProjectileParam() {
            startPoint = startPoint,
            range = 1f,
            lifeTime = 2f
        };
    }
}
