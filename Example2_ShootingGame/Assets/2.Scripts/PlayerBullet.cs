using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BaseFlyingWeapon
{
    
    void Start()
    {
        
    }

    void Update()
    {
        MoveToward();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //오브젝트의 BaseUnit을 통해 체력 감소
            BaseUnit enemy = other.gameObject.GetComponent<BaseUnit>();
            enemy.Attacked(1);
            UnitDestroy();
        }
    }
}
