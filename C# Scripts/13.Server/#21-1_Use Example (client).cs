using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace DummyClient
{
	
	class ServerSession : Session
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}에 연결되었습니다.");

			PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, playerName = "태사단"};
			packet.skills.Add(new PlayerInfoReq.Skill() { id = 101, level = 5, duration = 0.3f });
			packet.skills.Add(new PlayerInfoReq.Skill() { id = 102, level = 7, duration = 20f });
			packet.skills.Add(new PlayerInfoReq.Skill() { id = 105, level = 1, duration = 3f });
			packet.skills.Add(new PlayerInfoReq.Skill() { id = 201, level = 2, duration = 0.05f });

			for (int i = 0; i < 5; i++)
			{
				// 이제 패킷 타입만 PlayerInfoReq로 선언하면,
				// Serialize()를 통해 해당 클래스 구성을 몰라도 직렬화할 수 있다.
				ArraySegment<byte> sendBuff = packet.Serialize();

				if (sendBuff != null)
					Send(sendBuff);
			}
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}의 접속이 종료되었습니다.");
		}

		public override int OnReceive(ArraySegment<byte> buffer)
		{
			string receivedData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
			Console.WriteLine($"[From Server] {receivedData}");

			return buffer.Count;
		}

		public override void OnSend(int byteCount)
		{
			Console.WriteLine($"전송된 바이트: {byteCount}");
		}
	}

}
