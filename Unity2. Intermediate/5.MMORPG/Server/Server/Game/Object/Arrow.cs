using System;
using Google.Protobuf.Protocol;

namespace Server.Game; 

public class Arrow : Projectile {
    public GameObject Owner { get; set; }

    private long nextMoveTick = 0;
    public override void Update() {
        if (Owner == null || Room == null) return;
        if (nextMoveTick >= Environment.TickCount64) return;

        nextMoveTick = Environment.TickCount64 + 50;

        var destPos = GetFrontCellPos();
        if (Room.Map.CanGo(destPos)) {
            CellPos = destPos;
            
            S_Move movePacket = new S_Move() {
                ObjectId = Id,
                PosInfo = PosInfo
            };
            Room.Broadcast(movePacket);
        } else {
            var hit = Room.Map.Find(destPos);
            if (hit != null) {
                // TODO: 피격 판정 
            }
            Room.LeaveGame(Id);
        }
    }
}