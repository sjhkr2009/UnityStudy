using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    public GameObject enemyBulletPrefab;

    public Transform bulletGroup;
    public Transform enemyGroup;
    public Transform enemyBulletGroup;
    
    List<GameObject> bulletList = new List<GameObject>();
    List<GameObject> enemyList = new List<GameObject>();
    List<GameObject> enemyBulletList = new List<GameObject>();

    public int poolCount = 100;

    int bulletIndex = 0;
    int enemyIndex = 0;
    int enemyBulletIndex = 0;

    List<GameObject> MakeObjectPool(List<GameObject> objectList, GameObject prefab, Transform group)
    {
        for (int i = 0; i < poolCount; i++)
        {
            objectList.Add(Instantiate(prefab, group));
            objectList[objectList.Count - 1].SetActive(false);
            //Debug.Log($"{group.gameObject.name} 생성됨: {i+1}");
        }
        return objectList;
    }

    void Awake()
    {
        bulletList = MakeObjectPool(bulletList, bulletPrefab, bulletGroup);
        enemyList = MakeObjectPool(enemyList, enemyPrefab, enemyGroup);
        enemyBulletList = MakeObjectPool(enemyBulletList, enemyBulletPrefab, enemyBulletGroup);
    }

    public void BulletSpawn(Vector3 pos)
    {
        bulletIndex = Spawn(bulletList, pos, 0f, bulletIndex);
    }

    public void EnemySpawn(Vector3 pos)
    {
        Debug.Log("적 최대 개수: " + enemyList.Count);
        enemyIndex = Spawn(enemyList, pos, 180f, enemyIndex);
        Debug.Log($"적 생성 완료: {enemyIndex}");
    }

    public void EnemyBulletSpawn(Vector3 pos, Quaternion rot)
    {
        enemyBulletIndex = Spawn(enemyBulletList, pos, rot, enemyBulletIndex);
    }

    int Spawn(List<GameObject> objectList, Vector3 _position, float _rotation, int _index)
    {
        Debug.Log("오브젝트 풀링: " + objectList.Count);
        objectList[_index].transform.position = _position;
        objectList[_index].transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
        objectList[_index].SetActive(true);
        _index++;
        _index %= poolCount;
        return _index;
    }

    int Spawn(List<GameObject> objectList, Vector3 _position, Quaternion _rotation, int _index)
    {
        objectList[_index].transform.position = _position;
        objectList[_index].transform.rotation = _rotation;
        objectList[_index].SetActive(true);
        _index++;
        _index %= poolCount;
        return _index;
    }
}
