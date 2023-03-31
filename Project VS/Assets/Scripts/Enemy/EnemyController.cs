using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float speed;
    public Rigidbody2D target;

    private bool isAlive;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        isAlive = true;
    }

    private void FixedUpdate() {
        if (!isAlive) return;
        
        Vector2 dirVec = target.position - rigid.position;
        Vector2 deltaVector = dirVec.normalized * (speed * Time.deltaTime);
        
        rigid.MovePosition(rigid.position + deltaVector);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate() {
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }
}
