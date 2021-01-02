using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace DummyClient
{
	// Serialization (직렬화) : 정보를 다른 컴퓨터 환경에 전송할 수 있는 형태로 만드는 것.
	//	ㄴ 여기서는 패킷을 일렬로 나열된 비트 배열에 저장해 보내는 변환 과정을 의미한다.
	//	ㄴ 반대로 Deserialization(역직렬화)은 받은 패킷을 패킷ID를 통해 적절한 타입으로 변환, 재구성하는 것을 의미한다. (서버쪽 ClientSession 참고)

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

	// 클라이언트에서 접속한 서버의 역할을 할 세션. 서버의 대리인 역할을 한다.
	class ServerSession : Session
	{
		// 참고: unsafe를 이용하여 C++의 포인터 조작과 유사한 동작을 실행할 수 있다.
		//		 이는 추후 다룰 예정.
		/*
		static unsafe void ToBytes(byte[] array, int offset, ulong value)
		{
			fixed (byte* ptr = &array[offset])
				*(ulong*)ptr = value;
		}
		*/

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}에 연결되었습니다.");

			PlayerInfoReq packet = new PlayerInfoReq() { 
				packetId = (ushort)PacketID.PlayerInfoReq, 
				playerId = 1001
			};

			for (int i = 0; i < 5; i++)
			{
				ArraySegment<byte> arr = SendBufferHelper.Open(4096);

				// * 수정된 부분
				// TryWriteBytes를 이용하여, GetByte로 변환 후 복사하는 과정을 한 번에 실행한다.
				// success에 비트 연산을 통해 한 번이라도 실패했다면 false로 바뀌게 한다.
				ushort count = 0;
				bool success = true;

				// 패킷 크기가 들어갈 부분은 2바이트 비워준다.
				count += 2;
				success &= BitConverter.TryWriteBytes(new Span<byte>(arr.Array, arr.Offset + count, arr.Count - count), packet.packetId);
				count += 2;
				success &= BitConverter.TryWriteBytes(new Span<byte>(arr.Array, arr.Offset + count, arr.Count - count), packet.playerId);
				count += 8;

				// 패킷의 크기는 미리 패킷에 정의해두지 않고, 보낼 때 크기를 센 다음 마지막에 count로 입력해준다.
				success &= BitConverter.TryWriteBytes(new Span<byte>(arr.Array, arr.Offset, arr.Count), count);

				ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);

				if (success)
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
