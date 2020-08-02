using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    float cameraSizeX => cameraController.CameraXSize;
    float cameraSizeY => cameraController.CameraYSize;
    float cameraPosZ => cameraController.CameraZPos;

    PoolManager poolManager;
    CameraController cameraController;

    void Start()
    {
        poolManager = GetComponent<PoolManager>();
        cameraController = GameManager.Instance.CameraController;
    }

    public Vector3 RandomSpawnPositionOverMap()
    {
        float minX = -cameraSizeX * 1.33f;
        float maxX = cameraSizeX * 1.33f;
        float posX = Random.Range(minX, maxX);

        float posY = cameraSizeY * 1.1f + cameraPosZ;
        if (Random.value < 0.5f) posY = -cameraSizeY * 1.1f + cameraPosZ;

        Vector3 spawnPos = new Vector3(posX, 0f, posY);

        return spawnPos;
    }

    public Component SpawnOverMapToCenter(ObjectPool objectType)
    {
        Component newObject = null;
        Vector3 spawnPos = RandomSpawnPositionOverMap();
        newObject = poolManager.Spawn(objectType, spawnPos, Quaternion.LookRotation(Vector3.zero - spawnPos));
        return newObject;
    }

    public Component SpawnOnMap(ObjectPool objectType)
    {
        Vector3 spawnPos = Vector3.zero;
        while (Vector3.Distance(spawnPos, Vector3.zero) < 2.5f)
        {
            spawnPos = new Vector3(Random.Range(-cameraSizeX * 0.95f, cameraSizeX * 0.95f), 0f, Random.Range(-cameraSizeY * 0.95f + cameraPosZ, cameraSizeY * 0.95f + cameraPosZ));
        }
        Component newObject = poolManager.Spawn(objectType, spawnPos, Quaternion.identity);
        return newObject;
    }
    public Component SpawnOverMapToRandom(ObjectPool objectType)
    {
        Vector3 spawnPosUp = new Vector3(Random.Range(-cameraSizeX, cameraSizeX), 0f, cameraSizeY * 1.1f + cameraPosZ);
        Vector3 spawnPosRight = new Vector3(cameraSizeX * 1.2f, 0f, Random.Range(-cameraSizeY + cameraPosZ, cameraSizeY + cameraPosZ));
        Vector3 spawnPosDown = new Vector3(Random.Range(-cameraSizeX, cameraSizeX), 0f, -cameraSizeY * 1.1f + cameraPosZ);
        Vector3 spawnPosLeft = new Vector3(-cameraSizeX * 1.2f, 0f, Random.Range(-cameraSizeY + cameraPosZ, cameraSizeY + cameraPosZ));

        float getPositionRandom = Random.value;
        Component newObject = null;

        if (getPositionRandom < 0.25f) newObject = poolManager.Spawn(objectType, spawnPosUp, Quaternion.LookRotation(spawnPosDown));
        else if (getPositionRandom < 0.5f) newObject = poolManager.Spawn(objectType, spawnPosRight, Quaternion.LookRotation(spawnPosLeft));
        else if (getPositionRandom < 0.75f) newObject = poolManager.Spawn(objectType, spawnPosDown, Quaternion.LookRotation(spawnPosUp));
        else newObject = poolManager.Spawn(objectType, spawnPosLeft, Quaternion.LookRotation(spawnPosRight));

        return newObject;
    }
}
