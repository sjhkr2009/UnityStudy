using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyNormal4 : BaseUnit
{
    [TabGroup("Enemy")] [SerializeField] Transform body;
    [TabGroup("Enemy")] [SerializeField] float rotateLevel;
    [TabGroup("Enemy")] [SerializeField] float expireTime;
    [TabGroup("Homing")] [SerializeField] protected float homingRotation = 3f;
    [TabGroup("Homing")] [SerializeField] protected float homingRotationAdd = 0.5f;
    [TabGroup("Homing")] [SerializeField] protected float homingStartDelay = 0.2f;
    [TabGroup("Homing")] [SerializeField] protected float addSpeed = 0.05f;

    float originHomingRotation;
    GameObject target;


    void OnEnable()
    {
        ReSpawn();
        target = GameManager.instance.player.gameObject;
        StartCoroutine(Homing(target));
        StartCoroutine(Expire());
    }

    void Update()
    {
        DeadCheck();
        MoveToward();
        //body.LookAt(GameManager.instance.player.transform);

        if(transform.position.z < -5.5f)
        {
            gameObject.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        body.transform.Rotate(0, 0, rotateLevel);
    }

    IEnumerator Expire()
    {
        yield return new WaitForSeconds(expireTime);

        if (gameObject.activeSelf)
        {
            UnitDestroy();
        }
    }

    IEnumerator Homing(GameObject target)
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

            float difference = targetRotation - currentRotation;
            if (difference > 180f)
            {
                difference -= 360f;
            }
            else if (difference < -180f)
            {
                difference += 360f;
            }

            if (Mathf.Abs(difference) < homingRotation)
            {
                transform.rotation = Quaternion.Euler(targetDir);
            }
            else if (difference >= 0)
            {
                Quaternion getRotation = Quaternion.Euler(new Vector3(currentDir.x, currentRotation + homingRotation, currentDir.z));
                transform.rotation = getRotation;
            }
            else if (difference < 0)
            {
                Quaternion getRotation = Quaternion.Euler(new Vector3(currentDir.x, currentRotation - homingRotation, currentDir.z));
                transform.rotation = getRotation;
            }
            else
            {
                Debug.Log("Error!");
            }
            homingRotation += homingRotationAdd;

            speed += addSpeed;

            yield return new WaitForSeconds(0.04f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
        {
            BaseUnit target = other.transform.parent.gameObject.GetComponent<BaseUnit>();
            target.Attacked(1);
            UnitDestroy();
        }
    }
}
