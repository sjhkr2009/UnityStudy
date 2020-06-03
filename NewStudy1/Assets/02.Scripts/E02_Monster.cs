class Monster : Creature
{
    protected Monster(MonsterType type) : base(CreatureType.Monster)
    {
        monsterType = type;
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
}

class Slime : Monster
{
    static int slimeCount = 0;
    public Slime() : base(MonsterType.Slime)
    {
        SetInfo(25, 5);
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
        bossCount++;
        name = "보스 " + bossCount;
    }
}