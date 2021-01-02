using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace DummyClient
{
	// 직렬화 / 역직렬화를 패킷 클래스 내에 패킹하기.
	
	public abstract class Packet
	{
		public ushort packetSize;
		public ushort packetId;

		// + 추가된 부분
		// 패킷을 추상 클래스로 만들고, Serialize / DeSerialize 함수를 선언한다.
		// Serialize	: 이 클래스의 정보를 직렬화하여 바이트 배열 포인터로 반환한다.
		// DeSerialize	: 배열 정보를 입력받아 이 클래스의 변수에 내용을 채운다.
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

		// 패킷의 ID는 패킷 클래스마다 고유하므로 생성자로 지정한다.
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
			ushort count = 4;   // 헤더 + 패킷ID = 4바이트

			// Serialize는 이쪽에서 처리하니 상관없지만, DeSerialize는 상대의 정보를 받아 처리하므로 문제가 생길 수 있다.
			// 상대방이 패킷을 조작하여 8바이트 자료형인데 그보다 적게 보내는 등의 경우를 방지하기 위해 ReadOnlySpan을 사용한다.
			
			// ReadOnlySpan은 지정한 타입과 시작점을 바탕으로 메모리 주소의 특정 영역을 지정한다.
			// 매개변수로 들어온 버퍼가 비어 있어 해당 범위를 읽을 수 없다면 예외 처리될 것이다. (이후 Session에 의해 Disconnect() 처리됨)
			playerId = BitConverter.ToInt64(new ReadOnlySpan<byte>(s.Array, s.Offset + count, s.Count - count));

			// (참고) 기존 방식
			// playerId = BitConverter.ToInt64(buffer.Array, buffer.Offset + count);

			count += 8;
		}
	}

	class ServerSession : Session
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}에 연결되었습니다.");

			PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001 };

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
