using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;
    [BoxGroup("Normal")] [SerializeField] float normalPointMinX = -18f;
    [BoxGroup("Normal")] [SerializeField] float normalPointMaxX = 18f;
    [BoxGroup("Normal")] [SerializeField] float normalPointMinY = 16f;
    [BoxGroup("Normal")] [SerializeField] float normalPointMaxY = 20f;
    [BoxGroup("Normal")] [SerializeField] float spawnDelay = 1f;

    float spawnXPoint;
    float spawnYPoint;

    private void Awake()
    {
        spawnManager = GetComponent<SpawnManager>();
    }
    void Start()
    {
        StartCoroutine("NormalSpawn");
    }

    IEnumerator NormalSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            spawnXPoint = Random.Range(normalPointMinX, normalPointMaxX);
            spawnYPoint = Random.Range(normalPointMinY, normalPointMaxY);
            Vector3 spawnPoint = new Vector3(spawnXPoint, 0f, spawnYPoint);

            spawnManager.SpawnEnemy1(spawnPoint);
        }
    }
}
