using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ObjectPool
{
    EnemyTP1,
    EnemyTP2,
    EnemyTP3,
    EnemyTP4,
    EnemyTS1,
    EnemyTS2,
    EnemyTS3,
    EnemyTS4,
    ItemHexagonBomb,
    ItemFixedBomb,
    ItemHeal,
    ParticleTPSmall,
    ParticleTSSmall,
    ParticleTSMiddle,
    ParticleExplosion,
    ParticleHexagonExp,
    ParticleHealing,
    ParticleFever,
    AudioFX
}

public class PoolManager : MonoBehaviour
{
    [Header("Enemies")]
    [TabGroup("Objects")][SerializeField] GameObject enemyToPlanet1;
    [TabGroup("Objects")] [SerializeField] GameObject enemyToStar1;
    [TabGroup("Objects")] [SerializeField] GameObject enemyToPlanet2;
    [TabGroup("Objects")] [SerializeField] GameObject enemyToStar2;
    [TabGroup("Objects")] [SerializeField] GameObject enemyToPlanet3;
    [TabGroup("Objects")] [SerializeField] GameObject enemyToStar3;
    [TabGroup("Objects")] [SerializeField] GameObject enemyToPlanet4;
    [TabGroup("Objects")] [SerializeField] GameObject enemyToStar4;
    [Header("Items")]
    [TabGroup("Objects")] [SerializeField] GameObject itemHexagonBomb;
    [TabGroup("Objects")] [SerializeField] GameObject itemFixedBomb;
    [TabGroup("Objects")] [SerializeField] GameObject itemHeal;
    [Header("Particles")]
    [TabGroup("Objects")] [SerializeField] GameObject particleTPSmall;
    [TabGroup("Objects")] [SerializeField] GameObject particleTSSmall;
    [TabGroup("Objects")] [SerializeField] GameObject particleTSMiddle;
    [TabGroup("Objects")] [SerializeField] GameObject particleExplosion;
    [TabGroup("Objects")] [SerializeField] GameObject particleHexagonExp;
    [TabGroup("Objects")] [SerializeField] GameObject particleHealing;
    [TabGroup("Objects")] [SerializeField] GameObject particleFever;
    [Header("Others")]
    [TabGroup("Objects")] [SerializeField] GameObject audioFX;

    private List<Enemy> enemyTP1List = new List<Enemy>();
    private List<Enemy> enemyTS1List = new List<Enemy>();
    private List<Enemy> enemyTP2List = new List<Enemy>();
    private List<Enemy> enemyTS2List = new List<Enemy>();
    private List<Enemy> enemyTP3List = new List<Enemy>();
    private List<Enemy> enemyTS3List = new List<Enemy>();
    private List<Enemy> enemyTP4List = new List<Enemy>();
    private List<Enemy> enemyTS4List = new List<Enemy>();
    private List<ItemBomb> itemHexagonBombList = new List<ItemBomb>();
    private List<ItemBomb> itemFixedBombList = new List<ItemBomb>();
    private List<ItemHeal> itemHealList = new List<ItemHeal>();
    private List<ParticleSystem> particleTPSmallList = new List<ParticleSystem>();
    private List<ParticleSystem> particleTSSmallList = new List<ParticleSystem>();
    private List<ParticleSystem> particleTSMiddleList = new List<ParticleSystem>();
    private List<Explosion> particleExplosionList = new List<Explosion>();
    private List<Explosion> particleHexagonExpList = new List<Explosion>();
    private List<ParticleSystem> particleHealingList = new List<ParticleSystem>();
    private List<ParticleSystem> particleFeverList = new List<ParticleSystem>();
    [HideInInspector] public List<AudioSource> audioFXList = new List<AudioSource>();

    [Header("Enemies")]
    [TabGroup("Groups")] [SerializeField] Transform enemyTP1Group;
    [TabGroup("Groups")] [SerializeField] Transform enemyTS1Group;
    [TabGroup("Groups")] [SerializeField] Transform enemyTP2Group;
    [TabGroup("Groups")] [SerializeField] Transform enemyTS2Group;
    [TabGroup("Groups")] [SerializeField] Transform enemyTP3Group;
    [TabGroup("Groups")] [SerializeField] Transform enemyTS3Group;
    [TabGroup("Groups")] [SerializeField] Transform enemyTP4Group;
    [TabGroup("Groups")] [SerializeField] Transform enemyTS4Group;
    [Header("Items")]
    [TabGroup("Groups")] [SerializeField] Transform itemHexagonBombGroup;
    [TabGroup("Groups")] [SerializeField] Transform itemFixedBombGroup;
    [TabGroup("Groups")] [SerializeField] Transform itemHealGroup;
    [Header("Particles")]
    [TabGroup("Groups")] [SerializeField] Transform particleTPSmallGroup;
    [TabGroup("Groups")] [SerializeField] Transform particleTSSmallGroup;
    [TabGroup("Groups")] [SerializeField] Transform particleTSMiddleGroup;
    [TabGroup("Groups")] [SerializeField] Transform particleExplosionGroup;
    [TabGroup("Groups")] [SerializeField] Transform particleHexagonExpGroup;
    [TabGroup("Groups")] [SerializeField] Transform particleHealingGroup;
    [TabGroup("Groups")] [SerializeField] Transform particleFeverGroup;
    [Header("Others")]
    [TabGroup("Groups")] [SerializeField] Transform audioFXGroup;

