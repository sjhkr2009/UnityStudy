using Google.Protobuf.Protocol;
using Server.Game.Utility;

namespace Server.Game;

public class Player : GameObject {
    public Player() : base(GameObjectType.Player) { }
    
    public ClientSession Session { get; set; }

    public Vector2Int CellPos {
        get => new Vector2Int(Info.PosInfo.PosX, Info.PosInfo.PosY);
        set {
            Info.PosInfo.PosX = value.x;
            Info.PosInfo.PosY = value.y;
        }
    }
    
    
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