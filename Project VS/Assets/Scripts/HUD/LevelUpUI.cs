using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : GameListenerBehavior, IUiEventListener<AbilityIndex> {
    // TODO : 업그레이드할 대상을 선정하는 로직 추가 필요
    [SerializeField] private List<AbilityIndex> itemIndices = new List<AbilityIndex>() {
        AbilityIndex.WeaponSpinAround,
        AbilityIndex.WeaponAutoGun,
        AbilityIndex.SkillFireball
    };
    
    [SerializeField] private GameObject curtain;
    [SerializeField] private RectTransform buttonRoot;
    
    private List<AbilityUpgradeButton> upgradeButtons = new List<AbilityUpgradeButton>();

    private void Awake() {
        curtain.SetActive(false);
    }
    
    public void Show() {
        curtain.SetActive(true);
        for (int i = 0; i < itemIndices.Count; i++) {
            var button = PoolManager.GetByType<AbilityUpgradeButton>(buttonRoot);
            button.Initialize(this, itemIndices[i]);
            upgradeButtons.Add(button);
        }
    }

    public void Abandon() {
        curtain.SetActive(false);
        upgradeButtons.ForEach(b => PoolManager.Abandon(b.gameObject));
        upgradeButtons.Clear();
    }
    
    public void InvokeEvent(AbilityIndex abilityIndex) {
        GameManager.Ability.AddOrUpgradeItem(abilityIndex);
        
        GameManager.Controller?.CallSelectItem();
        Abandon();
    }

    public override void OnLevelUp() {
        Show();
    }
}
