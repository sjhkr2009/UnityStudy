using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemUpgradeButton : MonoBehaviour, IPoolHandler {
    private EquipmentData data;

    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text descText;

    private IUiEventListener<ItemIndex> listener;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Initialize(IUiEventListener<ItemIndex> listener, ItemIndex itemIndex) {
        data = EquipmentDataContainer.GetDataOrDefault(itemIndex);
        this.listener = listener;
        UpdateData();
    }

    public void UpdateData() {
        if (data == null) {
            Debugger.Error("[ItemUpgradeButton.OnClick] ItemUpgradeButton not initialized");
            return;
        }
        
        iconImage.sprite = data.itemIcon;
        titleText.text = data.itemName;

        int level = GameManager.Item.GetLevel(data.itemIndex);
        descText.text = data.GetDescription(level + 1);
        levelText.text = level <= 0 ? "New!" : $"Lv.{level} >> Lv.{level + 1}";
    }

    private void OnClick() {
        if (data == null) {
            Debugger.Error("[ItemUpgradeButton.OnClick] ItemUpgradeButton not initialized");
            return;
        }
        listener?.InvokeEvent(data.itemIndex);
    }

    public void OnInitialize() { }

    public void OnRelease() {
        data = null;
    }
}