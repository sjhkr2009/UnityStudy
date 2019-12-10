using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BaseFlyingWeapon
{
    [SerializeField] GameObject[] targets;
    [SerializeField] GameObject target;
    GameObject newTarget;


    void Start()
    {
        
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        target = GetTarget();
        //나중에 리스트로 변경하고, SpawnManager에서 적 오브젝트를 풀링할 때 여기에 가져올 것
    }

    private void OnEnable()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        target = GetTarget();

        if(target != null)
        {
            StartCoroutine(Homing(target));
        }
    }

    void Update()
    {
        //OutRangeBulletExpire(); //범위를 벗어나면 사라지는 총알에 적용

        if (target.activeSelf == false || target == null)
        {
            speed = originSpeed;
            target = GetTarget();
            StartCoroutine(Homing(target));
        }

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

    GameObject GetTarget()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].activeSelf)
            {
                newTarget = targets[i];
                break;
            }
        }
        if(!newTarget.activeSelf || newTarget == null)
        {
            UnitDestroy();
        }

        return newTarget;
    }
}
