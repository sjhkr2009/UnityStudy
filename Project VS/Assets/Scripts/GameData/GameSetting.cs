using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InternalData;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(menuName = "Create GameSetting", fileName = nameof(GameSetting))]
public class GameSetting : ScriptableObject {
    public static GameSetting Load() {
        var setting = Resources.Load<GameSetting>(nameof(GameSetting));
        if (!setting) {
            Debugger.Error("[GameSetting.Load] Cannot find 'GameSetting' on Resources folder!!");
            setting = CreateInstance<GameSetting>();
        }
        return setting;
    }

    public List<EnemyExpData> expGainData = new List<EnemyExpData>() { new EnemyExpData() };
    public List<int> expByLevel = new List<int>() { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public float maxGameTime = 60f * 10;
    
    public int GetRequiredExp(int currentLevel) {
        if (expByLevel.Count == 0) {
            Debugger.Error("[GameSetting.GetRequiredExp] expByLevel is Empty!!");
            return 10;
        }
        
        int index = (currentLevel - 1).Clamp(0, expByLevel.Count - 1);
        return expByLevel[currentLevel];
    }

    public int GetGainExp(EnemyStatus targetEnemy) {
        var expData = expGainData.FirstOrDefault(data => data.targetTier == targetEnemy.Tier && data.targetLevel == targetEnemy.Level) ??
                      expGainData.FirstOrDefault(data => data.targetTier == targetEnemy.Tier);
        if (expData == null) {
            Debugger.Log($"[GameSetting.GetGainExp] Cannot exp data {targetEnemy.Tier} : {targetEnemy.Level}");
            return 1;
        }
        
        return expData.gainExp;
    }
}
