using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class PacketHandler
{
	public static void S_ChatHandler(PacketSession packetSession, IPacket packet)
	{
		S_Chat chatPacket = packet as S_Chat;
		ServerSession serverSession = packetSession as ServerSession;

		//if(chatPacket.playerId == 1)
		{
			Debug.Log(chatPacket.chat);

			// 유니티 관련 동작은 유니티 메인 스레드에서 실행해야 한다.
			// 백그라운드 스레드에서 호출 시 실행되지 않는다.
			GameObject go = GameObject.Find("Player");
			if (go == null)
				Debug.Log("Player not found.");
			else
				Debug.Log("Player found!");
		}
	}
}
