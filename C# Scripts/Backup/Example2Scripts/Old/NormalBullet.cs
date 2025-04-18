﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BaseFlyingWeapon
{
    

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
        if (other.transform.parent.CompareTag(targetName))
        {
            BaseUnit target = other.transform.parent.gameObject.GetComponent<BaseUnit>();
            target.Attacked(damage);
            UnitDestroy();
        }
    }
}
