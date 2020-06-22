using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHomingBullet : BaseFlyingWeapon
{
    [SerializeField] List<GameObject> targetList = new List<GameObject>(); //스폰 매니저의 리스트로 대체 예정
    [SerializeField] GameObject target;
    GameObject newTarget;


    private void OnEnable()
    {
        hp = originHp;
        targetList = GameManager.instance.spawnManager.allEnemyList; //나중에 리스트로 변경하고, SpawnManager에서 적 오브젝트를 풀링할 때 여기에 가져올 것
        target = GetNearTarget();
        targetName = "Enemy";

        if(target != null)
        {
            StartCoroutine(Homing(target));
        }
    }

    void Update()
    {
        //OutRangeBulletExpire(); //범위를 벗어나면 사라지는 총알에 적용

        if (target == null || target.activeSelf == false)
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
        if (other.transform.parent.CompareTag(targetName))
        {
            BaseUnit targetObject = other.transform.parent.gameObject.GetComponent<BaseUnit>();
            targetObject.Attacked(damage);
            HitParticle();
            homingRotation = originHomingRotation;
            hp -= 1f;
            if(hp <= 0f)
            {
                UnitDestroy();
            }
        }
    }

    GameObject GetNearTarget()
    {
        float minDistance = 1000f;
        
        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].activeSelf)
            {
                float distance = Vector3.Distance(transform.position, targetList[i].transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    newTarget = targetList[i];
                }
            }
        }
        if(GameManager.instance.state == GameManager.State.Play)
        {
            if (newTarget == null || !newTarget.activeSelf)
            {
                UnitDestroy();
                return null;
            }
        }

        return newTarget;
    }
}
