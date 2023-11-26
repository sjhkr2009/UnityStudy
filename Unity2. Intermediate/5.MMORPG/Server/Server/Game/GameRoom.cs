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

    public void HandleMove(Player player, C_Move movePacket) {
        if (player == null) return;

        lock (_lock) {
            // 서버상의 좌표 이동
            var info = player.Info;
            info.PosInfo = movePacket.PosInfo;
		
            // 타 플레이어들에게 전송
            var resPacket = new S_Move();
            resPacket.PlayerId = player.Info.PlayerId;
            resPacket.PosInfo = movePacket.PosInfo;

            Broadcast(resPacket);
        }
    }
    
    public void HandleSkill(Player player, C_Skill skillPacket) {
        if (player == null) return;

        lock (_lock) {
            var info = player.Info;
            if (info.PosInfo.State == CreatureState.Idle) return;
            
            // TODO: 스킬 사용 가능여부 체크 (유효성 검사)
            // ...
            
            // 통과 시 스킬 사용처리
            info.PosInfo.State = CreatureState.Skill;
		
            // 타 플레이어들에게 전송
            var resPacket = new S_Skill() {
                Info = new SkillInfo()
            };
            resPacket.PlayerId = info.PlayerId;
            resPacket.Info.SkillId = 1; // 스킬 정보는 보통 json이나 xml 등의 외부 파일에서 따로 관리한다. 여기선 임의로 1을 넣음.

            Broadcast(resPacket);
            
            // TODO: 데미지 판정
        }
    }
}