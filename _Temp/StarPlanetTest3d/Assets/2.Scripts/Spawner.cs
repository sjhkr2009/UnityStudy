using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] ObjectPool objectPool;
    [SerializeField] float minSpawnDelay;
    [SerializeField] float maxSpawnDelay;

    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
        StartCoroutine(EnemySpawn());
    }

    void Update()
    {
        
    }

    IEnumerator EnemySpawn()
    {
        while (true)
        {
            float cameraSize = Camera.main.orthographicSize;

            float minX = -cameraSize * 9f / 16f - 1f;
            float maxX = cameraSize * 9f / 16f + 1f;

            float posX = Random.Range(minX, maxX);
            float posY = cameraSize + 1f;
            Debug.Log($"X값 범위: {minX} ~ {maxX}");

            if(Random.value < 0.5f)
            {
                posY = -cameraSize - 1f;
            }
            
            Vector3 position = new Vector3(posX, 0f, posY);

            if (Random.value < 0.5f)
            {
                objectPool.EnemyTP1Spawn(position, Quaternion.LookRotation(Vector3.zero - position));
            }
            else
            {
                objectPool.EnemyTS1Spawn(position, Quaternion.LookRotation(Vector3.zero - position));
            }

            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
