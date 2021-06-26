using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_LeaveGameHandler(PacketSession packetSession, IPacket packet)
	{
		ClientSession clientSession = packetSession as ClientSession;

		if (clientSession.Room == null)
			return;

		GameRoom room = clientSession.Room;
		room.Push(() => room.Leave(clientSession));
	}

	public static void C_MoveHandler(PacketSession packetSession, IPacket packet)
	{
		C_Move movePkt = packet as C_Move;
		ClientSession clientSession = packetSession as ClientSession;

		if (clientSession.Room == null)
			return;

		//Console.WriteLine($"[PacketHandler::C_MoveHandler] Move To ( {movePkt.posX}, {movePkt.posY}, {movePkt.posZ} )");

		GameRoom room = clientSession.Room;
		room.Push(() => room.Move(clientSession, movePkt));
	}
}
