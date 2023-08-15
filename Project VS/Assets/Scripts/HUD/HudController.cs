using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : GameListenerBehaviour {
    [SerializeField] private GaugeWithText expDrawer;
    [SerializeField] private TMP_Text killDrawer;
    [SerializeField] private TMP_Text levelDrawer;
    [SerializeField] private TMP_Text timeDrawer;

    private void Awake() {
        expDrawer.SetValue(0, 1);
        killDrawer.text = "0";
        levelDrawer.text = "1";
        timeDrawer.text = "00:00";
    }

    void UpdateExp() {
        var status = GameManager.Controller.GetCurrentStatus();
        var exp = status.exp;
        var requiredExp = status.requiredExp;
        expDrawer.SetValue(exp, requiredExp);
    }

    void UpdateLevel() {
        var status = GameManager.Controller.GetCurrentStatus();
        levelDrawer.text = $"{status.level}";
    }
    
    void UpdateKillCount() {
        var status = GameManager.Controller.GetCurrentStatus();
        killDrawer.text = status.killCount.ToString();
    }
    
    void UpdateTimer() {
        var status = GameManager.Controller.GetCurrentStatus();
        var time = TimeSpan.FromSeconds(status.gameTime);
        timeDrawer.text = time.ToString("mm':'ss");
    }

    public override void OnGameStart() {
        UpdateExp();
        UpdateLevel();
        UpdateKillCount();
        UpdateTimer();
    }

    public override void OnDeadEnemy(EnemyStatus deadEnemy) {
        UpdateKillCount();
    }

    public override void OnGainDropItem(DropItemIndex dropItemIndex) {
        UpdateExp();
    }

    public override void OnLevelUp() {
        UpdateLevel();
    }

    public override void OnEverySecond() {
        UpdateTimer();
    }
}
