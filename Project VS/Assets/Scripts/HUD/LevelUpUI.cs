using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : GameListenerBehaviour, IUiEventListener<AbilityIndex> {
    [SerializeField] private GameObject curtain;
    [SerializeField] private RectTransform buttonRoot;
    
    private List<AbilityUpgradeButton> upgradeButtons = new List<AbilityUpgradeButton>();
    private int waitingLevelUp = 0;
    private bool isShowing = false;

    private void Awake() {
        curtain.SetActive(false);
    }

    private void Show() {
        waitingLevelUp--;
        if (isShowing) {
            Debugger.Error("[LevelUpUI.Show] Call Duplicated!");
            return;
        }

        isShowing = true;
        
        curtain.SetActive(true);
        var cardIndices = CardManager.GetAvailableCardList();

        if (cardIndices == null || cardIndices.Count == 0) {
            curtain.SetActive(false);
            GameManager.Controller.ResumeGame();
            isShowing = false;
            return;
        }

        for (int i = 0; i < cardIndices.Count; i++) {
            var button = PoolManager.GetByType<AbilityUpgradeButton>(buttonRoot);
            button.Initialize(this, cardIndices[i]);
            upgradeButtons.Add(button);
        }
    }

    public void Abandon() {
        curtain.SetActive(false);
        upgradeButtons.ForEach(b => PoolManager.Abandon(b.gameObject));
        upgradeButtons.Clear();
        isShowing = false;
    }
    
    public void InvokeEvent(AbilityIndex abilityIndex) {
        GameManager.Ability.AddOrUpgradeItem(abilityIndex);
        GameManager.Controller?.CallSelectItem();
        Abandon();
    }

    public override void OnLevelUp() {
        waitingLevelUp++;
    }

    private void Update() {
        if (waitingLevelUp > 0 && !isShowing) {
            Show();
        }
    }
}
