class Player : Creature
{
    protected Player(PlayerType type) : base(CreatureType.Player)
    {
        playerType = type;
    }

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
    static int knightCount = 0;
    public Knight() : base(PlayerType.Knight)
    {
        SetInfo(200, 10);
        knightCount++;
        name = "기사 " + knightCount;
    }
}
