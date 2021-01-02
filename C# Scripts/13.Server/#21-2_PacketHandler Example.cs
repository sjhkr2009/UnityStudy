using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	// 클라/서버에 각각 작성한다. 수신된 패킷 종류에 따라 어떻게 처리할지 작성한다.
	// 해당 함수는 매니저에서 호출된다.

	public static void C_PlayerInfoReqHandler(PacketSession packetSession, IPacket packet)
	{
		C_PlayerInfoReq p = packet as C_PlayerInfoReq;

		Console.WriteLine($"Player Info Required : {p.playerId} ({p.playerName})");

		foreach (C_PlayerInfoReq.Skill skill in p.skills)
		{
			Console.WriteLine($"Skill [{skill.id}] : {skill.level} 레벨 (지속시간: {skill.duration}초)");
		}
	}
}
