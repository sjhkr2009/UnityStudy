using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class PacketHandler
{
	public static void S_BroadcastEnterGameHandler(PacketSession packetSession, IPacket packet)
	{
		S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
		ServerSession serverSession = packetSession as ServerSession;

		PlayerManager.Instance.EnterGame(pkt);
	}
	public static void S_BroadcastLeaveGameHandler(PacketSession packetSession, IPacket packet)
	{
		S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
		ServerSession serverSession = packetSession as ServerSession;

		PlayerManager.Instance.LeaveGame(pkt);
	}
	public static void S_PlayerListHandler(PacketSession packetSession, IPacket packet)
	{
		S_PlayerList pkt = packet as S_PlayerList;
		ServerSession serverSession = packetSession as ServerSession;

		PlayerManager.Instance.Add(pkt);
	}
	public static void S_BroadcastMoveHandler(PacketSession packetSession, IPacket packet)
	{
		S_BroadcastMove pkt = packet as S_BroadcastMove;
		ServerSession serverSession = packetSession as ServerSession;

		PlayerManager.Instance.Move(pkt);
	}
}
