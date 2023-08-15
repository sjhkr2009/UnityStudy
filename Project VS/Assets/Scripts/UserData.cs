using System;
using UnityEngine;

[Serializable]
public class UserData {
    private UserData(){}

    private static UserData loadedData;

    private const string SaveKey = "USER_GAME_DATA";
    
    public int coinCount;
    public int attackPowerUpgrade;
    public int moveSpeedUpgrade;
    public int criticalRateUpgrade;
    public int maxHpUpgrade;
    public int attackSpeedUpgrade;

    public void Save() {
        try {
            var dataString = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(SaveKey, dataString);
            PlayerPrefs.Save();
        } catch (Exception e) {
            Debugger.Error($"[UserData.Save] {e.GetType().Name} | Fail to Save UserData!!");
        }
    }

    public static UserData GetData() {
        return loadedData ??= LoadOrCreate();
    }

    public static int Coin {
        get => GetData().coinCount;
        set {
            GetData().coinCount = value;
            GetData().Save();
        }
    }

    private static UserData LoadOrCreate() {
        var loadedStr = PlayerPrefs.GetString(SaveKey);
        if (string.IsNullOrEmpty(loadedStr)) {
            return new UserData();
        }

        try {
            return JsonUtility.FromJson<UserData>(loadedStr);
        } catch (Exception e) {
            Debugger.Error($"[UserData.LoadOrCreate] {e.GetType().Name} | Fail to Load UserData!!");
            return new UserData();
        }
    }
}
