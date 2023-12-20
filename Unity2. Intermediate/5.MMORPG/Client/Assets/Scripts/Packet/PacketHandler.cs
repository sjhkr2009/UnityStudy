using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler {
	public static void S_MoveHandler(PacketSession session, IMessage packet) {
		S_Move movePacket = packet as S_Move;
		ServerSession serverSession = session as ServerSession;

		if (movePacket == null) return;

		var player = Director.Object.Find<BaseController>(movePacket.ObjectId);
		if (!player) return;

		player.PositionInfo = movePacket.PosInfo;
	}
	
	public static void S_SkillHandler(PacketSession session, IMessage packet) {
		S_Skill skillPacket = packet as S_Skill;
		ServerSession serverSession = session as ServerSession;

		if (skillPacket == null) return;

		var player = Director.Object.Find<PlayerController>(skillPacket.ObjectId);
		if (!player) return;

		player.UseSkill(skillPacket.Info.SkillId);
	}

	public static void S_EnterGameHandler(PacketSession session, IMessage packet) {
		S_EnterGame enterGamePacket = packet as S_EnterGame;
		if (enterGamePacket == null) return;
		
		Director.Object.Add(enterGamePacket.Player, true);
		Debug.Log($"[S_EnterGameHandler] Entered : {enterGamePacket?.Player.Name}");
	}
	
	public static void S_SpawnHandler(PacketSession session, IMessage packet) {
		S_Spawn spawnPacket = packet as S_Spawn;
		if (spawnPacket == null) return;

		foreach (var info in spawnPacket.Objects) {
			Director.Object.Add(info);
		}
		
		Debug.Log($"[S_SpawnHandler] Others : {spawnPacket?.Objects}");
	}
	
	public static void S_DespawnHandler(PacketSession session, IMessage packet) {
		S_Despawn despawnPacket = packet as S_Despawn;
		if (despawnPacket == null) return;

		foreach (var id in despawnPacket.ObjectIds) {
			Director.Object.Remove(id);
		}
		
		Debug.Log($"[S_DespawnHandler] Remain : {despawnPacket?.ObjectIds}");
	}
	
	public static void S_LeaveGameHandler(PacketSession session, IMessage packet) {
		Director.Object.RemoveMyPlayer();
		Debug.Log($"[S_LeaveGameHandler] -");
	}
}
