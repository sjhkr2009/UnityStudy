using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EnemyDataContainer), menuName = "Custom/" + nameof(EnemyDataContainer))]
public class EnemyDataContainer : ScriptableObject {
    private static EnemyDataContainer _instance;

    public List<EnemyStatData> statDatas = new List<EnemyStatData>();

    public void Initialize() {
        if (statDatas?.Count > 0) {
            Debugger.Warning("[EnemyDataContainer.Initialize] statDatas already exist!");
            return;
        }
        
        statDatas = new List<EnemyStatData>();
        foreach (EnemyIndex enemyIndex in Enum.GetValues(typeof(EnemyIndex))) {
            if (enemyIndex == EnemyIndex.None) continue;

            statDatas.Add(new EnemyStatData() { enemyIndex = enemyIndex });
        }
    }

    public static EnemyStatData GetStatData(EnemyIndex enemyIndex) {
        var container = GetInstance();
        if (container.statDatas == null || !container.statDatas.Any(s => s.enemyIndex == enemyIndex)) {
            Debugger.Error("[EnemyDataContainer.GetStatData] Container is null or empty!!");
            return default;
        }

        return container.statDatas.First(s => s.enemyIndex == enemyIndex);
    }
    
    private static EnemyDataContainer GetInstance() {
        if (_instance) return _instance;

        _instance = Resources.Load<EnemyDataContainer>(nameof(EnemyDataContainer));
        if (!_instance) {
            Debugger.Error("Cannot find 'EnemyDataContainer' in Resources...");
            _instance = CreateInstance<EnemyDataContainer>();
        }

        return _instance;
    }
}
