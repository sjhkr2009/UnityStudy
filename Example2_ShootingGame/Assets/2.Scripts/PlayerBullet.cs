using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BaseFlyingWeapon
{
    public GameObject target;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        target = GameObject.FindWithTag("Enemy");
        StartCoroutine(Homing(target));
    }

    void Update()
    {
        //ExpiredCheck();

        MoveToward();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseUnit enemy = other.gameObject.GetComponent<BaseUnit>();
            enemy.Attacked(1);
            UnitDestroy();
        }
    }
}
