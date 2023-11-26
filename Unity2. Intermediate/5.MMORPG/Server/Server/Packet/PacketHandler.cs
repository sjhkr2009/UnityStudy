using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using ServerCore;
using System;
using Server.Game;

class PacketHandler {
	public static void C_MoveHandler(PacketSession session, IMessage packet) {
		C_Move movePacket = packet as C_Move;
		ClientSession clientSession = session as ClientSession;
		
		// 클래스 내부는 멀티스레드 환경에서 언제든 변경될 수 있으니, MyPlayer, Room 변수를 지역 변수로 저장해놓고 사용한다. 
		var player = clientSession?.MyPlayer;
		var room = player?.Room;
		if (movePacket == null || room == null) return;

		room.HandleMove(player, movePacket);
	}
	
	public static void C_SkillHandler(PacketSession session, IMessage packet) {
		C_Skill skillPacket = packet as C_Skill;
		ClientSession clientSession = session as ClientSession;
		
		var player = clientSession?.MyPlayer;
		var room = player?.Room;
		if (skillPacket == null || room == null) return;

		room.HandleSkill(player, skillPacket);
	}
}
