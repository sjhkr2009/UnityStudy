using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IUnit
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    public MoveUI moveUI;
    public float speed = 3f;
    bool canMove;
    public int hp;
    public float noAttackedTime = 1f;

    public enum State
    {
        Idle,Attacked,Destroyed,Clear
    }

    public State state
    {
        get
        {
            return playerState;
        }
        set
        {
            switch (value)
            {
                case State.Idle:
                    playerState = value;
                    StopCoroutine("AfterAttacked");
                    break;
                case State.Attacked:
                    playerState = value;
                    StartCoroutine("AfterAttacked");
                    break;
                case State.Destroyed:
                    playerState = value;
                    canMove = false;
                    rb.velocity = Vector2.zero;
                    StageManager.instance.state = StageManager.State.GameOver;
                    break;
                case State.Clear:
                    playerState = value;
                    canMove = false;
                    rb.velocity = Vector2.zero;
                    StageManager.instance.state = StageManager.State.StageClear;
                    break;
            }
        }
    }
    private State playerState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        canMove = true;
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        if (moveUI.isMoving)
        {
            Move();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Move()
    {
        rb.velocity = moveUI.PlayerMove() * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Goal")
        {
            state = State.Clear;
        }
    }

    public void Attacked(int damage)
    {
        if (state == State.Idle)
        {
            hp -= damage;
            state = State.Attacked;
        }

        if (hp <= 0 && state != State.Destroyed)
        {
            state = State.Destroyed;
        }
    }

    IEnumerator AfterAttacked()
    {
        float cooltime = 0;
        while (cooltime < noAttackedTime - 0.2f)
        {
            sr.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.05f);
            sr.color = new Color(1, 0, 0, 0.8f);
            yield return new WaitForSeconds(0.05f);
            cooltime = cooltime + 0.1f;
        }
        sr.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);

        if (state == State.Attacked)
        {
            state = State.Idle;
        }
    }
}
