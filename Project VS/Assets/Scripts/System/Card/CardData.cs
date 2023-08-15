using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CardTier {
   SILVER,
   GOLD,
   PLATINUM
}

public class CardData {
    protected CardData(Sprite icon, string cardName, string cardDescription, CardTier cardTier, AbilityIndex myAbility, int myLevel, AbilityIndex needAbility, int needLevel) {
        this.cardIcon = icon;
        this.cardName = cardName;
        this.cardDescription = cardDescription;
        this.cardTier = cardTier;
        this.requiredAbilityMinLevel = needLevel;
        this.requiredAbility = needAbility;
        myAbilityLevel = myLevel;
        MyAbility = myAbility;
    }
    
    public static CardData CreateAbilityCard(CardTier tier, AbilityIndex myIndex, int destLevel = 1) {
        var myData = AbilityDataContainer.LoadData(myIndex);
        var cardData = new CardData(myData.GetIcon(destLevel), myData.itemName, myData.GetDescription(1), tier, myIndex, 1, AbilityIndex.None, 0);
        return cardData;
    }
    
    public static CardData CreateUpgradeAbilityCard(CardTier tier, AbilityIndex myIndex, int destLevel) {
        if (destLevel <= 1) return CreateAbilityCard(tier, myIndex);
        
        var myData = AbilityDataContainer.LoadData(myIndex);
        var cardData = new CardData(myData.GetIcon(destLevel), myData.itemName, myData.GetDescription(destLevel), tier, myIndex, destLevel, myIndex, destLevel - 1);
        return cardData;
    }
    
    public static CardData CreateDependentAbilityCard(CardTier tier, AbilityIndex myIndex, AbilityIndex requiredIndex, int destLevel = 1) {
        var myData = AbilityDataContainer.LoadData(myIndex);
        var cardData = new CardData(myData.GetIcon(destLevel), myData.itemName, myData.GetDescription(destLevel), tier, myIndex, 1, requiredIndex, destLevel);
        return cardData;
    }
    
    public Sprite cardIcon;
    public string cardName;
    public string cardDescription;

    public CardTier cardTier;

    public AbilityIndex MyAbility { get; protected set; } // 이 카드가 상징하는 어빌리티

    protected AbilityIndex requiredAbility; // 획득하기 위해 필요한 어빌리티
    protected int requiredAbilityMinLevel; // requiredAbility가 있을 경우, 요구되는 최소 레벨
    protected int myAbilityLevel; // 이 카드 획득 시 해당 레벨이 되며, myAbility를 이 레벨 이상 보유중이면 등장하지 않음  

    public virtual bool CanShowNow(IReadOnlyList<AbilityBase> currentAbilities) {
        if (currentAbilities.Any(a => a.Index == MyAbility && myAbilityLevel <= a.Level)) return false;
        if (requiredAbility != AbilityIndex.None) {
            var required = currentAbilities.FirstOrDefault(a => a.Index == requiredAbility);
            if (required == null || required.Level < requiredAbilityMinLevel) return false;
        }

        return true;
    }
}