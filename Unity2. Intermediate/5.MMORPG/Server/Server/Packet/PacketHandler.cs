using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using ServerCore;
using System;

class PacketHandler {
	public static void C_MoveHandler(PacketSession session, IMessage packet) {
		C_Move movePacket = packet as C_Move;
		ClientSession clientSession = session as ClientSession;
		
		Console.WriteLine($"C_Move ({movePacket?.PosInfo?.PosX}, {movePacket?.PosInfo?.PosY})");

		if (movePacket == null || clientSession?.MyPlayer?.Room == null) return;

		// 서버상의 좌표 이동
		var info = clientSession.MyPlayer.Info;
		info.PosInfo = movePacket.PosInfo;
		
		// 타 플레이어들에게 전송
		var resPacket = new S_Move();
		resPacket.PlayerId = clientSession.MyPlayer.Info.PlayerId;
		resPacket.PosInfo = movePacket.PosInfo;

		clientSession.MyPlayer.Room.Broadcast(resPacket);
	}
}
