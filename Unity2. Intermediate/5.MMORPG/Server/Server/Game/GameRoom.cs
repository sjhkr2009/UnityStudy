using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game.Utility;

namespace Server.Game; 

public class GameRoom {
    private object _lock = new object();
    public int RoomId { get; set; }

    private Dictionary<int, Player> players = new();

    private Map map = new Map();
    
    public void Initialize(int mapId) {
        map.LoadMap(mapId);
    }

    public void EnterGame(Player newPlayer) {
        if (newPlayer == null) return;

        lock (_lock) {
            players.Add(newPlayer.Info.ObjectId, newPlayer);
            newPlayer.Room = this;
            
            // 본인한테 입장 패킷 및 다른 사람들 정보 전송
            S_EnterGame myEnterPacket = new S_EnterGame {
                Player = newPlayer.Info
            };
            newPlayer.Session.Send(myEnterPacket);
            
            S_Spawn mySpawnPacket = new S_Spawn() {
                Objects = { players.Values.Except(new[] { newPlayer }).Select(otherPlayer => otherPlayer.Info) }
            };
            newPlayer.Session.Send(mySpawnPacket);
            
            // 다른 플레이어들에게 입장한 플레이어 정보 전송
            S_Spawn otherSpawnPacket = new S_Spawn() { Objects = { newPlayer.Info } };
            players.Values.ForEach(p => {
                if (p != newPlayer) p.Session.Send(otherSpawnPacket);
            });
        }
    }

    public void LeaveGame(int playerId) {
        lock (_lock) {
            players.TryGetValue(playerId, out var leavePlayer);
            if (leavePlayer == null) return;

            players.Remove(playerId);
            leavePlayer.Room = null;
            
            // 본인한테 퇴장 패킷 전송
            S_LeaveGame leavePacket = new S_LeaveGame();
            leavePlayer.Session.Send(leavePacket);
            
            // 다른 플레이어들에게 퇴장하는 플레이어 정보 전송
            S_Despawn despawnPacket = new S_Despawn() { PlayerIds = { leavePlayer.Info.ObjectId } };
            players.Values.ForEach(p => p.Session.Send(despawnPacket));
        }
    }

    public void Broadcast(IMessage packet) {
        lock (_lock) {
            players.Values.ForEach(p => p.Session.Send(packet));
        }
    }

    public void HandleMove(Player player, C_Move movePacket) {
        if (player == null) return;

        lock (_lock) {
            // 검증
            var playerInfo = player.Info;
            var destPos = movePacket.PosInfo;
            
            // 위치 이동 시 갈 수 있는 지역인지 확인한다. 
            if (destPos.PosX != playerInfo.PosInfo.PosX || destPos.PosY != playerInfo.PosInfo.PosY) {
                if (!map.CanGo(new Vector2Int(destPos.PosX, destPos.PosY))) return;
            }

            playerInfo.PosInfo.State = destPos.State;
            playerInfo.PosInfo.MoveDir = destPos.MoveDir;
            map.ApplyMove(player, new Vector2Int(destPos.PosX, destPos.PosY));
		
            // 타 플레이어들에게 전송
            var resPacket = new S_Move();
            resPacket.PlayerId = player.Info.ObjectId;
            resPacket.PosInfo = destPos;

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
            resPacket.PlayerId = info.ObjectId;
            resPacket.Info.SkillId = 1; // 스킬 정보는 보통 json이나 xml 등의 외부 파일에서 따로 관리한다. 여기선 임의로 1을 넣음.

            Broadcast(resPacket);
            
            // 데미지 판정 (스킬이 많아지면 클래스화하겠지만, 일단 id별로 처리)
            if (skillPacket.Info.SkillId == 1) {
                // 1번 스킬 - 근접 공격
                Vector2Int skillPos = player.GetFrontCellPos(info.PosInfo.MoveDir);
                Player target = map.Find(skillPos);
                if (target != null) {
                    Console.WriteLine($"Player Hit: {target.Info.Name}");
                }
            } else if (skillPacket.Info.SkillId == 2) {
                // 2번 스킬 - 원거리 투사체
                
            }
            
            
            
        }
    }
}