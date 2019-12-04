using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnManager : MonoBehaviour
{
    [TabGroup("Prefab List")] [SerializeField] GameObject playerBullet;
    //[TabGroup("Prefab List")] [SerializeField] GameObject enemy1;

    List<GameObject> playerBulletList = new List<GameObject>();

    int playerBulletIndex = 0;

    [BoxGroup("Pool Amount")] [SerializeField] int objectPoolSmall = 30;
    [BoxGroup("Pool Amount")] [SerializeField] int objectPoolNormal = 100;
    [BoxGroup("Pool Amount")] [SerializeField] int objectPoolBig = 500;

    [TabGroup("Group")] [SerializeField] Transform playerBulletGroup;

    [Button("Make Objects")]
    void Awake()
    {
        playerBulletList = ObjectInstance(playerBulletList, playerBullet, playerBulletGroup);
    }

    List<GameObject> ObjectInstance(List<GameObject> objectList, GameObject prefab, Transform group)
    {
        for (int i = 0; i < objectPoolNormal; i++)
        {
            objectList.Add(Instantiate(prefab, group));
            objectList[objectList.Count - 1].SetActive(false);
        }
        return objectList;
    }

    int Spawn(List<GameObject> objectList, Vector3 position, Quaternion rotation, int index, int poolAmount)
    {
        objectList[index].transform.position = position;
        objectList[index].transform.rotation = rotation;
        objectList[index].SetActive(true);
        index++;
        index %= poolAmount;
        return index;

    }

    [Button("Spawn Bullet")]
    public void SpawnPlayerBullet(Vector3 position, Quaternion rotation)
    {
        playerBulletIndex = Spawn(playerBulletList, position, rotation, playerBulletIndex, objectPoolNormal);
    }
}
