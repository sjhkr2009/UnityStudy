using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void S_BroadcastEnterGameHandler(PacketSession packetSession, IPacket packet)
	{
		S_BroadcastEnterGame enterPkt = packet as S_BroadcastEnterGame;
		ServerSession serverSession = packetSession as ServerSession;

	}
	public static void S_BroadcastLeaveGameHandler(PacketSession packetSession, IPacket packet)
	{
		S_BroadcastLeaveGame leavePkt = packet as S_BroadcastLeaveGame;
		ServerSession serverSession = packetSession as ServerSession;

	}
	public static void S_PlayerListHandler(PacketSession packetSession, IPacket packet)
	{
		S_PlayerList pkt = packet as S_PlayerList;
		ServerSession serverSession = packetSession as ServerSession;

	}
	public static void S_BroadcastMoveHandler(PacketSession packetSession, IPacket packet)
	{
		S_BroadcastMove movePkt = packet as S_BroadcastMove;
		ServerSession serverSession = packetSession as ServerSession;

	}
}
