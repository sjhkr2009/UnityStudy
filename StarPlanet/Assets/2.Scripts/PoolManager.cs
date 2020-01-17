using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ObjectPool
    {
        EnemyTP1,
        EnemyTS1,
        ParticleTP1,
        ParticleTS1,
        AudioFX
    }

public class PoolManager : MonoBehaviour
{
    
    [SerializeField] GameObject enemyToPlanet1;
    [SerializeField] GameObject enemyToStar1;
    [SerializeField] GameObject particleTP1;
    [SerializeField] GameObject particleTS1;
    [SerializeField] GameObject audioFX;

    private List<Enemy> enemyTP1List = new List<Enemy>();
    private List<Enemy> enemyTS1List = new List<Enemy>();
    private List<GameObject> particleTP1List = new List<GameObject>();
    private List<ParticleSystem> particleTS1List = new List<ParticleSystem>();
    [HideInInspector] public List<AudioSource> audioFXList = new List<AudioSource>();

    [SerializeField] Transform enemyTP1Group;
    [SerializeField] Transform enemyTS1Group;
    [SerializeField] Transform particleTP1Group;
    [SerializeField] Transform particleTS1Group;
    [SerializeField] Transform audioFXGroup;

    private int enemyTP1Index = 0;
    private int enemyTS1Index = 0;
    private int particleTP1Index = 0;
    private int particleTS1Index = 0;

    [SerializeField] int poolNumber = 100;

    void Awake()
    {
        enemyTP1List = MakeObjectPool<Enemy>(enemyToPlanet1, enemyTP1Group);
        enemyTS1List = MakeObjectPool<Enemy>(enemyToStar1, enemyTS1Group);
        particleTP1List = MakeObjectPool<GameObject>(particleTP1, particleTP1Group);
        particleTS1List = MakeObjectPool<ParticleSystem>(particleTS1, particleTS1Group);
        audioFXList = MakeObjectPool<AudioSource>(audioFX, audioFXGroup, 20);
    }
    /// <summary>
    /// 오브젝트를 비활성화 상태로 생성하고 리스트에 넣은 후, 해당 리스트를 반환합니다. 
    /// </summary>
    /// <param name="gameObject">대상 오브젝트</param>
    /// <param name="group">생성할 그룹</param>
    /// <param name="poolNumber">오브젝트 생성 개수(기본값 = 100)</param>
    /// <returns></returns>
    List<T> MakeObjectPool<T> (GameObject gameObject, Transform group, int poolNumber = 100)
    {
        List<T> _objectList = new List<T>();

        for (int i = 0; i < poolNumber; i++)
        {
            GameObject _instance = Instantiate(gameObject, group);
            _objectList.Add(_instance.GetComponent<T>());
            _instance.SetActive(false);
        }
        return _objectList;
    }

    void SpawnObject<T> (List<T> objectList, int index, Vector3 pos, Quaternion rot) where T : Component
    {
        GameObject _gameObject = objectList[index].gameObject;
        _gameObject.SetActive(true);
        _gameObject.transform.position = pos;
        _gameObject.transform.rotation = rot;
    }

    public MonoBehaviour Spawn<T>(ObjectPool type, Vector3 position, Quaternion rotation) where T : Component
    {
        MonoBehaviour _returnObject;

        switch (type)
        {
            case ObjectPool.EnemyTP1:
                SpawnObject(enemyTP1List, enemyTP1Index, position, rotation);
                _returnObject = enemyTP1List[enemyTP1Index];
                enemyTP1Index = (enemyTP1Index + 1) % poolNumber;
                break;
            case ObjectPool.EnemyTS1:
                SpawnObject(enemyTS1List, enemyTS1Index, position, rotation);
                _returnObject = enemyTS1List[enemyTS1Index];
                enemyTS1Index = (enemyTS1Index + 1) % poolNumber;
                break;
            case ObjectPool.ParticleTP1:
                SpawnObject(particleTP1List, particleTP1Index, position, rotation);
                _returnObject = particleTP1List[particleTP1Index];
                particleTP1Index = (particleTP1Index + 1) % poolNumber;
                break;
            case ObjectPool.ParticleTS1:
                SpawnObject(particleTS1List, particleTS1Index, position, rotation);
                particleTS1Index = (particleTS1Index + 1) % poolNumber;
                break;
            default:
                return null;
        }
        return _returnObject;
    }
}
