using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyRotate : BaseUnit
{
    [TabGroup("Enemy")] [SerializeField] Transform[] shootPoint;
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

        if (transform.position.z < -8f)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        body.transform.Rotate(0, 1, 0);
    }

    IEnumerator Shoot()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(shootDelay);

            if (-20f < transform.position.x && transform.position.x < 20f && -5f < transform.position.z && transform.position.z < 15f)
            {
                for (int i = 0; i < shootPoint.Length; i++)
                {
                    GameManager.instance.EnemyBulletShooting(shootPoint[i].position, shootPoint[i].rotation);
                }
            }
        }
    }

    IEnumerator SpeedDown()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(1f);

            speed *= 0.9f;
        }
    }
}
