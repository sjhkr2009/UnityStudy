using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AbilityUpgradeButton : MonoBehaviour, IPoolHandler {
    private AbilityData data;

    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text descText;

    private IUiEventListener<AbilityIndex> listener;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Initialize(IUiEventListener<AbilityIndex> listener, AbilityIndex abilityIndex) {
        data = AbilityDataContainer.GetDataOrDefault(abilityIndex);
        this.listener = listener;
        UpdateData();
    }

    public void UpdateData() {
        if (data == null) {
            Debugger.Error("[AbilityUpgradeButton.UpdateData] ItemUpgradeButton not initialized");
            return;
        }
        
        iconImage.sprite = data.itemIcon;
        titleText.text = data.itemName;

        int level = GameManager.Ability.GetLevel(data.abilityIndex);
        descText.text = data.GetDescription(level + 1);
        levelText.text = level <= 0 ? "New!" : $"Lv.{level} >> Lv.{level + 1}";
    }

    private void OnClick() {
        if (data == null) {
            Debugger.Error("[AbilityUpgradeButton.OnClick] ItemUpgradeButton not initialized");
            return;
        }
        listener?.InvokeEvent(data.abilityIndex);
    }

    public void OnInitialize() { }

    public void OnRelease() {
        data = null;
    }
}
