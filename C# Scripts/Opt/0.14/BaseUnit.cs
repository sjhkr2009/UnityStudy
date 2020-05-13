using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour, IUnit
{
    public enum State
    {
        Idle, Attacked, Destroyed, Clear
    }
    public State state;

    SpriteRenderer sr;
    public int hp;
    public float noAttackedTime;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
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
        while (cooltime < noAttackedTime)
        {
            sr.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.05f);
            sr.color = new Color(1, 0, 0, 0.8f);
            yield return new WaitForSeconds(0.05f);
            cooltime = cooltime + 0.1f;
        }
        sr.color = new Color(1, 1, 1, 1);

        if (state == State.Attacked)
        {
            state = State.Idle;
        }
    }
    
}
