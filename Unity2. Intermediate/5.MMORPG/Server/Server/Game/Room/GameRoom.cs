using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Data;
using Server.Game.Utility;

namespace Server.Game; 

public class GameRoom {
    private object _lock = new object();
    public int RoomId { get; set; }

    private Dictionary<int, Player> players = new();
    private Dictionary<int, Monster> monsters = new();
    private Dictionary<int, Projectile> projectiles = new();

    public Map Map { get; private set; } = new Map();
    
    public void Initialize(int mapId) {
        Map.LoadMap(mapId);
    }

    public void EnterGame(GameObject gameObject) {
        if (gameObject == null) return;

        var type = gameObject.GetObjectType();

        lock (_lock) {
            gameObject.Room = this;
            
            if (type == GameObjectType.Player && gameObject is Player player) {
                players.Add(gameObject.Id, player);
                
                // 플레이어라면 본인한테 입장 패킷 및 다른 사람들 정보 전송
                S_EnterGame myEnterPacket = new S_EnterGame {
                    Player = gameObject.Info
                };
                player.Session.Send(myEnterPacket);
            
                S_Spawn mySpawnPacket = new S_Spawn() {
                    Objects = { players.Values.Except(new[] { gameObject }).Select(otherPlayer => otherPlayer.Info) }
                };
                player.Session.Send(mySpawnPacket);
            } else if (type == GameObjectType.Monster && gameObject is Monster monster) {
                monsters.Add(monster.Id, monster);
            } else if (type == GameObjectType.Projectile && gameObject is Projectile projectile) {
                projectiles.Add(projectile.Id, projectile);
            }

            // 다른 플레이어들에게 입장한 오브젝트 정보 전송
            S_Spawn otherSpawnPacket = new S_Spawn() { Objects = { gameObject.Info } };
            players.Values.ForEach(p => {
                if (p.Id != gameObject.Id) p.Session.Send(otherSpawnPacket);
            });
        }
    }

    public void LeaveGame(int objectId) {
        var type = ObjectManager.GetObjectTypeBy(objectId);

        lock (_lock) {
            if (type == GameObjectType.Player) {
                if (!players.Remove(objectId, out var leavePlayer)) return;
                
                leavePlayer.Room = null;
                Map.ApplyLeave(leavePlayer);
            
                // 본인한테 퇴장 패킷 전송
                S_LeaveGame leavePacket = new S_LeaveGame();
                leavePlayer.Session.Send(leavePacket);
            } else if (type == GameObjectType.Monster) {
                if (!monsters.Remove(objectId, out var leaveMonster)) return;
                
                leaveMonster.Room = null;
                Map.ApplyLeave(leaveMonster);
            } else if (type == GameObjectType.Projectile) {
                if (!projectiles.Remove(objectId, out var leaveProjectile)) return;
                
                leaveProjectile.Room = null;
            }

            // 다른 플레이어들에게 퇴장하는 플레이어 정보 전송
            S_Despawn despawnPacket = new S_Despawn() { ObjectIds = { objectId } };
            players.Values.Where(p => p.Id != objectId).ForEach(p => p.Session.Send(despawnPacket));
        }
    }

    public void Update() {
        lock (_lock) {
            foreach (var projectile in projectiles.Values) {
                projectile.Update();
            }
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
                if (!Map.CanGo(new Vector2Int(destPos.PosX, destPos.PosY))) return;
            }

            playerInfo.PosInfo.State = destPos.State;
            playerInfo.PosInfo.MoveDir = destPos.MoveDir;
            Map.ApplyMove(player, new Vector2Int(destPos.PosX, destPos.PosY));
		
            // 타 플레이어들에게 전송
            var resPacket = new S_Move();
            resPacket.ObjectId = player.Info.ObjectId;
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
            resPacket.ObjectId = info.ObjectId;
            resPacket.Info.SkillId = 1; // 스킬 정보는 보통 json이나 xml 등의 외부 파일에서 따로 관리한다. 여기선 임의로 1을 넣음.

            Broadcast(resPacket);

            if (!DataManager.SkillDict.TryGetValue(skillPacket.Info.SkillId, out var skillData)) return;

            switch (skillData.skillType) {
                case SkillType.SkillAuto:
                    // 근접 공격
                    Vector2Int skillPos = player.GetFrontCellPos(info.PosInfo.MoveDir);
                    var target = Map.Find(skillPos);
                    if (target != null) {
                        Console.WriteLine($"GameObject Hit: {target.Info.Name}");
                    }
                    break;
                case SkillType.SkillProjectile:
                    // 원거리 투사체
                    var arrow = ObjectManager.Create<Arrow>();
                    if (arrow == null) return;

                    arrow.Owner = player;
                    arrow.Data = skillData;
                    
                    arrow.PosInfo.State = CreatureState.Moving;
                    arrow.PosInfo.MoveDir = player.PosInfo.MoveDir;
                    arrow.PosInfo.PosX = player.PosInfo.PosX;
                    arrow.PosInfo.PosY = player.PosInfo.PosY;
                    arrow.Speed = skillData.Projectile.speed;
                
                    EnterGame(arrow);
                    break;
            }
        }
    }
}