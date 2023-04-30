using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    [SerializeField] private GaugeWithText expDrawer;

    private void OnEnable() {
        GameManager.OnGameStart += UpdateAll;
        GameManager.OnDeadEnemy += UpdateExp;
    }

    private void OnDisable() {
        GameManager.OnGameStart -= UpdateAll;
        GameManager.OnDeadEnemy -= UpdateExp;
    }

    void UpdateAll() {
        UpdateExp();
    }

    void UpdateExp() {
        if (!expDrawer) return;
        
        var exp = GlobalData.Controller.Exp;
        var requiredExp = GlobalData.Controller.RequiredExp;
        expDrawer.SetValue(exp, requiredExp);
    }
}
