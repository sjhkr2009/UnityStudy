using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnDelay;
    SpawnManager spawnManager;

    void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
        StartCoroutine("CoEnemySpawn");
        
    }

    IEnumerator CoEnemySpawn()
    {
        while (true)
        {
            Vector3 pos = new Vector3(Random.Range(-18f, 18f), 0f, Random.Range(21f, 25f));
            float delay = Random.Range(spawnDelay, spawnDelay + 1.5f);
            Debug.Log($"({pos.x}, {pos.z}) 위치에 적 생성을 시작합니다.");
            spawnManager.EnemySpawn(pos);
            yield return new WaitForSeconds(delay);
        }
    }
}
