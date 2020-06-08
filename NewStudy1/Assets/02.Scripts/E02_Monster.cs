class Monster : Creature
{
    protected Monster(MonsterType type) : base(CreatureType.Monster)
    {
        monsterType = type;
        damageDifference = 0.2f;
    }
    
    protected enum MonsterType
    {
        None,
        Slime = 1,
        Ork = 2,
        Skeleton = 3,
        Boss = 4
    }

    protected MonsterType monsterType = MonsterType.None;

    public float speed { get; protected set; }
}

class Slime : Monster
{
    static int slimeCount = 0;
    public Slime() : base(MonsterType.Slime)
    {
        SetInfo(25, 5);
        maxHp = GetHp();
        speed = 70f;
        slimeCount++;
        name = "슬라임 " + slimeCount;
    }
}

class Ork : Monster
{
    static int orkCount = 0;
    public Ork() : base(MonsterType.Ork)
    {
        SetInfo(65, 8);
        maxHp = GetHp();
        speed = 5f;
        orkCount++;
        name = "오크 " + orkCount;
    }
}
class Skeleton : Monster
{
    static int skeletonCount = 0;
    public Skeleton() : base(MonsterType.Skeleton)
    {
        SetInfo(35, 17);
        maxHp = GetHp();
        speed = 35f;
        skeletonCount++;
        name = "스켈레톤 " + skeletonCount;
    }
}
class Boss : Monster
{
    static int bossCount = 0;
    public Boss() : base(MonsterType.Boss)
    {
        SetInfo(100, 15);
        maxHp = GetHp();
        speed = 50f;
        bossCount++;
        name = "보스 " + bossCount;
    }
}