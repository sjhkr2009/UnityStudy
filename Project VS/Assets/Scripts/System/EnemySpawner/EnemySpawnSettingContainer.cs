using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EnemySpawnSettingContainer), menuName = "Custom/" + nameof(EnemySpawnSettingContainer))]
public class EnemySpawnSettingContainer : ScriptableObject {
    private static EnemySpawnSettingContainer _instance;
    public static EnemySpawnSettingContainer Instance =>
        _instance ??= Resources.Load<EnemySpawnSettingContainer>(nameof(EnemySpawnSettingContainer));
    
    public List<EnemySpawnSetting> spawnSettings = new List<EnemySpawnSetting>();
}
