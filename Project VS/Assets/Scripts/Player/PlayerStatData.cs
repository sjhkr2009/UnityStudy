using System;

[Serializable]
public class PlayerStatData {
    public float maxHp = 200f;
    public float speed = 3f;
    public float attackPower = 1f;
    public float attackRange = 1f;
    public float skillCoolTimeRate = 1f;
    public float attackSpeed = 1f;
    public float criticalRate = 50f;

    public PlayerStatData Clone() {
        return new PlayerStatData() {
            maxHp = maxHp,
            speed = speed,
            attackPower = attackPower,
            attackRange = attackRange,
            skillCoolTimeRate = skillCoolTimeRate,
            attackSpeed = attackSpeed,
            criticalRate = criticalRate
        };
    }
}