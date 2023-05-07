using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {
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
        var exp = GameManager.Controller.Exp;
        var requiredExp = GameManager.Controller.RequiredExp;
        expDrawer.SetValue(exp, requiredExp);
    }

    void UpdateLevel() {
        levelDrawer.text = $"Lv.{GameManager.Controller.Level}";
    }
    
    void UpdateKillCount() {
        killDrawer.text = GameManager.Controller.KillCount.ToString();
    }
    
    void UpdateTimer() {
        var time = TimeSpan.FromSeconds(GameManager.Controller.GameTime);
        timeDrawer.text = time.ToString("mm':'ss");
    }
}
