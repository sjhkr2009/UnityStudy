using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile, IPoolHandler {
    private float range;
    private float penetration;
    private Rigidbody2D rigid;
    private Vector3 startPoint;

    private Vector2 tempVelocity = Vector2.zero;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }
    
    /// <param name="penetration">value lower 0 means infinite penetration</param>
    public override void Initialize(ProjectileParam param) {
        base.Initialize(param);
        penetration = param.penetration;
        range = param.range;
        startPoint = param.startPoint;
        rigid.velocity = param.direction * param.speed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(Define.Tag.Enemy) || penetration <= 0) return;

        penetration--;
        if (penetration <= 0) PoolManager.Abandon(gameObject);
    }

    private void Update() {
        if (Vector2.Distance(startPoint, transform.position) > range) {
            PoolManager.Abandon(gameObject);
        }
    }

    public override void OnPauseGame() {
        if (!gameObject.activeSelf) return;
        
        tempVelocity = rigid.velocity;
        rigid.velocity = Vector2.zero;
    }

    public override void OnResumeGame() {
        if (!gameObject.activeSelf) return;
        
        rigid.velocity = tempVelocity;
    }

    public void OnInitialize() {
        
    }

    public void OnRelease() {
        rigid.velocity = Vector2.zero;
        tempVelocity = Vector2.zero;
    }
}
