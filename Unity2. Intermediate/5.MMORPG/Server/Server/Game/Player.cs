using Google.Protobuf.Protocol;

namespace Server.Game; 

public class Player {
    public GameRoom Room { get; set; }
    public ClientSession Session { get; set; }

    public PlayerInfo Info { get; set; } = new PlayerInfo();
}