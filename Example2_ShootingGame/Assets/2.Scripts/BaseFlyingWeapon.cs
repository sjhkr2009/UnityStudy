using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class BaseFlyingWeapon : MonoBehaviour
{
    [TabGroup("Basic")] [SerializeField] protected float speed;
    [TabGroup("Basic")] [SerializeField] protected GameObject destroyFX;
    [TabGroup("Basic")] [SerializeField] float minX = -20f;
    [TabGroup("Basic")] [SerializeField] float maxX = 20f;
    [TabGroup("Basic")] [SerializeField] float minY = -10f;
    [TabGroup("Basic")] [SerializeField] float maxY = 20f;
    [TabGroup("Special")] [SerializeField] float homingRotationStart = 3f;
    [TabGroup("Special")] [SerializeField] float homingRotationAdd = 0.1f;
    [TabGroup("Special")] [SerializeField] float homingDelay = 0.2f;


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
    
    protected IEnumerator Homing(GameObject target)
    {
        yield return new WaitForSeconds(homingDelay);
        
        while (this.gameObject.activeSelf)
        {
            Vector3 targetDir = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles;
            float targetRotation = targetDir.y;
            Vector3 currentDir = transform.rotation.eulerAngles;
            float currentRotation = currentDir.y;

            Debug.Log($"타겟: {targetRotation} / 총알: {currentRotation}");

            float difference = targetRotation - currentRotation;
            if(difference > 180f)
            {
                difference -= 360f;
            }
            else if(difference < -180f)
            {
                difference += 360f;
            }
            Debug.Log($"회전해야 하는 각도: {difference}");

            if (Mathf.Abs(difference) < homingRotationStart)
            {
                transform.rotation = Quaternion.Euler(targetDir);
            }
            else if(difference >= 0)
            {
                Quaternion getRotation = Quaternion.Euler(new Vector3(currentDir.x, currentRotation + homingRotationStart, currentDir.z));
                transform.rotation = getRotation;
            } else if(difference < 0)
            {
                Quaternion getRotation = Quaternion.Euler(new Vector3(currentDir.x, currentRotation - homingRotationStart, currentDir.z));
                transform.rotation = getRotation;
            }
            else
            {
                Debug.Log("Error!");
            }
            homingRotationStart += homingRotationAdd;

            yield return new WaitForSeconds(0.04f);
        }
    }

    protected void ExpiredCheck()
    {
        Vector3 thisPos = transform.position;
        if(thisPos.x < minX || thisPos.x > maxX || thisPos.z < minY || thisPos.z > maxY)
        {
            gameObject.SetActive(false);
        }
    }
}
