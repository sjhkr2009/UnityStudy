using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SkillInteractionUI : MonoBehaviour {
    [SerializeField] private SkillButton skillButton1;
    [SerializeField] private SkillButton skillButton2;

    void Start() {
        skillButton1.ResetCallback(GameBroadcaster.CallSkill1);
        skillButton2.ResetCallback(GameBroadcaster.CallSkill2);
        UpdateButtons();
    }

    private void UpdateButtons() {
        var skillController = GameManager.Player.SkillController;
        skillButton1.SetCooldown(skillController.CanUseSkill1, skillController.RemainCooldown1, skillController.Cooldown1);
        skillButton2.SetCooldown(skillController.CanUseSkill2, skillController.RemainCooldown2, skillController.Cooldown2);
    }

    private void Update() {
        UpdateButtons();
    }
}
