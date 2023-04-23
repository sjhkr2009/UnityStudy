using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolHandler {
    public float damage;
    public int penetration;

    private Rigidbody2D rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }
    
    public void Initialize(float damage, int penetration = -1) => Initialize(damage, penetration, Vector3.zero);

    /// <param name="penetration">value lower 0 means infinite penetration</param>
    public void Initialize(float damage, int penetration, Vector3 dir) {
        this.damage = damage;
        this.penetration = penetration;
        if (penetration > 0) rigid.velocity = dir;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(Define.Tag.Enemy) || penetration <= 0) return;

        penetration--;
        if (penetration <= 0) PoolManager.Abandon(gameObject);
    }

    public void OnInitialize() {
        
    }

    public void OnRelease() {
        rigid.velocity = Vector2.zero;
    }
}
