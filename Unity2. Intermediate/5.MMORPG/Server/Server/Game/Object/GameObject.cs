using Google.Protobuf.Protocol;
using Server.Game.Utility;

namespace Server.Game; 

public class GameObject {
    public GameObject(GameObjectType type) {
        ObjectType = type;
    }
    
    public GameObjectType ObjectType { get; protected set; }
    public GameRoom Room { get; set; }
    public ObjectInfo Info { get; set; } = new ObjectInfo() { PosInfo = new PositionInfo() };
    
    public int Id {
        get => Info.ObjectId;
        set => Info.ObjectId = value;
    }
    
    public PositionInfo PosInfo => Info.PosInfo;

    public Vector2Int CellPos {
        get => new Vector2Int(PosInfo.PosX, PosInfo.PosY);
        set {
            PosInfo.PosX = value.x;
            PosInfo.PosY = value.y;
        }
    }
}