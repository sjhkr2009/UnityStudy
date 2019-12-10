using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnManager : MonoBehaviour
{
    [TabGroup("Prefab List")] [SerializeField] GameObject playerBullet;
    [TabGroup("Prefab List")] [SerializeField] GameObject playerHomingBullet;
    //[TabGroup("Prefab List")] [SerializeField] GameObject enemy1;

    List<GameObject> playerBulletList = new List<GameObject>();
    List<GameObject> playerHomingBulletList = new List<GameObject>();

    int playerBulletIndex = 0;
    int playerHomingBulletIndex = 0;

    [BoxGroup("Pool Amount")] [SerializeField] int objectPoolSmall = 30;
    [BoxGroup("Pool Amount")] [SerializeField] int objectPoolNormal = 100;
    [BoxGroup("Pool Amount")] [SerializeField] int objectPoolBig = 500;

    [TabGroup("Group")] [SerializeField] Transform playerBulletGroup;
    [TabGroup("Group")] [SerializeField] Transform playerHomingBulletGroup;

    [Button("Make Objects")]
    void Awake()
    {
        playerBulletList = MakeObjectPool(playerBulletList, playerBullet, playerBulletGroup, objectPoolNormal);
        playerHomingBulletList = MakeObjectPool(playerHomingBulletList, playerHomingBullet, playerHomingBulletGroup, objectPoolNormal);
    }

    List<GameObject> MakeObjectPool(List<GameObject> objectList, GameObject prefab, Transform group, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ObjectInstance(objectList, prefab, group);
        }
        return objectList;
    }

    void ObjectInstance(List<GameObject> objectList, GameObject prefab, Transform group)
    {
        objectList.Add(Instantiate(prefab, group));
        objectList[objectList.Count - 1].SetActive(false);
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
        //리스트의 모든 오브젝트가 활성화 상태이면, 추가로 생성하기
        if (playerBulletList[playerBulletIndex].activeSelf)
        {
            ObjectInstance(playerBulletList, playerBullet, playerBulletGroup);
        }
        
        playerBulletIndex = Spawn(playerBulletList, position, rotation, playerBulletIndex, playerBulletList.Count);
    }

    public void SpawnPlayerHomingBullet(Vector3 position, Quaternion rotation)
    {
        if (playerHomingBulletList[playerHomingBulletIndex].activeSelf)
        {
            ObjectInstance(playerHomingBulletList, playerHomingBullet, playerHomingBulletGroup);
        }

        playerHomingBulletIndex = Spawn(playerHomingBulletList, position, rotation, playerHomingBulletIndex, playerHomingBulletList.Count);
    }
}
