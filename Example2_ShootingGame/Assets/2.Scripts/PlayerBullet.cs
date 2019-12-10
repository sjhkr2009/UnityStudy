using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BaseFlyingWeapon
{
    
    void Start()
    {
        
    }

    private void OnEnable()
    {
        speed = originSpeed;
    }

    void Update()
    {
        OutRangeBulletExpire();
        MoveToward();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseUnit enemy = other.gameObject.GetComponent<BaseUnit>();
            enemy.Attacked(damage);
            UnitDestroy();
        }
    }
}
