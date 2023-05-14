using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemUpgradeButton : MonoBehaviour, IPoolHandler {
    private ItemData data;
    public ItemController ItemController => GameManager.Item;

    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text descText;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Initialize(ItemIndex itemIndex) {
        data = ItemDataContainer.GetDataOrDefault(itemIndex);
        UpdateData();
    }

    public void UpdateData() {
        if (data == null) {
            Debugger.Error("[ItemUpgradeButton.OnClick] ItemUpgradeButton not initialized");
            return;
        }
        
        iconImage.sprite = data.itemIcon;
        descText.text = data.itemDesc;
        
        int level = ItemController.GetLevel(data.itemIndex);
        levelText.text = $"Lv.{level + 1}";
    }

    private void OnClick() {
        if (data == null) {
            Debugger.Error("[ItemUpgradeButton.OnClick] ItemUpgradeButton not initialized");
            return;
        }
        ItemController.AddOrUpgradeItem(data.itemIndex);
    }

    public void OnInitialize() { }

    public void OnRelease() {
        data = null;
    }
}
