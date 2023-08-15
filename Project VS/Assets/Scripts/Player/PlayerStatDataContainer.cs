using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(PlayerStatDataContainer), menuName = "Custom/Create Player Status Data")]
public class PlayerStatDataContainer : ScriptableObject {
    private static PlayerStatDataContainer instance;

    [SerializeField] private PlayerStatData defaultData;
    [SerializeField] private List<PlayerStatData> statByUpgradePoint = new List<PlayerStatData>();
    
#if UNITY_EDITOR
    [Button]
    public void Save() {
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif

    public static PlayerStatData LoadData() {
        var data = GetInstance();

        try {
            var userData = UserData.GetData();
            var statData = data.defaultData.Clone();
            statData.attackPower = data.statByUpgradePoint[userData.attackPowerUpgrade].attackPower;
            statData.attackSpeed = data.statByUpgradePoint[userData.attackSpeedUpgrade].attackSpeed;
            statData.criticalRate = data.statByUpgradePoint[userData.criticalRateUpgrade].criticalRate;
            statData.maxHp = data.statByUpgradePoint[userData.maxHpUpgrade].maxHp;
            statData.speed = data.statByUpgradePoint[userData.moveSpeedUpgrade].speed;

            return statData;
        } catch (Exception e) {
            Debugger.Error($"[PlayerStatDataContainer.LoadData] {e.GetType().Name} on Load PlayerStatData!");
            return data.defaultData.Clone();
        }
    }
    
    private static PlayerStatDataContainer GetInstance() {
        if (instance) return instance;

        instance = Resources.Load<PlayerStatDataContainer>(nameof(PlayerStatDataContainer));
        if (!instance) {
            Debugger.Error("Cannot find 'PlayerStatusDataContainer' in Resources...");
            instance = CreateInstance<PlayerStatDataContainer>();
        }

        return instance;
    }
}
