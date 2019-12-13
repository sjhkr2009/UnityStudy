using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyNormal1 : BaseUnit
{

    [TabGroup("Enemy")] [SerializeField] Transform shootPoint;
    [TabGroup("Enemy")] [SerializeField] Transform body;
    [TabGroup("Enemy")] [SerializeField] float shootDelay = 1f;

    void OnEnable()
    {
        ReSpawn();
        StartCoroutine(Shoot());
    }

    void Update()
    {
        DeadCheck();
        MoveToward();
        body.LookAt(GameManager.instance.player.transform);

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

            if(-20f < transform.position.x && transform.position.x < 20f && - 5f < transform.position.z && transform.position.z < 15f)
            {
                GameManager.instance.EnemyBulletShooting(shootPoint.position, body.rotation);
            }
        }
    }
}
