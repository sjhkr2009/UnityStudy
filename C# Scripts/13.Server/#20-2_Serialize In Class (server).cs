using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace Server
{
	// 직렬화 / 역직렬화를 패킷 클래스 내에 패킹하기. (서버 쪽)

	// 패킷 클래스 부분은 클라이언트와 동일하게 복붙한다.
	// 나중에 패킷 클래스는 따로 만들어 공용으로 쓸 예정.
	public abstract class Packet
	{
		public ushort packetSize;
		public ushort packetId;

		public abstract ArraySegment<byte> Serialize();
		public abstract void DeSerialize(ArraySegment<byte> s);
	}

	public enum PacketID : ushort
	{
		PlayerInfoReq = 1,
		PlayerInfoOk = 2
	}

	class PlayerInfoReq : Packet
	{
		public long playerId;

		public PlayerInfoReq()
		{
			packetId = (ushort)PacketID.PlayerInfoReq;
		}

		public override ArraySegment<byte> Serialize()
		{
			ArraySegment<byte> arr = SendBufferHelper.Open(4096);

			ushort count = 0;
			bool success = true;

			count += 2;
			success &= BitConverter.TryWriteBytes(new Span<byte>(arr.Array, arr.Offset + count, arr.Count - count), packetId);
			count += 2;
			success &= BitConverter.TryWriteBytes(new Span<byte>(arr.Array, arr.Offset + count, arr.Count - count), playerId);
			count += 8;

			success &= BitConverter.TryWriteBytes(new Span<byte>(arr.Array, arr.Offset, arr.Count), count);

			if (!success)
				return null;

			return SendBufferHelper.Close(count);
		}
		public override void DeSerialize(ArraySegment<byte> s)
		{
			ushort count = 4;

			playerId = BitConverter.ToInt64(new ReadOnlySpan<byte>(s.Array, s.Offset + count, s.Count - count));
			count += 8;
		}
	}

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
			count += 2;
			ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
			count += 2;

			switch ((PacketID)id)
			{
				// * 수정된 부분
				// 이제 패킷ID에 맞는 클래스(PlayerInfoReq)를 생성하고,
				// 받은 버퍼를 DeSerialize()에 인자로 넘겨 클래스 정보를 역직렬화하여 채운다.
				case PacketID.PlayerInfoReq:
					PlayerInfoReq packet = new PlayerInfoReq();
					packet.DeSerialize(buffer);
					Console.WriteLine($"Player Info Required : {packet.playerId}");
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
