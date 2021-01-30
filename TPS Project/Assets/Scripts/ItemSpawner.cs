using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;          // 생성할 아이템들의 원본 프리팹
    public Transform playerTransform;   // 플레이어 위치

    private float lastSpawnTime;        // 최근에 아이템을 생성한 시간
    public float maxDistance = 5f;      // 플레이어 주변에 아이템이 배치될 최대 반경

    private float timeBetSpawn;         // 아이템 생성 간격이 들어갈 변수 (최소~최대값 중 랜덤)
    public float timeBetSpawnMax = 7f;
    public float timeBetSpawnMin = 2f;

    private void Start()
    {
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0f;
    }

    private void Update()
    {
        if(Time.time >= lastSpawnTime + timeBetSpawn &&
            playerTransform != null)
		{
            Spawn();
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
		}
    }

    private void Spawn()
    {
        if (items.Length == 0)
            return;
        
        Vector3 spawnPos = Utility.GetRandomPointOnNavMesh(playerTransform.position, maxDistance) +
            (Vector3.up * 0.5f);

        GameObject item = Instantiate(items[Random.Range(0, items.Length)], spawnPos, Quaternion.identity);
        Destroy(item, 10f);
    }
}