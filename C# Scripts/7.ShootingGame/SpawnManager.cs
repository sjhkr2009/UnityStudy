using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnManager : MonoBehaviour
{
    [TabGroup("Prefab List")] [SerializeField] GameObject playerBullet;
    [TabGroup("Prefab List")] [SerializeField] GameObject playerHomingBullet;
    [TabGroup("Prefab List")] [SerializeField] GameObject fairyBullet;
    [TabGroup("Prefab List")] [SerializeField] GameObject enemyBullet;
    [TabGroup("Prefab List")] [SerializeField] GameObject enemy1;
    [TabGroup("Prefab List")] [SerializeField] GameObject enemy2;
    [TabGroup("Prefab List")] [SerializeField] GameObject enemy3;
    [TabGroup("Prefab List")] [SerializeField] GameObject enemy4;
    [TabGroup("Prefab List")] [SerializeField] GameObject enemy4m;

    List<GameObject> playerBulletList = new List<GameObject>();
    List<GameObject> playerHomingBulletList = new List<GameObject>();
    List<GameObject> fairyBulletList = new List<GameObject>();
    List<GameObject> enemyBulletList = new List<GameObject>();
    List<GameObject> enemy1List = new List<GameObject>();
    List<GameObject> enemy2List = new List<GameObject>();
    List<GameObject> enemy3List = new List<GameObject>();
    List<GameObject> enemy4List = new List<GameObject>();
    List<GameObject> enemy4mList = new List<GameObject>();

    public List<GameObject> allEnemyList = new List<GameObject>();

    int playerBulletIndex = 0;
    int playerHomingBulletIndex = 0;
    int fairyBulletIndex = 0;
    int enemyBulletIndex = 0;
    int enemy1Index = 0;
    int enemy2Index = 0;
    int enemy3Index = 0;
    int enemy4Index = 0;
    int enemy4mIndex = 0;

    [TabGroup("Group")] [SerializeField] Transform playerBulletGroup;
    [TabGroup("Group")] [SerializeField] Transform playerHomingBulletGroup;
    [TabGroup("Group")] [SerializeField] Transform fairyBulletGroup;
    [TabGroup("Group")] [SerializeField] Transform enemyBulletGroup;
    [TabGroup("Group")] [SerializeField] Transform enemy1Group;
    [TabGroup("Group")] [SerializeField] Transform enemy2Group;
    [TabGroup("Group")] [SerializeField] Transform enemy3Group;
    [TabGroup("Group")] [SerializeField] Transform enemy4Group;
    [TabGroup("Group")] [SerializeField] Transform enemy4mGroup;

    [BoxGroup("Pool Amount")] [SerializeField] int smallPool = 30;
    [BoxGroup("Pool Amount")] [SerializeField] int normalPool = 100;
    [BoxGroup("Pool Amount")] [SerializeField] int bigPool = 400;

    void Awake()
    {
        playerBulletList = MakeObjectPool(playerBulletList, playerBullet, playerBulletGroup, normalPool);
        playerHomingBulletList = MakeObjectPool(playerHomingBulletList, playerHomingBullet, playerHomingBulletGroup, normalPool);
        fairyBulletList = MakeObjectPool(fairyBulletList, fairyBullet, fairyBulletGroup, smallPool);
        enemyBulletList = MakeObjectPool(enemyBulletList, enemyBullet, enemyBulletGroup, bigPool);
        enemy1List = MakeObjectPool(enemy1List, enemy1, enemy1Group, normalPool);
        enemy2List = MakeObjectPool(enemy2List, enemy2, enemy2Group, smallPool);
        enemy3List = MakeObjectPool(enemy3List, enemy3, enemy3Group, normalPool);
        enemy4List = MakeObjectPool(enemy4List, enemy4, enemy4Group, normalPool);
        enemy4mList = MakeObjectPool(enemy4mList, enemy4m, enemy4mGroup, normalPool);

        MakeTargetingList(enemy1List);
        MakeTargetingList(enemy2List);
        MakeTargetingList(enemy3List);
        MakeTargetingList(enemy4List);
        MakeTargetingList(enemy4mList);
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

    public void SpawnFairyBullet(Vector3 position, Quaternion rotation)
    {
        if (fairyBulletList[fairyBulletIndex].activeSelf)
        {
            ObjectInstance(fairyBulletList, fairyBullet, fairyBulletGroup);
        }

        fairyBulletIndex = Spawn(fairyBulletList, position, rotation, fairyBulletIndex, fairyBulletList.Count);
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

    public void SpawnEnemy3(Vector3 position)
    {
        if (enemy3List[enemy3Index].activeSelf)
        {
            ObjectInstance(enemy3List, enemy3, enemy3Group);
            allEnemyList.Add(enemy3List[enemy3List.Count - 1]);
        }
        Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
        enemy3Index = Spawn(enemy3List, position, rotation, enemy3Index, enemy3List.Count);
    }

    public void SpawnEnemy4(Vector3 position)
    {
        if (enemy4List[enemy4Index].activeSelf)
        {
            ObjectInstance(enemy4List, enemy4, enemy4Group);
            allEnemyList.Add(enemy4List[enemy4List.Count - 1]);
        }
        Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
        enemy4Index = Spawn(enemy4List, position, rotation, enemy4Index, enemy4List.Count);
    }

    public void SpawnEnemy4mini(Vector3 position)
    {
        if (enemy4mList[enemy4mIndex].activeSelf)
        {
            ObjectInstance(enemy4mList, enemy4m, enemy4mGroup);
            allEnemyList.Add(enemy4mList[enemy4mList.Count - 1]);
        }
        Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
        enemy4mIndex = Spawn(enemy4mList, position, rotation, enemy4mIndex, enemy4mList.Count);
    }
}
