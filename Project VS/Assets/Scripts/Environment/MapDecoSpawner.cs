using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapDecoSpawner : MonoBehaviour {
    [SerializeField, BoxGroup("기본 정보")] private Transform parent;
    [SerializeField, BoxGroup("기본 정보")] private Rect spawnArea;
    [SerializeField, BoxGroup("소환물 정보")] private DecoSpawnInfo[] decoSpawnInfos;
    [SerializeField, ReadOnly] private List<GameObject> spawnedDecos = new List<GameObject>();
    
    [Serializable]
    class DecoSpawnInfo {
        public GameObject prefab;
        public bool isRandomSpawn;
        [ShowIf("isRandomSpawn")] public int minSpawnCount;
        [ShowIf("isRandomSpawn")] public int maxSpawnCount;
        [HideIf("isRandomSpawn")] public Vector2 offset;
        [HideIf("isRandomSpawn")] public Vector2 interval;
    }
    
    [Button]
    public void SpawnDeco() {
        if (decoSpawnInfos == null) {
            Debugger.Error($"[MapDecoSpawner.SpawnDeco] decoSpawnInfos == null!!");
            return;
        }
        
        spawnedDecos.ForEach(DestroyImmediate);
        spawnedDecos.Clear();
        
        var randomSpawnInfos = decoSpawnInfos.Where(i => i.isRandomSpawn).ToList();
        var patternSpawnInfos = decoSpawnInfos.Where(i => !i.isRandomSpawn).ToList();

        List<Vector2> avoidPosition = new List<Vector2>();
        
        foreach (var spawnInfo in patternSpawnInfos) {
            Vector2 spawnStartPosition = new Vector2(
                spawnArea.xMin + spawnInfo.offset.x,
                spawnArea.yMin + spawnInfo.offset.y
            );
            Vector2 spawnPosition = spawnStartPosition;
            if (spawnInfo.interval.x == 0f || spawnInfo.interval.y == 0f) {
                Debugger.Error($"[MapDecoSpawner.SpawnDeco] {spawnInfo.prefab} : interval의 각 값은 양수로 지정해주세요. (cur: {spawnInfo.interval})");
                continue;
            }

            while (spawnPosition.y < spawnArea.yMax) {
                while (spawnPosition.x < spawnArea.xMax) {
                    GameObject spawnedDeco = Instantiate(spawnInfo.prefab, spawnPosition, Quaternion.identity, parent);
                    spawnedDecos.Add(spawnedDeco);
                    avoidPosition.Add(spawnPosition);
                    spawnPosition += Vector2.right * spawnInfo.interval.x;
                }
                spawnPosition = new Vector2(spawnStartPosition.x, spawnPosition.y + spawnInfo.interval.y);
            }
        }
        
        foreach (var spawnInfo in randomSpawnInfos) {
            int spawnCount = Random.Range(spawnInfo.minSpawnCount, spawnInfo.maxSpawnCount + 1);
            for (int i = 0; i < spawnCount; i++) {
                Vector2 spawnPosition = new Vector2(
                    Random.Range(spawnArea.xMin, spawnArea.xMax),
                    Random.Range(spawnArea.yMin, spawnArea.yMax)
                );
                int tryCount = 0;
                while (avoidPosition.Any(pos => Vector2.Distance(pos, spawnPosition) < 1.5f)) {
                    tryCount++;
                    spawnPosition = new Vector2(
                        Random.Range(spawnArea.xMin, spawnArea.xMax),
                        Random.Range(spawnArea.yMin, spawnArea.yMax)
                    );
                    if (tryCount > 20) {
                        Debugger.Warning($"[MapDecoSpawner.SpawnDeco] Cannot Find Spawnable Point!!");
                    }
                }
                GameObject spawnedDeco = Instantiate(spawnInfo.prefab, spawnPosition, Quaternion.identity, parent);
                spawnedDecos.Add(spawnedDeco);
            }
        }
    }
}
