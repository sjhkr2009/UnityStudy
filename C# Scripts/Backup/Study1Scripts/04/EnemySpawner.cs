using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public PoolManager poolManager;
    
    void Start()
    {
        StartCoroutine("CoSpawn");
    }

    IEnumerator CoSpawn()
    {
        while (true)
        {
            Vector3 pos = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(10f, 15f));
            float delay = Random.Range(0.5f, 2.5f);

            //Instantiate(enemy, pos, Quaternion.Euler(0f, 180f, 0f)); //오브젝트 풀링 방식에서는 일일이 생성하지 않는다
            poolManager.SpawnEnemy(pos);

            yield return new WaitForSeconds(delay);
        }
    }

    // void Update() 등의 함수를 안 쓸 경우 지워주는 게 최적화에 도움이 된다.
}
