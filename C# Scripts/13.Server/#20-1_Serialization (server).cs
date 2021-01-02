using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace Server
{
	class Packet
	{
		public ushort packetSize;
		public ushort packetId;
	}

	public enum PacketID : ushort
	{
		PlayerInfoReq = 1,
		PlayerInfoOk = 2
	}

	class PlayerInfoReq : Packet
	{
		public long playerId;
	}

	class PlayerInfoOk : Packet
	{
		public int hp;
		public int attack;
	}

	// 서버에 접속한 클라이언트와 통신하기 위한 세션.
	// 서버에서 연결하는 대상이 클라이언트만 있는 것은 아니므로(다른 서버와도 통신 가능) 명시적으로 이름을 지어준다.
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
			// * 수정된 부분
			// PacketID를 통해 받은 패킷을 어떻게 처리할 지 결정한다.
			ushort count = 0;

			ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
			count += 2;
			ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
			count += 2;

			switch ((PacketID)id)
			{
				case PacketID.PlayerInfoReq:
					long playerId = BitConverter.ToInt64(buffer.Array, buffer.Offset + count);
					count += 8;
					Console.WriteLine($"Player Info Required : {playerId}");
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
