using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBarUI : GameListenerBehaviour {
    [SerializeField] private Slider hpBar;

    public override void OnGameStart() {
        UpdateHpBar();
    }

    public override void OnHitPlayer() {
        UpdateHpBar();
    }
    
    void UpdateHpBar() {
        var player = GameManager.Player;
        if (!player) {
            Debugger.Error("[PlayerStatusBarUI.UpdateHpBar] GameManager.Player is null!!");
            return;
        }

        hpBar.value = player.Status.Hp / player.Status.MaxHp;
    }
}
