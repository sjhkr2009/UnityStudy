using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardManager {
    public static List<CardData> CurrentSelectedCard = new List<CardData>();

    public static readonly Dictionary<int,CardTierLevelData> CardTierLevelDatas = new Dictionary<int, CardTierLevelData>() {
        {1, new CardTierLevelData(75,20,5,0,0)},
        {2, new CardTierLevelData(75,20,5,0,0)},
        {3, new CardTierLevelData(74,20,5,20,1)},
        {4, new CardTierLevelData(73,20,5,45,1)},
        {5, new CardTierLevelData(72,20,5,70,1)},
        {6, new CardTierLevelData(72,20,5,100,1)},
        {7, new CardTierLevelData(70,20,5,10,2)},
        {8, new CardTierLevelData(70,20,5,25,2)},
        {9, new CardTierLevelData(69,20,5,40,2)},
        {10, new CardTierLevelData(68,20,5,55,2)},
        {11, new CardTierLevelData(67,20,5,75,2)},
        {12, new CardTierLevelData(67,20,5,100,2)},
        {13, new CardTierLevelData(65,20,5,10,3)},
        {14, new CardTierLevelData(65,20,5,25,3)},
        {15, new CardTierLevelData(64,20,5,50,3)},
        {16, new CardTierLevelData(63,20,5,75,3)},
        {17, new CardTierLevelData(62,20,5,100,3)},
        {18, new CardTierLevelData(62,20,5,10,4)},
        {19, new CardTierLevelData(60,20,5,25,4)},
        {20, new CardTierLevelData(60,20,5,40,4)}
    };

    public static readonly List<CardData> AllCard = new List<CardData> {
        CardData.CreateAbilityCard(CardTier.SILVER, AbilityIndex.WeaponAutoGun),
        CardData.CreateAbilityCard(CardTier.SILVER, AbilityIndex.WeaponRandomAreaStrike),
        CardData.CreateAbilityCard(CardTier.SILVER, AbilityIndex.WeaponSpinAround),
        CardData.CreateAbilityCard(CardTier.SILVER, AbilityIndex.SkillFireball),
    };

    public static List<CardData> SilverCard { get; set; }
    public static List<CardData> GoldCard { get; set; }
    public static List<CardData> PlatinumCard { get; set; }

    public static void OnStartGame() {
        SilverCard = new List<CardData>();
        GoldCard = new List<CardData>();
        PlatinumCard = new List<CardData>();
        CurrentSelectedCard = new List<CardData>();
        
        foreach (var card in AllCard) {
            switch (card.cardTier) {
                case CardTier.SILVER:
                    SilverCard.Add(card);
                    break;
                case CardTier.GOLD:
                    GoldCard.Add(card);
                    break;
                case CardTier.PLATINUM:
                    PlatinumCard.Add(card);
                    break;
            }
        }
    }

    public static List<CardData> GetAvailableCardList() {
        // 티어 반환
        var currentTier = GetCurrentTier(GameManager.Controller.Level);
        List<CardData> availableCardList = new List<CardData>();
        var currentAbilities = GameManager.Ability.AllAbilities;
        
        // 카드 색출
        switch (currentTier) {
            case CardTier.SILVER:
                availableCardList = SilverCard.Where(card => card.CanShowNow(currentAbilities)).ToList();
                break;
            case CardTier.GOLD:
                availableCardList = GoldCard.Where(card => card.CanShowNow(currentAbilities)).ToList();
                break;
            case CardTier.PLATINUM:
                availableCardList = PlatinumCard.Where(card => card.CanShowNow(currentAbilities)).ToList();
                break;
        }

        if (availableCardList.Count < 3) {
            var additionalCards = AllCard
                .Where(c => c.CanShowNow(currentAbilities) && !availableCardList.Contains(c));
            availableCardList.AddRange(additionalCards.Shuffle().Take(3 - availableCardList.Count));
        }
        
        return availableCardList.Shuffle().Take(3).ToList();
    }

    /// <summary>
    /// 현재 반환해야 하는 카드 티어를 정하는 함수
    /// </summary>
    /// <returns></returns>
    private static CardTier GetCurrentTier(int level) {
        var tier = Random.Range(1, 101);
        int platinumCardCount = 0;

        level = level.Clamp(1, 20);

        foreach (var card in CurrentSelectedCard) {
            if (card.cardTier == CardTier.PLATINUM) platinumCardCount++;
        }
        
        int platinumPer = CardTierLevelDatas[level].platinumCondition > platinumCardCount
            ? CardTierLevelDatas[level].platinumPer + CardTierLevelDatas[level].platinumAdvantage
            : CardTierLevelDatas[level].platinumPer;

        if (tier <= platinumPer) {
            return CardTier.PLATINUM;
        } else if (tier <= platinumPer + CardTierLevelDatas[level].goldPer) {
            return CardTier.GOLD;
        }

        return CardTier.SILVER;
    }
}
