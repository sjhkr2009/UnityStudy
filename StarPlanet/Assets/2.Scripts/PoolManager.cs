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
    ItemHexagonBomb,
    ItemFixedBomb,
    ItemHeal,
    ParticleTP1,
    ParticleTS1,
    ParticleExplosion,
    ParticleHexagonExp,
    ParticleHealing,
    AudioFX
}

public class PoolManager : MonoBehaviour
{
    
    [SerializeField] GameObject enemyToPlanet1;
    [SerializeField] GameObject enemyToStar1;
    [SerializeField] GameObject enemyToPlanet2;
    [SerializeField] GameObject enemyToStar2;
    [SerializeField] GameObject itemHexagonBomb;
    [SerializeField] GameObject itemFixedBomb;
    [SerializeField] GameObject itemHeal;
    [SerializeField] GameObject particleTP1;
    [SerializeField] GameObject particleTS1;
    [SerializeField] GameObject particleExplosion;
    [SerializeField] GameObject particleHexagonExp;
    [SerializeField] GameObject particleHealing;
    [SerializeField] GameObject audioFX;

    private List<Enemy> enemyTP1List = new List<Enemy>();
    private List<Enemy> enemyTS1List = new List<Enemy>();
    private List<Enemy> enemyTP2List = new List<Enemy>();
    private List<Enemy> enemyTS2List = new List<Enemy>();
    private List<ItemBomb> itemHexagonBombList = new List<ItemBomb>();
    private List<ItemBomb> itemFixedBombList = new List<ItemBomb>();
    private List<ItemHeal> itemHealList = new List<ItemHeal>();
    private List<ParticleSystem> particleTP1List = new List<ParticleSystem>();
    private List<ParticleSystem> particleTS1List = new List<ParticleSystem>();
    private List<Explosion> particleExplosionList = new List<Explosion>();
    private List<Explosion> particleHexagonExpList = new List<Explosion>();
    private List<ParticleSystem> particleHealingList = new List<ParticleSystem>();
    [HideInInspector] public List<AudioSource> audioFXList = new List<AudioSource>();

    [SerializeField] Transform enemyTP1Group;
    [SerializeField] Transform enemyTS1Group;
    [SerializeField] Transform enemyTP2Group;
    [SerializeField] Transform enemyTS2Group;
    [SerializeField] Transform itemHexagonBombGroup;
    [SerializeField] Transform itemFixedBombGroup;
    [SerializeField] Transform itemHealGroup;
    [SerializeField] Transform particleTP1Group;
    [SerializeField] Transform particleTS1Group;
    [SerializeField] Transform particleExplosionGroup;
    [SerializeField] Transform particleHexagonExpGroup;
    [SerializeField] Transform particleHealingGroup;
    [SerializeField] Transform audioFXGroup;

    private int enemyTP1Index = 0;
    private int enemyTS1Index = 0;
    private int enemyTP2Index = 0;
    private int enemyTS2Index = 0;
    private int itemHexagonBombIndex = 0;
    private int itemFixedBombIndex = 0;
    private int itemHealIndex = 0;
    private int particleTP1Index = 0;
    private int particleTS1Index = 0;
    private int particleExplosionIndex = 0;
    private int particleHexagonExpIndex = 0;
    private int particleHealingIndex = 0;

    void Awake()
    {
        enemyTP1List = MakeObjectPool<Enemy>(enemyToPlanet1, enemyTP1Group);
        enemyTS1List = MakeObjectPool<Enemy>(enemyToStar1, enemyTS1Group);
        enemyTP2List = MakeObjectPool<Enemy>(enemyToPlanet2, enemyTP2Group, 30);
        enemyTS2List = MakeObjectPool<Enemy>(enemyToStar2, enemyTS2Group, 30);
        itemFixedBombList = MakeObjectPool<ItemBomb>(itemFixedBomb, itemFixedBombGroup, 50);
        itemHexagonBombList = MakeObjectPool<ItemBomb>(itemHexagonBomb, itemHexagonBombGroup, 30);
        itemHealList = MakeObjectPool<ItemHeal>(itemHeal, itemHealGroup, 30);
        particleTP1List = MakeObjectPool<ParticleSystem>(particleTP1, particleTP1Group);
        particleTS1List = MakeObjectPool<ParticleSystem>(particleTS1, particleTS1Group);
        particleExplosionList = MakeObjectPool<Explosion>(particleExplosion, particleExplosionGroup, 50);
        particleHexagonExpList = MakeObjectPool<Explosion>(particleHexagonExp, particleHexagonExpGroup, 20);
        particleHealingList = MakeObjectPool<ParticleSystem>(particleHealing, particleHealingGroup, 30);
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
            //Destroy Particle - EnemyTP1
            case ObjectPool.ParticleTP1:
                if (particleTP1List[particleTP1Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(particleTP1, position, rotation).GetComponent<ParticleSystem>();
                    _returnObject = newItem;
                    particleTP1List.Add(newItem);
                    break;
                }
                SpawnObject(particleTP1List, particleTP1Index, position, rotation);
                _returnObject = particleTP1List[particleTP1Index];
                particleTP1Index = (particleTP1Index + 1) % particleTP1List.Count;
                break;

            //Destroy Particle - EnemyTS1
            case ObjectPool.ParticleTS1:
                if (particleTS1List[particleTS1Index].gameObject.activeSelf)
                {
                    var newItem = Instantiate(particleTS1, position, rotation).GetComponent<ParticleSystem>();
                    _returnObject = newItem;
                    particleTS1List.Add(newItem);
                    break;
                }
                SpawnObject(particleTS1List, particleTS1Index, position, rotation);
                _returnObject = particleTS1List[particleTS1Index];
                particleTS1Index = (particleTS1Index + 1) % particleTS1List.Count;
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

            default:
                return null;
        }
        return _returnObject;
    }
}
