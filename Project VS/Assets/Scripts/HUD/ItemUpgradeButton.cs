using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemUpgradeButton : MonoBehaviour {
    public ItemData data;
    public int level;
    public WeaponController weaponController;

    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text levelText;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void Start() {
        iconImage.sprite = data.itemIcon;
    }

    private void LateUpdate() {
        levelText.text = $"Lv.{level + 1}";
    }

    private void OnClick() {
        weaponController.AddOrUpgradeWeapon(data.itemIndex);
    }
}
