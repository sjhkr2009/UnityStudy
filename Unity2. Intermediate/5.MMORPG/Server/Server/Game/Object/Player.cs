using Google.Protobuf.Protocol;
using Server.Game.Utility;

namespace Server.Game;

public class Player : GameObject {
    public Player() : base(GameObjectType.Player) { }
    
    public ClientSession Session { get; set; }
}