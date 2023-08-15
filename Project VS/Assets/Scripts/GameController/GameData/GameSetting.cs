using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameSetting), menuName = "Custom/Create GameSetting")]
public class GameSetting : ScriptableObject {
    public static GameSetting Load() {
        var setting = Resources.Load<GameSetting>(nameof(GameSetting));
        if (!setting) {
            Debugger.Error("[GameSetting.Load] Cannot find 'GameSetting' on Resources folder!!");
            setting = CreateInstance<GameSetting>();
        }
        return setting;
    }
    
    public List<int> expByLevel = new List<int>() { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public float maxGameTime = 60f * 10;
    
    public int GetRequiredExp(int currentLevel) {
        if (expByLevel.Count == 0) {
            Debugger.Error("[GameSetting.GetRequiredExp] expByLevel is Empty!!");
            return 10;
        }
        
        int index = (currentLevel - 1).Clamp(0, expByLevel.Count - 1);
        return expByLevel[index];
    }
}
