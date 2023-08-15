using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AbilityUpgradeButton : MonoBehaviour, IPoolHandler {
    private CardData data;

    [SerializeField] private Sprite[] frameSprites;
    [SerializeField] private Image frameImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descText;

    private IUiEventListener<AbilityIndex> listener;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Initialize(IUiEventListener<AbilityIndex> listener, CardData cardData) {
        data = cardData;
        this.listener = listener;
        UpdateData();
    }

    public void UpdateData() {
        if (data == null) {
            Debugger.Error("[AbilityUpgradeButton.UpdateData] ItemUpgradeButton not initialized");
            return;
        }

        switch (data.cardTier) {
            case CardTier.SILVER:
                frameImage.sprite = frameSprites[0];
                break;
            case CardTier.GOLD:
                frameImage.sprite = frameSprites[1];
                break;
            case CardTier.PLATINUM:
                frameImage.sprite = frameSprites[2];
                break;
        }
        
        iconImage.sprite = data.cardIcon;
        titleText.text = data.cardName;
        descText.text = data.cardDescription;
    }

    private void OnClick() {
        if (data == null) {
            Debugger.Error("[AbilityUpgradeButton.OnClick] ItemUpgradeButton not initialized");
            return;
        }
        
        CardManager.CurrentSelectedCard.Add(data);
        listener?.InvokeEvent(data.MyAbility);
    }

    public void OnInitialize() { }

    public void OnRelease() {
        data = null;
    }
}
