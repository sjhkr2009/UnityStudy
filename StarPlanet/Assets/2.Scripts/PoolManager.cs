using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ObjectPool
    {
        EnemyTP1,
        EnemyTP2,
        EnemyTS1,
        EnemyTS2,
        ParticleTP1,
        ParticleTS1,
        AudioFX
    }

public class PoolManager : MonoBehaviour
{
    
    [SerializeField] GameObject enemyToPlanet1;
    [SerializeField] GameObject enemyToStar1;
    [SerializeField] GameObject enemyToPlanet2;
    [SerializeField] GameObject enemyToStar2;
    [SerializeField] GameObject particleTP1;
    [SerializeField] GameObject particleTS1;
    [SerializeField] GameObject audioFX;

    private List<Enemy> enemyTP1List = new List<Enemy>();
    private List<Enemy> enemyTS1List = new List<Enemy>();
    private List<Enemy> enemyTP2List = new List<Enemy>();
    private List<Enemy> enemyTS2List = new List<Enemy>();
    private List<ParticleSystem> particleTP1List = new List<ParticleSystem>();
    private List<ParticleSystem> particleTS1List = new List<ParticleSystem>();
    [HideInInspector] public List<AudioSource> audioFXList = new List<AudioSource>();

    [SerializeField] Transform enemyTP1Group;
    [SerializeField] Transform enemyTS1Group;
    [SerializeField] Transform enemyTP2Group;
    [SerializeField] Transform enemyTS2Group;
    [SerializeField] Transform particleTP1Group;
    [SerializeField] Transform particleTS1Group;
    [SerializeField] Transform audioFXGroup;

    private int enemyTP1Index = 0;
    private int enemyTS1Index = 0;
    private int enemyTP2Index = 0;
    private int enemyTS2Index = 0;
    private int particleTP1Index = 0;
    private int particleTS1Index = 0;

    [SerializeField] int poolNumber = 100;

    void Awake()
    {
        enemyTP1List = MakeObjectPool<Enemy>(enemyToPlanet1, enemyTP1Group);
        enemyTS1List = MakeObjectPool<Enemy>(enemyToStar1, enemyTS1Group);
        enemyTP2List = MakeObjectPool<Enemy>(enemyToPlanet2, enemyTP2Group, 30);
        enemyTS2List = MakeObjectPool<Enemy>(enemyToStar2, enemyTS2Group, 30);
        particleTP1List = MakeObjectPool<ParticleSystem>(particleTP1, particleTP1Group);
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

    /// <summary>
    /// 오브젝트를 생성하고 컴포넌트를 반환합니다. 원하는 클래스로 캐스팅해서 사용할 수 있습니다.
    /// poolNumber(100개)와 다른 개수로 생성된 오브젝트를 추가할 경우 인덱스 값의 재설정에 유의하세요.
    /// </summary>
    /// <param name="type">생성할 오브젝트의 타입</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="rotation">생성된 오브젝트의 회전값</param>
    /// <returns></returns>
    public Component Spawn(ObjectPool type, Vector3 position, Quaternion rotation)
    {
        Component _returnObject = null;

        switch (type)
        {
            case ObjectPool.EnemyTP1:
                if (enemyTP1List[enemyTP1Index].gameObject.activeSelf)
                {
                    _returnObject = Instantiate(enemyToPlanet1, position, rotation).GetComponent<Enemy>();
                    break;
                }
                SpawnObject(enemyTP1List, enemyTP1Index, position, rotation);
                _returnObject = enemyTP1List[enemyTP1Index];
                enemyTP1Index = (enemyTP1Index + 1) % poolNumber;
                break;

            case ObjectPool.EnemyTS1:
                if (enemyTS1List[enemyTS1Index].gameObject.activeSelf)
                {
                    _returnObject = Instantiate(enemyToStar1, position, rotation).GetComponent<Enemy>();
                    break;
                }
                SpawnObject(enemyTS1List, enemyTS1Index, position, rotation);
                _returnObject = enemyTS1List[enemyTS1Index];
                enemyTS1Index = (enemyTS1Index + 1) % poolNumber;
                break;

            case ObjectPool.EnemyTP2:
                if (enemyTP2List[enemyTP2Index].gameObject.activeSelf)
                {
                    _returnObject = Instantiate(enemyToPlanet2, position, rotation).GetComponent<Enemy>();
                    break;
                }
                SpawnObject(enemyTP2List, enemyTP2Index, position, rotation);
                _returnObject = enemyTP2List[enemyTP2Index];
                enemyTP2Index = (enemyTP2Index + 1) % 30;
                break;

            case ObjectPool.EnemyTS2:
                if (enemyTS2List[enemyTS2Index].gameObject.activeSelf)
                {
                    _returnObject = Instantiate(enemyToStar2, position, rotation).GetComponent<Enemy>();
                    break;
                }
                SpawnObject(enemyTS2List, enemyTS2Index, position, rotation);
                _returnObject = enemyTS2List[enemyTS2Index];
                enemyTS2Index = (enemyTS2Index + 1) % 30;
                break;

            case ObjectPool.ParticleTP1:
                if (particleTP1List[particleTP1Index].gameObject.activeSelf)
                {
                    _returnObject = Instantiate(particleTP1, position, rotation).GetComponent<Enemy>();
                    break;
                }
                SpawnObject(particleTP1List, particleTP1Index, position, rotation);
                _returnObject = particleTP1List[particleTP1Index];
                particleTP1Index = (particleTP1Index + 1) % poolNumber;
                break;

            case ObjectPool.ParticleTS1:
                if (particleTS1List[particleTS1Index].gameObject.activeSelf)
                {
                    _returnObject = Instantiate(particleTS1, position, rotation).GetComponent<Enemy>();
                    break;
                }
                SpawnObject(particleTS1List, particleTS1Index, position, rotation);
                _returnObject = particleTS1List[particleTS1Index];
                particleTS1Index = (particleTS1Index + 1) % poolNumber;
                break;

            default:
                return null;
        }
        return _returnObject;
    }
}
