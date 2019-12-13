using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseUnit : MonoBehaviour
{
    [TabGroup("Basic")] [SerializeField] protected float hp = 1f;
    [TabGroup("Basic")] [SerializeField] protected float speed = 3.0f;
    [TabGroup("Basic")] [SerializeField] protected int exp = 0;
    [TabGroup("Basic")] [SerializeField] protected GameObject destroyFX;

    protected float originSpeed;
    float originHp;

    protected virtual void Awake()
    {
        originSpeed = speed;
        originHp = hp;
    }

    protected void DeadCheck()
    {
        if (hp <= 0f)
        {
            hp = 0f;
            UnitDestroy();
        }
    }

    protected void HitParticle()
    {
        GameObject _destroyFX = Instantiate(destroyFX, transform.position, transform.rotation);
        ParticleSystem destroyParticle = _destroyFX.GetComponent<ParticleSystem>();
        AudioSource destroyAudio = _destroyFX.GetComponent<AudioSource>();
        destroyParticle.Play();
        destroyAudio.Play();

        Destroy(_destroyFX, destroyParticle.main.duration);
    }

    protected void UnitDestroy()
    {
        HitParticle();
        gameObject.SetActive(false);
    }

    public void Attacked(float damage)
    {
        if(GameManager.instance.state == GameManager.State.Play)
        {
            hp -= damage;

            DeadCheck();
        }
    }
    protected void MoveToward()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    protected virtual void ReSpawn()
    {
        speed = originSpeed;
        hp = originHp;
    }

    private void OnDisable()
    {
        if(GameManager.instance.state == GameManager.State.Play)
        {
            GameManager.instance.currentExp += exp;
        }
    }
}
