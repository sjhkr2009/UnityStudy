using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseUnit : MonoBehaviour
{
    [TabGroup("Basic")] [SerializeField] protected int hp = 1;
    [TabGroup("Basic")] [SerializeField] protected float speed = 3.0f;
    [TabGroup("Basic")] [SerializeField] protected GameObject destroyFX;

    protected void UnitDestroy()
    {
        GameObject _destroyFX = Instantiate(destroyFX, transform.position, transform.rotation);
        ParticleSystem destroyParticle = _destroyFX.GetComponent<ParticleSystem>();
        AudioSource destroyAudio = _destroyFX.GetComponent<AudioSource>();
        destroyParticle.Play();
        destroyAudio.Play();

        Destroy(_destroyFX, destroyParticle.duration);
        gameObject.SetActive(false);
    }

    public void Attacked(int damage)
    {
        hp--;
        if (hp <= 0)
        {
            UnitDestroy();
        }
    }

    protected void ShootingEnemyMove()
    {
        //투사체를 쏘는 적의 움직임
    }

    protected void CollisionEnemyMove()
    {
        //충돌하여 공격하면 적의 움직임
    }
}
