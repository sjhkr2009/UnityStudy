﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;
    [TabGroup("Normal")] [SerializeField] float normalPointMinX = -16f;
    [TabGroup("Normal")] [SerializeField] float normalPointMaxX = 16f;
    [TabGroup("Normal")] [SerializeField] float normalPointMinY = 16f;
    [TabGroup("Normal")] [SerializeField] float normalPointMaxY = 20f;
    [TabGroup("Normal")] [SerializeField] float normal1SpawnDelay = 4f;
    [TabGroup("Normal")] [SerializeField] float normal3SpawnDelay = 2.5f;
    [TabGroup("Normal")] [SerializeField] float normal4SpawnDelay = 2.5f;
    [TabGroup("Rotate")] [SerializeField] float rotPointMinX = -10f;
    [TabGroup("Rotate")] [SerializeField] float rotPointMaxX = 10f;
    [TabGroup("Rotate")] [SerializeField] float rotPointMinY = 16f;
    [TabGroup("Rotate")] [SerializeField] float rotPointMaxY = 20f;
    [TabGroup("Rotate")] [SerializeField] float rotMinRotation = 172.5f;
    [TabGroup("Rotate")] [SerializeField] float rotMaxRotation = 187.5f;
    [TabGroup("Rotate")] [SerializeField] float rotSpawnDelay = 1f;

    float spawnXPoint;
    float spawnYPoint;
    float spawnRotation;

    private void Awake()
    {
        spawnManager = GetComponent<SpawnManager>();
    }
    public void Spawn()
    {
        StartCoroutine("Normal1Spawn");
        StartCoroutine("Normal3Spawn");
        StartCoroutine("RotateSpawn");
    }

    IEnumerator Normal1Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(normal1SpawnDelay);

            spawnXPoint = Random.Range(normalPointMinX, normalPointMaxX);
            spawnYPoint = Random.Range(normalPointMinY, normalPointMaxY);
            Vector3 spawnPoint = new Vector3(spawnXPoint, 0f, spawnYPoint);

            spawnManager.SpawnEnemy1(spawnPoint);
        }
    }

    IEnumerator Normal3Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(normal3SpawnDelay);

            spawnXPoint = Random.Range(normalPointMinX, normalPointMaxX);
            spawnYPoint = Random.Range(normalPointMinY, normalPointMaxY);
            Vector3 spawnPoint = new Vector3(spawnXPoint, 0f, spawnYPoint);

            spawnManager.SpawnEnemy3(spawnPoint);
        }
    }

    IEnumerator Normal4Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(normal4SpawnDelay);

            spawnXPoint = Random.Range(normalPointMinX/2f, normalPointMaxX/2f);
            spawnYPoint = Random.Range(normalPointMinY, normalPointMaxY);
            Vector3 spawnPoint = new Vector3(spawnXPoint, 0f, spawnYPoint);

            spawnManager.SpawnEnemy4(spawnPoint);
        }
    }

    IEnumerator RotateSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(rotSpawnDelay);

            spawnXPoint = Random.Range(rotPointMinX, rotPointMaxX);
            spawnYPoint = Random.Range(rotPointMinY, rotPointMaxY);
            spawnRotation = Random.Range(rotMinRotation, rotMaxRotation);
            Vector3 spawnPoint = new Vector3(spawnXPoint, 0f, spawnYPoint);

            spawnManager.SpawnEnemy2(spawnPoint, spawnRotation);
        }
    }
}
