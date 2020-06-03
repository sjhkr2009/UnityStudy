using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
