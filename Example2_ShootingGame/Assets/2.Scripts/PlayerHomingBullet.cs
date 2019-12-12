using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHomingBullet : BaseFlyingWeapon
{
    GameObject[] targets; //스폰 매니저의 리스트로 대체 예정
    [SerializeField]GameObject target;
    GameObject newTarget;



    private void OnEnable()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy"); //나중에 리스트로 변경하고, SpawnManager에서 적 오브젝트를 풀링할 때 여기에 가져올 것
        target = GetNearTarget();

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
            target = GetNearTarget();
            if (target != null)
            {
                StartCoroutine(Homing(target));
            }
        }

        MoveToward();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Enemy"))
        {
            BaseUnit enemy = other.transform.parent.gameObject.GetComponent<BaseUnit>();
            enemy.Attacked(damage);
            UnitDestroy();
        }
    }

    GameObject GetNearTarget()
    {
        float minDistance = 1000f;
        
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].activeSelf)
            {
                float distance = Vector3.Distance(transform.position, targets[i].transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    newTarget = targets[i];
                }
            }
        }
        if(!newTarget.activeSelf || newTarget == null)
        {
            UnitDestroy();
            return null;
        }

        return newTarget;
    }
}