    private int enemyTP1Index = 0;
    private int enemyTS1Index = 0;
    private int enemyTP2Index = 0;
    private int enemyTS2Index = 0;
    private int enemyTP3Index = 0;
    private int enemyTS3Index = 0;
    private int enemyTP4Index = 0;
    private int enemyTS4Index = 0;
    private int itemHexagonBombIndex = 0;
    private int itemFixedBombIndex = 0;
    private int itemHealIndex = 0;
    private int particleTPSmallIndex = 0;
    private int particleTSSmallIndex = 0;
    private int particleTSMiddleIndex = 0;
    private int particleExplosionIndex = 0;
    private int particleHexagonExpIndex = 0;
    private int particleHealingIndex = 0;
    private int particleFeverIndex = 0;

    void Awake()
    {
        enemyTP1List = MakeObjectPool<Enemy>(enemyToPlanet1, enemyTP1Group, 100);
        enemyTS1List = MakeObjectPool<Enemy>(enemyToStar1, enemyTS1Group);
        enemyTP2List = MakeObjectPool<Enemy>(enemyToPlanet2, enemyTP2Group, 30);
        enemyTS2List = MakeObjectPool<Enemy>(enemyToStar2, enemyTS2Group, 30);
        enemyTP3List = MakeObjectPool<Enemy>(enemyToPlanet3, enemyTP3Group, 20);
        enemyTS3List = MakeObjectPool<Enemy>(enemyToStar3, enemyTS3Group);
        enemyTP4List = MakeObjectPool<Enemy>(enemyToPlanet4, enemyTP4Group, 20);
        enemyTS4List = MakeObjectPool<Enemy>(enemyToStar4, enemyTS4Group, 30);

        itemFixedBombList = MakeObjectPool<ItemBomb>(itemFixedBomb, itemFixedBombGroup);
        itemHexagonBombList = MakeObjectPool<ItemBomb>(itemHexagonBomb, itemHexagonBombGroup, 30);
        itemHealList = MakeObjectPool<ItemHeal>(itemHeal, itemHealGroup, 30);

        particleTPSmallList = MakeObjectPool<ParticleSystem>(particleTPSmall, particleTPSmallGroup);
        particleTSSmallList = MakeObjectPool<ParticleSystem>(particleTSSmall, particleTSSmallGroup);
        particleTSMiddleList = MakeObjectPool<ParticleSystem>(particleTSMiddle, particleTSMiddleGroup, 30);
        particleExplosionList = MakeObjectPool<Explosion>(particleExplosion, particleExplosionGroup);
        particleHexagonExpList = MakeObjectPool<Explosion>(particleHexagonExp, particleHexagonExpGroup, 20);
        particleHealingList = MakeObjectPool<ParticleSystem>(particleHealing, particleHealingGroup, 30);
        particleFeverList = MakeObjectPool<ParticleSystem>(particleFever, particleFeverGroup, 100);

        audioFXList = MakeObjectPool<AudioSource>(audioFX, audioFXGroup, 20);
    }
    /// <summary>
    /// 오브젝트를 비활성화 상태로 생성하고 리스트에 넣은 후, 해당 리스트를 반환합니다. 
    /// </summary>
    /// <param name="gameObject">대상 오브젝트</param>
    /// <param name="group">생성할 그룹</param>
    /// <param name="poolNumber">오브젝트 생성 개수(기본값 = 50)</param>
    /// <returns></returns>
    List<T> MakeObjectPool<T> (GameObject gameObject, Transform group, int poolNumber = 50)
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
        //Enemy
            //Enemy To Planet - Tier 1
            case ObjectPool.EnemyTP1:
                if (enemyTP1List[enemyTP1Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(enemyToPlanet1, position, rotation).GetComponent<Enemy>();
                    _returnObject = newItem;
                    enemyTP1List.Add(newItem);
                    break;
                }
                SpawnObject(enemyTP1List, enemyTP1Index, position, rotation);
                _returnObject = enemyTP1List[enemyTP1Index];
                enemyTP1Index = (enemyTP1Index + 1) % enemyTP1List.Count;
                break;

            //Enemy To Star - Tier 1
            case ObjectPool.EnemyTS1:
                if (enemyTS1List[enemyTS1Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(enemyToStar1, position, rotation).GetComponent<Enemy>();
                    _returnObject = newItem;
                    enemyTS1List.Add(newItem);
                    break;
                }
                SpawnObject(enemyTS1List, enemyTS1Index, position, rotation);
                _returnObject = enemyTS1List[enemyTS1Index];
                enemyTS1Index = (enemyTS1Index + 1) % enemyTS1List.Count;
                break;

            //Enemy To Planet - Tier 2
            case ObjectPool.EnemyTP2:
                if (enemyTP2List[enemyTP2Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(enemyToPlanet2, position, rotation).GetComponent<Enemy>();
                    _returnObject = newItem;
                    enemyTP2List.Add(newItem);
                    break;
                }
                SpawnObject(enemyTP2List, enemyTP2Index, position, rotation);
                _returnObject = enemyTP2List[enemyTP2Index];
                enemyTP2Index = (enemyTP2Index + 1) % enemyTP2List.Count;
                break;

            //Enemy To Star - Tier 2
            case ObjectPool.EnemyTS2:
                if (enemyTS2List[enemyTS2Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(enemyToStar2, position, rotation).GetComponent<Enemy>();
                    _returnObject = newItem;
                    enemyTS2List.Add(newItem);
                    break;
                }
                SpawnObject(enemyTS2List, enemyTS2Index, position, rotation);
                _returnObject = enemyTS2List[enemyTS2Index];
                enemyTS2Index = (enemyTS2Index + 1) % enemyTS2List.Count;
                break;

            //Enemy To Planet - Tier 3
            case ObjectPool.EnemyTP3:
                if (enemyTP3List[enemyTP3Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(enemyToPlanet3, position, rotation).GetComponent<Enemy>();
                    _returnObject = newItem;
                    enemyTP3List.Add(newItem);
                    break;
                }
                SpawnObject(enemyTP3List, enemyTP3Index, position, rotation);
                _returnObject = enemyTP3List[enemyTP3Index];
                enemyTP3Index = (enemyTP3Index + 1) % enemyTP3List.Count;
                break;

            //Enemy To Star - Tier 3
            case ObjectPool.EnemyTS3:
                if (enemyTS3List[enemyTS3Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(enemyToStar3, position, rotation).GetComponent<Enemy>();
                    _returnObject = newItem;
                    enemyTS3List.Add(newItem);
                    break;
                }
                SpawnObject(enemyTS3List, enemyTS3Index, position, rotation);
                _returnObject = enemyTS3List[enemyTS3Index];
                enemyTS3Index = (enemyTS3Index + 1) % enemyTS3List.Count;
                break;

            //Enemy To Planet - Tier 4
            case ObjectPool.EnemyTP4:
                if (enemyTP4List[enemyTP4Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(enemyToPlanet4, position, rotation).GetComponent<Enemy>();
                    _returnObject = newItem;
                    enemyTP4List.Add(newItem);
                    break;
                }
                SpawnObject(enemyTP4List, enemyTP4Index, position, rotation);
                _returnObject = enemyTP4List[enemyTP4Index];
                enemyTP4Index = (enemyTP4Index + 1) % enemyTP4List.Count;
                break;

            //Enemy To Star - Tier 4
            case ObjectPool.EnemyTS4:
                if (enemyTS4List[enemyTS4Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(enemyToStar4, position, rotation).GetComponent<Enemy>();
                    _returnObject = newItem;
                    enemyTS4List.Add(newItem);
                    break;
                }
                SpawnObject(enemyTS4List, enemyTS4Index, position, rotation);
                _returnObject = enemyTS4List[enemyTS4Index];
                enemyTS4Index = (enemyTS4Index + 1) % enemyTS4List.Count;
                break;


            //Item
            //Hexagon Bomb
            case ObjectPool.ItemHexagonBomb:
                if (itemHexagonBombList[itemHexagonBombIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(itemHexagonBomb, position, rotation).GetComponent<ItemBomb>();
                    _returnObject = newItem;
                    itemHexagonBombList.Add(newItem);
                    break;
                }
                SpawnObject(itemHexagonBombList, itemHexagonBombIndex, position, rotation);
                _returnObject = itemHexagonBombList[itemHexagonBombIndex];
                itemHexagonBombIndex = (itemHexagonBombIndex + 1) % itemHexagonBombList.Count;
                break;

            //Fixed Bomb
            case ObjectPool.ItemFixedBomb:
                if (itemFixedBombList[itemFixedBombIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(itemFixedBomb, position, rotation).GetComponent<ItemBomb>();
                    _returnObject = newItem;
                    itemFixedBombList.Add(newItem);
                    break;
                }
                SpawnObject(itemFixedBombList, itemFixedBombIndex, position, rotation);
                _returnObject = itemFixedBombList[itemFixedBombIndex];
                itemFixedBombIndex = (itemFixedBombIndex + 1) % itemFixedBombList.Count;
                break;

            //Heal
            case ObjectPool.ItemHeal:
                if (itemHealList[itemHealIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(itemHeal, position, rotation).GetComponent<ItemHeal>();
                    _returnObject = newItem;
                    itemHealList.Add(newItem);
                    break;
                }
                SpawnObject(itemHealList, itemHealIndex, position, rotation);
                _returnObject = itemHealList[itemHealIndex];
                itemHealIndex = (itemHealIndex + 1) % itemHealList.Count;
                break;

        //Particle
            //Destroy Particle - EnemyTP Small
            case ObjectPool.ParticleTPSmall:
                if (particleTPSmallList[particleTPSmallIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(particleTPSmall, position, rotation).GetComponent<ParticleSystem>();
                    _returnObject = newItem;
                    particleTPSmallList.Add(newItem);
                    break;
                }
                SpawnObject(particleTPSmallList, particleTPSmallIndex, position, rotation);
                _returnObject = particleTPSmallList[particleTPSmallIndex];
                particleTPSmallIndex = (particleTPSmallIndex + 1) % particleTPSmallList.Count;
                break;

            //Destroy Particle - EnemyTS Small
            case ObjectPool.ParticleTSSmall:
                if (particleTSSmallList[particleTSSmallIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(particleTSSmall, position, rotation).GetComponent<ParticleSystem>();
                    _returnObject = newItem;
                    particleTSSmallList.Add(newItem);
                    break;
                }
                SpawnObject(particleTSSmallList, particleTSSmallIndex, position, rotation);
                _returnObject = particleTSSmallList[particleTSSmallIndex];
                particleTSSmallIndex = (particleTSSmallIndex + 1) % particleTSSmallList.Count;
                break;

            //Destroy Particle - EnemyTS Middle
            case ObjectPool.ParticleTSMiddle:
                if (particleTSMiddleList[particleTSMiddleIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(particleTSMiddle, position, rotation).GetComponent<ParticleSystem>();
                    _returnObject = newItem;
                    particleTSMiddleList.Add(newItem);
                    break;
                }
                SpawnObject(particleTSMiddleList, particleTSMiddleIndex, position, rotation);
                _returnObject = particleTSMiddleList[particleTSMiddleIndex];
                particleTSMiddleIndex = (particleTSMiddleIndex + 1) % particleTSMiddleList.Count;
                break;

            //Explosion Particle - Normal
            case ObjectPool.ParticleExplosion:
                if (particleExplosionList[particleExplosionIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(particleExplosion, position, rotation).GetComponent<Explosion>();
                    _returnObject = newItem;
                    particleExplosionList.Add(newItem);
                    break;
                }
                SpawnObject(particleExplosionList, particleExplosionIndex, position, rotation);
                _returnObject = particleExplosionList[particleExplosionIndex];
                particleExplosionIndex = (particleExplosionIndex + 1) % particleExplosionList.Count;
                break;

            //Explosion Particle - Hexagon
            case ObjectPool.ParticleHexagonExp:
                if (particleHexagonExpList[particleHexagonExpIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(particleHexagonExp, position, rotation).GetComponent<Explosion>();
                    _returnObject = newItem;
                    particleHexagonExpList.Add(newItem);
                    break;
                }
                SpawnObject(particleHexagonExpList, particleHexagonExpIndex, position, rotation);
                _returnObject = particleHexagonExpList[particleHexagonExpIndex];
                particleHexagonExpIndex = (particleHexagonExpIndex + 1) % particleHexagonExpList.Count;
                break;

            //Healing Particle
            case ObjectPool.ParticleHealing:
                if (particleHealingList[particleHealingIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(particleHealing, position, rotation).GetComponent<ParticleSystem>();
                    _returnObject = newItem;
                    particleHealingList.Add(newItem);
                    break;
                }
                SpawnObject(particleHealingList, particleHealingIndex, position, rotation);
                _returnObject = particleHealingList[particleHealingIndex];
                particleHealingIndex = (particleHealingIndex + 1) % particleHealingList.Count;
                break;

            //Fever Particle
            case ObjectPool.ParticleFever:
                if (particleFeverList[particleFeverIndex].gameObject.activeSelf)
                {
                    var newItem = Instantiate(particleFever, position, rotation).GetComponent<ParticleSystem>();
                    _returnObject = newItem;
                    particleFeverList.Add(newItem);
                    break;
                }
                SpawnObject(particleFeverList, particleFeverIndex, position, rotation);
                _returnObject = particleFeverList[particleFeverIndex];
                particleFeverIndex = (particleFeverIndex + 1) % particleFeverList.Count;
                break;

            default:
                return null;
        }
        return _returnObject;
    }
}
