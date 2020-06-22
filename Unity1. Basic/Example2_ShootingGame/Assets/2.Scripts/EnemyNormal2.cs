using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyNormal2 : BaseUnit
{

    [TabGroup("Enemy")] [SerializeField] Transform shootPoint;
    [TabGroup("Enemy")] [SerializeField] float shootDelay = 2f;

    void OnEnable()
    {
        ReSpawn();
        StartCoroutine(Shoot());
    }

    void Update()
    {
        DeadCheck();
        MoveToward();

        if(transform.position.z < -5.5f)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Shoot()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(shootDelay);

            GameManager.instance.EnemyBulletShooting(shootPoint.position, transform.rotation);
        }
    }
}
