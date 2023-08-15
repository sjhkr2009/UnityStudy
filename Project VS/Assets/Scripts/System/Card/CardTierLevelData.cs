using System.Collections.Generic;

public class CardTierLevelData {
    public CardTierLevelData(int silverPer, int goldPer, int platinumPer, int platinumAdvantage, int platinumCondition) {
        this.silverPer = silverPer;
        this.goldPer = goldPer;
        this.platinumPer = platinumPer;
        this.platinumAdvantage = platinumAdvantage;
        this.platinumCondition = platinumCondition;
    }
    public int silverPer; // 실버 확률
    public int goldPer; // 골드 확률 
    public int platinumPer; // 플래티넘 확률

    public int platinumAdvantage; // 플래티넘 획득 보정값 
    public int platinumCondition; // 플래티넘 획득 보정값을 주는 플래티넘 카드 갯수 조건
}