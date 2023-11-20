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

		var player = Director.Object.Find<BaseController>(movePacket.PlayerId);
		if (!player) return;

		player.PositionInfo = movePacket.PosInfo;
	}

	public static void S_EnterGameHandler(PacketSession session, IMessage packet) {
		S_EnterGame enterGamePacket = packet as S_EnterGame;
		if (enterGamePacket == null) return;
		
		Director.Object.AddPlayer(enterGamePacket.Player, true);
		Debug.Log($"[S_EnterGameHandler] Entered : {enterGamePacket?.Player.Name}");
	}
	
	public static void S_SpawnHandler(PacketSession session, IMessage packet) {
		S_Spawn spawnPacket = packet as S_Spawn;
		if (spawnPacket == null) return;

		foreach (var playerInfo in spawnPacket.Players) {
			Director.Object.AddPlayer(playerInfo);
		}
		
		Debug.Log($"[S_SpawnHandler] Others : {spawnPacket?.Players}");
	}
	
	public static void S_DespawnHandler(PacketSession session, IMessage packet) {
		S_Despawn despawnPacket = packet as S_Despawn;
		if (despawnPacket == null) return;

		foreach (var playerId in despawnPacket.PlayerIds) {
			Director.Object.Remove(playerId);
		}
		
		Debug.Log($"[S_DespawnHandler] Remain : {despawnPacket?.PlayerIds}");
	}
	
	public static void S_LeaveGameHandler(PacketSession session, IMessage packet) {
		Director.Object.RemoveMyPlayer();
		Debug.Log($"[S_LeaveGameHandler] -");
	}
}
