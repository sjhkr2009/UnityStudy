using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyToPlanet1;
    [SerializeField] GameObject enemyToStar1;

    private List<GameObject> enemyTP1List = new List<GameObject>();
    private List<GameObject> enemyTS1List = new List<GameObject>();

    [SerializeField] Transform enemyTP1Group;
    [SerializeField] Transform enemyTS1Group;

    private int enemyTP1Index = 0;
    private int enemyTS1Index = 0;

    [SerializeField] int poolNumber = 100;
    
    void Awake()
    {
        enemyTP1List = MakeObjectPool(enemyToPlanet1, enemyTP1Group);
        enemyTS1List = MakeObjectPool(enemyToStar1, enemyTS1Group);
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

    GameObject Spawn(List<GameObject> objectList, int index)
    {
        GameObject _gameObject = objectList[index];
        _gameObject.SetActive(true);

        return _gameObject;
    }

    public void EnemyTP1Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject _gameObject = Spawn(enemyTP1List, enemyTP1Index);
        _gameObject.transform.position = position;
        _gameObject.transform.rotation = rotation;
        enemyTP1Index++;
        enemyTP1Index %= poolNumber;
    }

    public void EnemyTS1Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject _gameObject = Spawn(enemyTS1List, enemyTS1Index);
        _gameObject.transform.position = position;
        _gameObject.transform.rotation = rotation;
        enemyTS1Index++;
        enemyTS1Index %= poolNumber;
    }
}
