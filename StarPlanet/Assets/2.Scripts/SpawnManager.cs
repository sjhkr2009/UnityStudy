using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    float screenView;
    float cameraSizeX;
    float cameraSizeY;

    PoolManager poolManager;

    void Start()
    {
        poolManager = GetComponent<PoolManager>();
        screenView = GameManager.Instance.screenHorizontal / GameManager.Instance.screenVertical;
        cameraSizeX = Camera.main.orthographicSize * screenView;
        cameraSizeY = Camera.main.orthographicSize;
    }

    public Component SpawnOverMapToCenter(ObjectPool objectType)
    {
        float minX = -cameraSizeX * 1.5f;
        float maxX = cameraSizeX * 1.5f;
        float posX = Random.Range(minX, maxX);

        float posY = cameraSizeY + 1f;
        if (Random.value < 0.5f) posY = cameraSizeY - 1f;

        Vector3 spawnPos = new Vector3(posX, 0f, posY);

        Component newObject = null;
        newObject = poolManager.Spawn(objectType, spawnPos, Quaternion.LookRotation(Vector3.zero - spawnPos));
        return newObject;
    }

    public Component SpawnOnMap(ObjectPool objectType)
    {
        Vector3 spawnPos = Vector3.zero;
        while (Vector3.Distance(spawnPos, Vector3.zero) < 2.5f)
        {
            spawnPos = new Vector3(UnityEngine.Random.Range(-cameraSizeX * 0.95f, cameraSizeX * 0.95f), 0f, UnityEngine.Random.Range(-cameraSizeY * 0.95f, cameraSizeY * 0.95f));
        }
        Component newObject = poolManager.Spawn(objectType, spawnPos, Quaternion.identity);
        return newObject;
    }
    public Component SpawnOverMapToRandom(ObjectPool objectType)
    {
        Vector3 spawnPosUp = new Vector3(Random.Range(-cameraSizeX, cameraSizeX), 0f, cameraSizeY + 1f);
        Vector3 spawnPosRight = new Vector3(cameraSizeX + 1f, 0f, Random.Range(-cameraSizeY, cameraSizeY));
        Vector3 spawnPosDown = new Vector3(Random.Range(-cameraSizeX, cameraSizeX), 0f, -cameraSizeY - 1f);
        Vector3 spawnPosLeft = new Vector3(-cameraSizeX - 1f, 0f, Random.Range(-cameraSizeY, cameraSizeY));

        float getPositionRandom = Random.value;
        Component newObject = null;

        if (getPositionRandom < 0.25f) newObject = poolManager.Spawn(objectType, spawnPosUp, Quaternion.LookRotation(spawnPosDown));
        else if (getPositionRandom < 0.5f) newObject = poolManager.Spawn(objectType, spawnPosRight, Quaternion.LookRotation(spawnPosLeft));
        else if (getPositionRandom < 0.75f) newObject = poolManager.Spawn(objectType, spawnPosDown, Quaternion.LookRotation(spawnPosUp));
        else newObject = poolManager.Spawn(objectType, spawnPosLeft, Quaternion.LookRotation(spawnPosRight));

        return newObject;
    }
}
