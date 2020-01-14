using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PoolManager : MonoBehaviour
{
    public enum ObjectPool
    {
        EnemyTP1,
        EnemyTS1,
        ParticleTP1,
        ParticleTS1
    }
    //public ObjectPool spawnObject;
    
    [SerializeField] GameObject enemyToPlanet1;
    [SerializeField] GameObject enemyToStar1;
    [SerializeField] GameObject particleTP1;
    [SerializeField] GameObject particleTS1;

    private List<GameObject> enemyTP1List = new List<GameObject>();
    private List<GameObject> enemyTS1List = new List<GameObject>();
    private List<GameObject> particleTP1List = new List<GameObject>();
    private List<GameObject> particleTS1List = new List<GameObject>();

    [SerializeField] Transform enemyTP1Group;
    [SerializeField] Transform enemyTS1Group;
    [SerializeField] Transform particleTP1Group;
    [SerializeField] Transform particleTS1Group;

    private int enemyTP1Index = 0;
    private int enemyTS1Index = 0;
    private int particleTP1Index = 0;
    private int particleTS1Index = 0;

    [SerializeField] int poolNumber = 100;

    void Awake()
    {
        enemyTP1List = MakeObjectPool(enemyToPlanet1, enemyTP1Group);
        enemyTS1List = MakeObjectPool(enemyToStar1, enemyTS1Group);
        particleTP1List = MakeObjectPool(particleTP1, particleTP1Group);
        particleTS1List = MakeObjectPool(particleTS1, particleTS1Group);
    }
    /// <summary>
    /// 오브젝트를 비활성화 상태로 생성하고 리스트에 넣은 후, 해당 리스트를 반환합니다. 
    /// </summary>
    /// <param name="gameObject">대상 오브젝트</param>
    /// <param name="group">생성할 그룹</param>
    /// <param name="poolNumber">오브젝트 생성 개수(기본값 = 100)</param>
    /// <returns></returns>
    List<GameObject> MakeObjectPool(GameObject gameObject, Transform group, int poolNumber = 100)
    {
        List<GameObject> _objectList = new List<GameObject>();

        for (int i = 0; i < poolNumber; i++)
        {
            GameObject _instance = Instantiate(gameObject, group);
            _objectList.Add(_instance);
            _instance.SetActive(false);
        }
        Debug.Log("오브젝트풀 생성 완료");
        return _objectList;
    }

    GameObject SpawnObject(List<GameObject> objectList, int index)
    {
        GameObject _gameObject = objectList[index];
        _gameObject.SetActive(true);

        return _gameObject;
    }

    public void Spawn(ObjectPool type, Vector3 position, Quaternion rotation)
    {
        GameObject _spawnedObject;
        switch (type)
        {
            case ObjectPool.EnemyTP1:
                _spawnedObject = SpawnObject(enemyTP1List, enemyTP1Index);
                enemyTP1Index = (enemyTP1Index + 1) % poolNumber;
                break;
            case ObjectPool.EnemyTS1:
                _spawnedObject = SpawnObject(enemyTS1List, enemyTS1Index);
                enemyTS1Index = (enemyTS1Index + 1) % poolNumber;
                break;
            case ObjectPool.ParticleTP1:
                _spawnedObject = SpawnObject(particleTP1List, particleTP1Index);
                particleTP1Index = (particleTP1Index + 1) % poolNumber;
                break;
            case ObjectPool.ParticleTS1:
                _spawnedObject = SpawnObject(particleTS1List, particleTS1Index);
                particleTS1Index = (particleTS1Index + 1) % poolNumber;
                break;
            default:
                return;
        }

        _spawnedObject.transform.position = position;
        _spawnedObject.transform.rotation = rotation;
    }

    public void Spawn(ObjectPool type, Vector3 position)
    {
        GameObject _spawnedObject;
        switch (type)
        {
            case ObjectPool.EnemyTP1:
                _spawnedObject = SpawnObject(enemyTP1List, enemyTP1Index);
                enemyTP1Index = (enemyTP1Index + 1) % poolNumber;
                break;
            case ObjectPool.EnemyTS1:
                _spawnedObject = SpawnObject(enemyTS1List, enemyTS1Index);
                enemyTS1Index = (enemyTS1Index + 1) % poolNumber;
                break;
            case ObjectPool.ParticleTP1:
                _spawnedObject = SpawnObject(particleTP1List, particleTP1Index);
                particleTP1Index = (particleTP1Index + 1) % poolNumber;
                break;
            case ObjectPool.ParticleTS1:
                _spawnedObject = SpawnObject(particleTS1List, particleTS1Index);
                particleTS1Index = (particleTS1Index + 1) % poolNumber;
                break;
            default:
                return;
        }

        _spawnedObject.transform.position = position;
    }
}
