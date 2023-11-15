using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Protocol;

namespace Server.Game; 

public class GameRoom {
    private object _lock = new object();
    public int RoomId { get; set; }

    private List<Player> players = new List<Player>();

    public void EnterGame(Player newPlayer) {
        if (newPlayer == null) return;

        lock (_lock) {
            players.Add(newPlayer);
            newPlayer.Room = this;
            
            // 본인한테 입장 패킷 및 다른 사람들 정보 전송
            S_EnterGame myEnterPacket = new S_EnterGame {
                Player = newPlayer.Info
            };
            newPlayer.Session.Send(myEnterPacket);
            
            S_Spawn mySpawnPacket = new S_Spawn() {
                Players = { players.Except(new[] { newPlayer }).Select(otherPlayer => otherPlayer.Info) }
            };
            newPlayer.Session.Send(mySpawnPacket);
            
            // 다른 플레이어들에게 입장한 플레이어 정보 전송
            S_Spawn otherSpawnPacket = new S_Spawn() { Players = { newPlayer.Info } };
            players.ForEach(p => {
                if (p != newPlayer) p.Session.Send(otherSpawnPacket);
            });
        }
    }

    public void LeaveGame(int playerId) {
        lock (_lock) {
            var leavePlayer = players.Find(p => p.Info.PlayerId == playerId);
            if (leavePlayer == null) return;

            players.Remove(leavePlayer);
            leavePlayer.Room = null;
            
            // 본인한테 퇴장 패킷 전송
            S_LeaveGame leavePacket = new S_LeaveGame();
            leavePlayer.Session.Send(leavePacket);
            
            // 다른 플레이어들에게 퇴장하는 플레이어 정보 전송
            S_Despawn despawnPacket = new S_Despawn() { PlayerIds = { leavePlayer.Info.PlayerId } };
            players.ForEach(p => p.Session.Send(despawnPacket));
        }
    }

    public void Broadcast(IMessage packet) {
        lock (_lock) {
            players.ForEach(p => p.Session.Send(packet));
        }
    }
}