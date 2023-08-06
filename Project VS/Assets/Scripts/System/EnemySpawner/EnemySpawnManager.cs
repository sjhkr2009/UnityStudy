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
        
        spawnSetting.OnSpawn();
    }

    private static string GetEnemyPrefabName(EnemyIndex enemyIndex) {
        return enemyIndex switch {
            EnemyIndex.Monster1 => "Enemy01",
            EnemyIndex.Monster2 => "Enemy02",
            _ => string.Empty
        };
    }
}
