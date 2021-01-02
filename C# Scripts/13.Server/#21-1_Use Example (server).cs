using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace Server
{
	class ClientSession : PacketSession
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}가 접속했습니다.");

			Thread.Sleep(5000);

			Disconnect();
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}의 접속이 종료되었습니다.");
		}

		public override void OnReceivePacket(ArraySegment<byte> buffer)
		{
			ushort count = 0;

			// 무슨 패킷인지는 파악해야 하니 헤더 부분은 직접 읽어야 한다.
			ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
			count += sizeof(ushort);
			ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
			count += sizeof(ushort);

			switch ((PacketID)id)
			{
				case PacketID.PlayerInfoReq:
					PlayerInfoReq packet = new PlayerInfoReq();
					packet.DeSerialize(buffer);
					Console.WriteLine($"Player Info Required : {packet.playerId} ({packet.playerName})");

					foreach (PlayerInfoReq.SkillInfo skill in packet.skills)
					{
						Console.WriteLine($"Skill [{skill.id}] : {skill.level} 레벨 (지속시간: {skill.duration}초)");
					}
					break;
			}

			Console.WriteLine($"[Packet Received] PacketID : {id}, Size : {size}");
		}

		public override void OnSend(int byteCount)
		{
			Console.WriteLine($"전송된 바이트: {byteCount}");
		}
	}

}
