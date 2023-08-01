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

    public static ProjectileParam CreateDefault() {
        return new ProjectileParam() {
            range = 20f,
            lifeTime = 10f,
            penetration = -1,
            speed = 10f
        };
    }
}
