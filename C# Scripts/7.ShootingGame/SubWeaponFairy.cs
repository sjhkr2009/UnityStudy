using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWeaponFairy : BaseSubWeapon
{
    
    private void OnEnable()
    {
        StartCoroutine(Shoot());
    }


    private void FixedUpdate()
    {
        if(GameManager.instance.state == GameManager.State.Play)
        {
            RoundMove();
        }
    }

    IEnumerator Shoot()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(shootDelay);

            float rotateLevel = Random.Range(0f, 360f);

            GameManager.instance.spawnManager.SpawnFairyBullet(transform.position, Quaternion.Euler(0f, rotateLevel, 0f));
        }
    }
}
