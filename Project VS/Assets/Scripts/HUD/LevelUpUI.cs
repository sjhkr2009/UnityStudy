using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelUpUI : MonoBehaviour, IUiEventListener<ItemIndex> {
    // TODO : 업그레이드할 대상을 선정하는 로직 추가 필요
    [SerializeField] private List<ItemIndex> itemIndices = new List<ItemIndex>() {
        ItemIndex.WeaponSpinAround,
        ItemIndex.WeaponAutoGun,
        ItemIndex.NormalShoes
    };
    private List<ItemUpgradeButton> upgradeButtons = new List<ItemUpgradeButton>();

    private void Awake() {
        GameManager.OnLevelUp += Show;
    }
    
    public void Show() {
        for (int i = 0; i < itemIndices.Count; i++) {
            var button = PoolManager.GetByType<ItemUpgradeButton>(transform);
            button.Initialize(this, itemIndices[i]);
            upgradeButtons.Add(button);
        }
    }

    public void Abandon() {
        upgradeButtons.ForEach(b => PoolManager.Abandon(b.gameObject));
        upgradeButtons.Clear();
    }
    
    public void InvokeEvent(ItemIndex itemIndex) {
        GameManager.Item.AddOrUpgradeItem(itemIndex);
        GameManager.Instance.CallSelectItem();
        Abandon();
    }
}
