using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 매번 오브젝트를 생성/파괴하는 것은 리소스를 많이 먹으니 오브젝트 풀링을 이용한다.
    // 오브젝트를 파괴하는 대신 비활성화한 후, 원래 생성되던 위치로 옮겨 다시 활성화한다.

    public GameObject bulletPrefab;
    public GameObject enemyPrefab;

    public GameObject[] bulletArray;
    public GameObject[] enemyArray;

    public int poolCount = 100; //여기서는 아예 100개를 생성해버리지만, 숫자에 맞게 생성되도록 리스트로 만들수도 있다.

    int currentEnemyIndex = 0;
    int currentBulletIndex = 0;

    private void Awake() //시작 시 오브젝트를 한꺼번에 생성해 배열에 넣고, 비활성화 상태로 세팅한다. 
    {
        bulletArray = new GameObject[poolCount];
        for (int i = 0; i < poolCount; i++)
        {
            bulletArray[i] = Instantiate(bulletPrefab);
            bulletArray[i].SetActive(false);
        }

        enemyArray = new GameObject[poolCount];
        for (int i = 0; i < poolCount; i++)
        {
            enemyArray[i] = Instantiate(enemyPrefab);
            enemyArray[i].SetActive(false);
        }

        //활성화 상태인지 아닌지 알아보는 함수는 GameObject.activeSelf가 있다.
    }

    public void SpawnEnemy(Vector3 pos)
    {
        enemyArray[currentEnemyIndex].transform.position = pos;
        enemyArray[currentEnemyIndex].SetActive(true);
        currentEnemyIndex++;
    }

    public void SpawnBullet(Vector3 pos)
    {
        
    }

    private void Update()
    {
        if(currentEnemyIndex >= poolCount)
        {
            currentEnemyIndex = 0;
        }
        if(currentBulletIndex >= poolCount)
        {
            currentBulletIndex = 0;
        }
    }
}
