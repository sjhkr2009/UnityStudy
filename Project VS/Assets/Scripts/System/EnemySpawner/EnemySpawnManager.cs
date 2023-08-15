using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemySpawnManager {
    public static HashSet<EnemyControllerBase> SpawnedEnemies { get; } = new HashSet<EnemyControllerBase>();

    public static void Spawn(EnemySpawnSetting spawnSetting, Vector2 spawnPos) {
        var enemy = PoolManager.Get<EnemyControllerBase>(GetEnemyPrefabName(spawnSetting.enemyIndex));
        var stat = EnemyDataContainer.GetStatData(spawnSetting.enemyIndex);
        
        if (!enemy) {
            Debugger.Error($"[EnemyFactory.Spawn] Cannot fine prefab of '{spawnSetting.enemyIndex}'");
            return;
        }

        if (stat == null) {
            Debugger.Error($"[EnemyFactory.Spawn] Cannot fine stat data of '{spawnSetting.enemyIndex}'");
            return;
        }

        enemy.transform.position = spawnPos;
        enemy.Status.Initialize(stat);

        var hardModeTimer = GameManager.Controller.PlayingTimeSecond - 360f;
        if (hardModeTimer > 0f) {
            var multiple = 1f + (hardModeTimer / 60f);
            enemy.Status.IncreaseStat(multiple);
        }
        
        spawnSetting.OnSpawn();
    }

    private static string GetEnemyPrefabName(EnemyIndex enemyIndex) {
        return enemyIndex switch {
            EnemyIndex.Slime => "Enemy01",
            EnemyIndex.Zombie => "Enemy02",
            EnemyIndex.Bat => "Enemy03",
            EnemyIndex.Goblin => "Enemy04",
            EnemyIndex.Worm => "Enemy05",
            EnemyIndex.Skeleton => "Enemy06",
            EnemyIndex.BossCyclubs => "Boss01",
            EnemyIndex.BossDemon => "Boss02",
            _ => string.Empty
        };
    }
    public static void Clear() {
        var clearList = new List<GameObject>();
        
        // Abandon 과정에서 HashSet에서 요소가 제거되므로, 다른 리스트에 복사해두고 릴리즈한다
        SpawnedEnemies.ForEach(item => {
            if (item) clearList.Add(item.gameObject);
        });
        clearList.ForEach(PoolManager.Abandon);
        
        SpawnedEnemies.Clear();
    }
}
