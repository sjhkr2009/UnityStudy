using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IUnit
{
    AudioSource attacked;
    SpriteRenderer sr;
    public Sprite destroyImage; 
    public ParticleSystem destoryFX;

    public int hp = 3;
    public float noAttackedTime = 1f;

    public enum State
    {
        Idle, Attacked, Destroyed, Clear
    }

    public State state
    {
        get
        {
            return enemyState;
        }
        set
        {
            switch (value)
            {
                case State.Idle:
                    enemyState = value;
                    break;
                case State.Attacked:
                    enemyState = value;
                    if(attacked != null)
                    {
                        attacked.Play();
                    }
                    StartCoroutine("AfterAttacked");
                    break;
                case State.Destroyed:
                    enemyState = value;
                    Destroy();
                    break;
                case State.Clear:
                    enemyState = value;
                    //StageManager.instance.state = StageManager.State.StageClear;
                    break;
            }
        }
    }
    private State enemyState;

    public void Attacked(int damage)
    {
        if (state == State.Idle)
        {
            hp -= damage;
            
            if(hp > 0)
            {
                state = State.Attacked;
            }
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

    void Start()
    {
        attacked = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (hp <= 0 && state != State.Destroyed)
        {
            state = State.Destroyed;
        }
    }
    
    public virtual void Destroy()
    {
        ParticleSystem instance = Instantiate(destoryFX, transform.position, transform.rotation);
        instance.Play();

        AudioSource expAudio = instance.GetComponent<AudioSource>();
        expAudio.Play();

        Destroy(instance.gameObject, instance.duration);
        sr.sprite = destroyImage;
    }
}
