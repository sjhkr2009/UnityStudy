using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IUnit
{
    GameObject destroyImage;
    AudioSource attacked;
    SpriteRenderer sr;
    public ParticleSystem destoryFX;

    public int hp = 3;
    public float noAttackedTime = 0.7f;

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
                    StopCoroutine("AfterAttacked");
                    break;
                case State.Attacked:
                    enemyState = value;
                    attacked.Play();
                    StartCoroutine("AfterAttacked");
                    break;
                case State.Destroyed:
                    enemyState = value;
                    destroyImage.SetActive(true);
                    break;
                case State.Clear:
                    enemyState = value;
                    StageManager.instance.state = StageManager.State.StageClear;
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
            state = State.Attacked;
        }

        if (hp <= 0 && state != State.Destroyed)
        {
            state = State.Destroyed;

            ParticleSystem instance = Instantiate(destoryFX, transform.position, transform.rotation);
            instance.Play();

            AudioSource expAudio = instance.GetComponent<AudioSource>();
            expAudio.Play();

            Destroy(instance.gameObject, instance.duration);
            gameObject.SetActive(false);
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
        destroyImage = transform.GetChild(0).gameObject;
        attacked = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }
}
