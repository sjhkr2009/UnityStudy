using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseFlyingWeapon : MonoBehaviour
{
    [BoxGroup("Common")] [SerializeField] protected float speed;
    [BoxGroup("Common")] [SerializeField] protected float damage = 1f;
    [BoxGroup("Common")] [SerializeField] protected float hp = 1f;
    [BoxGroup("Common")] [SerializeField] protected GameObject destroyFX;
    [BoxGroup("Common")] [SerializeField] protected string targetName;
    [TabGroup("Normal")] [SerializeField] float minX = -20f;
    [TabGroup("Normal")] [SerializeField] float maxX = 20f;
    [TabGroup("Normal")] [SerializeField] float minY = -10f;
    [TabGroup("Normal")] [SerializeField] float maxY = 20f;
    [TabGroup("Homing")] [SerializeField] protected float homingRotation = 3f;
    [TabGroup("Homing")] [SerializeField] protected float homingRotationAdd = 0.5f;
    [TabGroup("Homing")] [SerializeField] protected float homingStartDelay = 0.2f;

    protected float originSpeed;
    protected float originHp;
    protected float originHomingRotation;
    private void Awake()
    {
        originSpeed = speed;
        originHp = hp;
        originHomingRotation = homingRotation;

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

    protected void MoveToward()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
    
    protected IEnumerator Homing(GameObject target)
    {
        yield return new WaitForSeconds(homingStartDelay);
        homingRotation = originHomingRotation;

        while (this.gameObject.activeSelf)
        {
            if (!target.activeSelf)
            {
                break;
            }
            
            Vector3 targetDir = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles;
            float targetRotation = targetDir.y;
            Vector3 currentDir = transform.rotation.eulerAngles;
            float currentRotation = currentDir.y;

            //Debug.Log($"타겟: {targetRotation} / 총알: {currentRotation}");

            float difference = targetRotation - currentRotation;
            if(difference > 180f)
            {
                difference -= 360f;
            }
            else if(difference < -180f)
            {
                difference += 360f;
            }
            //Debug.Log($"회전해야 하는 각도: {difference}");

            if (Mathf.Abs(difference) < homingRotation)
            {
                transform.rotation = Quaternion.Euler(targetDir);
            }
            else if(difference >= 0)
            {
                Quaternion getRotation = Quaternion.Euler(new Vector3(currentDir.x, currentRotation + homingRotation, currentDir.z));
                transform.rotation = getRotation;
            } else if(difference < 0)
            {
                Quaternion getRotation = Quaternion.Euler(new Vector3(currentDir.x, currentRotation - homingRotation, currentDir.z));
                transform.rotation = getRotation;
            }
            else
            {
                Debug.Log("Error!");
            }
            homingRotation += homingRotationAdd;

            speed = originSpeed + homingRotation/3f;

            yield return new WaitForSeconds(0.04f);
        }
    }

    protected void OutRangeBulletExpire()
    {
        Vector3 thisPos = transform.position;
        if(thisPos.x < minX || thisPos.x > maxX || thisPos.z < minY || thisPos.z > maxY)
        {
            gameObject.SetActive(false);
        }
    }
}
