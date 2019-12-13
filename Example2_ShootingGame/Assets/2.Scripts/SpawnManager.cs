using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnManager : MonoBehaviour
{
    [TabGroup("Prefab List")] [SerializeField] GameObject playerBullet;
    [TabGroup("Prefab List")] [SerializeField] GameObject playerHomingBullet;
    [TabGroup("Prefab List")] [SerializeField] GameObject enemyBullet;
    [TabGroup("Prefab List")] [SerializeField] GameObject enemy1;
    [TabGroup("Prefab List")] [SerializeField] GameObject enemy2;

    List<GameObject> playerBulletList = new List<GameObject>();
    List<GameObject> playerHomingBulletList = new List<GameObject>();
    List<GameObject> enemyBulletList = new List<GameObject>();
    List<GameObject> enemy1List = new List<GameObject>();
    List<GameObject> enemy2List = new List<GameObject>();

    public List<GameObject> allEnemyList = new List<GameObject>();

    int playerBulletIndex = 0;
    int playerHomingBulletIndex = 0;
    int enemyBulletIndex = 0;
    int enemy1Index = 0;
    int enemy2Index = 0;

    [TabGroup("Group")] [SerializeField] Transform playerBulletGroup;
    [TabGroup("Group")] [SerializeField] Transform playerHomingBulletGroup;
    [TabGroup("Group")] [SerializeField] Transform enemyBulletGroup;
    [TabGroup("Group")] [SerializeField] Transform enemy1Group;
    [TabGroup("Group")] [SerializeField] Transform enemy2Group;

    [BoxGroup("Pool Amount")] [SerializeField] int smallPool = 30;
    [BoxGroup("Pool Amount")] [SerializeField] int normalPool = 100;
    [BoxGroup("Pool Amount")] [SerializeField] int bigPool = 500;

    void Awake()
    {
        playerBulletList = MakeObjectPool(playerBulletList, playerBullet, playerBulletGroup, normalPool);
        playerHomingBulletList = MakeObjectPool(playerHomingBulletList, playerHomingBullet, playerHomingBulletGroup, normalPool);
        enemyBulletList = MakeObjectPool(enemyBulletList, enemyBullet, enemyBulletGroup, normalPool);
        enemy1List = MakeObjectPool(enemy1List, enemy1, enemy1Group, normalPool);
        enemy2List = MakeObjectPool(enemy2List, enemy2, enemy2Group, normalPool);

        MakeTargetingList(enemy1List);
        MakeTargetingList(enemy2List);
    }

    void MakeTargetingList(List<GameObject> objectList)
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            allEnemyList.Add(objectList[i]);
        }

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

    int Spawn(List<GameObject> objectList, Vector3 position, Quaternion rotation, int index, int poolListAmount)
    {
        objectList[index].transform.position = position;
        objectList[index].transform.rotation = rotation;
        objectList[index].SetActive(true);
        index++;
        index %= poolListAmount;
        return index;

    }

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

    public void SpawnEnemyBullet(Vector3 position, Quaternion rotation)
    {
        if (enemyBulletList[enemyBulletIndex].activeSelf)
        {
            ObjectInstance(enemyBulletList, enemyBullet, enemyBulletGroup);
        }

        enemyBulletIndex = Spawn(enemyBulletList, position, rotation, enemyBulletIndex, playerHomingBulletList.Count);
    }

    public void SpawnEnemy1(Vector3 position)
    {
        if (enemy1List[enemy1Index].activeSelf)
        {
            ObjectInstance(enemy1List, enemy1, enemy1Group);
            allEnemyList.Add(enemy1List[enemy1List.Count-1]);
        }
        Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
        enemy1Index = Spawn(enemy1List, position, rotation, enemy1Index, enemy1List.Count);
    }

    public void SpawnEnemy2(Vector3 position, float rotation)
    {
        if (enemy2List[enemy1Index].activeSelf)
        {
            ObjectInstance(enemy2List, enemy2, enemy2Group);
            allEnemyList.Add(enemy2List[enemy2List.Count - 1]);
        }
        Quaternion rot = Quaternion.Euler(0f, rotation, 0f);
        enemy2Index = Spawn(enemy2List, position, rot, enemy2Index, enemy2List.Count);
    }
}
