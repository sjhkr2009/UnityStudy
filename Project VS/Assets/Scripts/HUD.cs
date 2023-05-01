using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    [SerializeField] private GaugeWithText expDrawer;
    [SerializeField] private TMP_Text killDrawer;
    [SerializeField] private TMP_Text levelDrawer;
    [SerializeField] private TMP_Text timeDrawer;

    private void OnEnable() {
        GameManager.OnGameStart += UpdateAll;
        GameManager.OnDeadEnemy += UpdateExp;
        GameManager.OnDeadEnemy += UpdateKillCount;
        GameManager.OnLevelUp += UpdateLevel;
        GameManager.OnEverySecond += UpdateTimer;
    }

    private void OnDisable() {
        GameManager.OnGameStart -= UpdateAll;
        GameManager.OnDeadEnemy -= UpdateExp;
        GameManager.OnDeadEnemy -= UpdateKillCount;
        GameManager.OnLevelUp -= UpdateLevel;
        GameManager.OnEverySecond -= UpdateTimer;
    }

    void UpdateAll() {
        UpdateExp();
        UpdateLevel();
        UpdateKillCount();
        UpdateTimer();
    }

    void UpdateExp() {
        var exp = GlobalData.Controller.Exp;
        var requiredExp = GlobalData.Controller.RequiredExp;
        expDrawer.SetValue(exp, requiredExp);
    }

    void UpdateLevel() {
        levelDrawer.text = $"Lv.{GlobalData.Controller.Level}";
    }
    
    void UpdateKillCount() {
        killDrawer.text = GlobalData.Controller.KillCount.ToString();
    }
    
    void UpdateTimer() {
        var time = TimeSpan.FromSeconds(GlobalData.Controller.GameTime);
        timeDrawer.text = time.ToString("mm':'ss");
    }
}
