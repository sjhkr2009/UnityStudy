class Player : Creature
{
    protected Player(PlayerType type) : base(CreatureType.Player)
    {
        playerType = type;
    }
    public float evasion { get; protected set; }
    protected enum PlayerType
    {
        None,
        Knight = 1,
        Archor = 2,
        Mage = 3,
        Thief = 4
    }
    protected PlayerType playerType = PlayerType.None;
}

class Knight : Player
{
    public Knight() : base(PlayerType.Knight)
    {
        SetInfo(250, 10);
        maxHp = GetHp();
        evasion = 10f;
        damageDifference = 0.1f;
        name = "전사";
    }
}

class Archor : Player
{
    public Archor() : base(PlayerType.Knight)
    {
        SetInfo(120, 15);
        maxHp = GetHp();
        evasion = 60f;
        damageDifference = 0.2f;
        name = "궁수";
    }
}
class Mage : Player
{
    public Mage() : base(PlayerType.Knight)
    {
        SetInfo(60, 30);
        maxHp = GetHp();
        evasion = 30f;
        damageDifference = 0.4f;
        name = "마법사";
    }
}
class Thief : Player
{
    public Thief() : base(PlayerType.Knight)
    {
        SetInfo(75, 22);
        maxHp = GetHp();
        evasion = 95f;
        damageDifference = 0.33f;
        name = "도적";
    }
}