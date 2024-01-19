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

    public StatInfo Stat => Info.StatInfo;

    public float Speed {
        get => Stat.Speed;
        set => Stat.Speed = value;
    }
    
    public PositionInfo PosInfo => Info.PosInfo;

    public Vector2Int CellPos {
        get => new Vector2Int(PosInfo.PosX, PosInfo.PosY);
        set {
            PosInfo.PosX = value.x;
            PosInfo.PosY = value.y;
        }
    }

    public Vector2Int GetFrontCellPos() => GetFrontCellPos(PosInfo.MoveDir);
    public Vector2Int GetFrontCellPos(MoveDir dir) {
        var cellPos = CellPos;

        switch (dir) {
            case MoveDir.Up:
                cellPos += Vector2Int.up;
                break;
            case MoveDir.Down:
                cellPos += Vector2Int.down;
                break;
            case MoveDir.Right:
                cellPos += Vector2Int.right;
                break;
            case MoveDir.Left:
                cellPos += Vector2Int.left;
                break;
        }

        return cellPos;
    }
}