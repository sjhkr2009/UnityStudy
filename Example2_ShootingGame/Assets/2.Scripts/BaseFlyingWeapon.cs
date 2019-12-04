using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseFlyingWeapon : MonoBehaviour
{
    [TabGroup("Basic")] [SerializeField] protected float speed;
    [TabGroup("Basic")] [SerializeField] protected GameObject destroyFX;
    [TabGroup("Basic")] [SerializeField] float minX = -20f;
    [TabGroup("Basic")] [SerializeField] float maxX = 20f;
    [TabGroup("Basic")] [SerializeField] float minY = -10f;
    [TabGroup("Basic")] [SerializeField] float maxY = 20f;

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

    protected void MoveToward()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    private void Update()
    {
        Vector3 thisPos = transform.position;
        if(thisPos.x < minX || thisPos.x > maxX || thisPos.z < minY || thisPos.z > maxY)
        {
            gameObject.SetActive(false);
        }
    }
}
