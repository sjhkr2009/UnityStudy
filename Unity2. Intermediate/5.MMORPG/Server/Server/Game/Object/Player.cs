using Google.Protobuf.Protocol;
using Server.Game.Utility;

namespace Server.Game;

public class Player : GameObject {
    public Player() : base(GameObjectType.Player) {
        Speed = 10f;
    }
    
    public ClientSession Session { get; set; }
}