using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

// * 가변 데이터 직렬화 1 - 문자열 전송

namespace DummyClient
{
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
		public string playerName;
		public string nickName;

		public PlayerInfoReq()
		{
			packetId = (ushort)PacketID.PlayerInfoReq;
		}

		public override ArraySegment<byte> Serialize()
		{
			ArraySegment<byte> arr = SendBufferHelper.Open(4096);

			ushort count = 0;
			bool success = true;

			// * 수정된 부분
			// Span.Slice 를 이용하여, 직렬화할 영역을 지정할 때 Span을 부분적으로 잘라서 사용한다.
			// Span은 받아온 버퍼의 메모리 영역을 가리키도록 초기화하여 한 번만 선언한다.
			Span<byte> span = new Span<byte>(arr.Array, arr.Offset, arr.Count);

			// Span에서 데이터가 입력된 양(count)만큼 뺀 공간을 지정한다. 첫 2바이트(데이터 크기)는 비워둔다.
			// Span.Slice : 해당 Span의 일부를 가리키는 Span을 반환한다. 매개변수로 입력된 Span은 변하지 않는다.
			count += sizeof(ushort);
			success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), packetId);
			count += sizeof(ushort);
			success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), playerId);
			count += sizeof(long);

			// + 추가된 부분
			// string 타입 직렬화하기
			//-----------------------------------------------------------------------------
			// 크기가 가변적이므로, 데이터 크기(ushort) + 데이터 형태로 저장한다.

			// 1. string 타입에 입력된 값의 바이트 수를 저장해둔다. (Unicode는 UTF-16을 의미)
			ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(playerName);

			// 2. string 데이터 크기(ushort 타입)를 먼저 직렬화한다.
			success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), nameLen);
			count += sizeof(ushort);

			// 3. 버퍼에 string 크기만큼의 영역을 지정해서, string 데이터를 인코딩하여 저장한다.
			Array.Copy(Encoding.Unicode.GetBytes(playerName), 0, arr.Array, arr.Offset + count, nameLen);
			count += nameLen;
			//-----------------------------------------------------------------------------
			// 좀 더 효율적인 방식으로, GetBytes에 오버로딩된 버전을 이용하면 지정한 메모리에 인코딩된 내용을 바로 넣을 수 있다.

			// 1. 메모리 주소를 지정하여 string을 변환해서 넣는다. 작성된 바이트 수가 반환된다. string의 크기를 입력하기 위해 2바이트는 비워둔다.
			// 매개 변수: (인코딩할 문자열, 문자열의 시작점, 문자열 길이(글자수), 작성할 배열, 작성할 배열의 시작점 인덱스)
			nameLen = (ushort)Encoding.Unicode.GetBytes(nickName, 0, nickName.Length, arr.Array, arr.Offset + count + sizeof(ushort));
			
			// 2. 비워둔 2바이트에 위에서 반환된 문자열 길이를 작성하고, '문자열 길이 + 2' 바이트만큼 count를 추가한다.
			success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), nameLen);
			count += sizeof(ushort);
			count += nameLen;
			//-----------------------------------------------------------------------------

			// 첫 2바이트에는 직렬화한 데이터 크기가 들어가므로 span의 시작부분에 최종 count를 입력해준다.
			success &= BitConverter.TryWriteBytes(span, count);

			if (!success)
				return null;

			return SendBufferHelper.Close(count);
		}
		public override void DeSerialize(ArraySegment<byte> arr)
		{
			// 헤더 + 패킷ID 만큼의 공간은 외부에서 읽었을테니 비워준다.
			ushort count = 0;
			count += sizeof(ushort);
			count += sizeof(ushort);

			// * 수정된 부분
			// 이쪽도 Span.Slice를 사용한다. 읽기 전용이므로 ReadOnlySpan을 사용한다.
			ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(arr.Array, arr.Offset, arr.Count);
			playerId = BitConverter.ToInt64(span.Slice(count, span.Length - count));
			count += sizeof(long);

			// + 추가된 부분
			// string 타입 데이터 역직렬화
			// 첫 2바이트를 변환하여 길이를 알아낸 다음, 해당 길이만큼 Span을 잘라서 string으로 변환한 데이터를 받는다.
			ushort nameLen = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
			count += sizeof(ushort);
			playerName = Encoding.Unicode.GetString(span.Slice(count, nameLen));
			count += nameLen;
			// 마찬가지로, Span을 이용하여 데이터를 읽어왔으니 nameLen 정보가 조작되어 있었다면 예외가 반환되어 해당 유저를 종료시킬 것이다.

			nameLen = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
			count += sizeof(ushort);
			nickName = Encoding.Unicode.GetString(span.Slice(count, nameLen));
			count += nameLen;
		}
	}

	class ServerSession : Session
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}에 연결되었습니다.");

			PlayerInfoReq packet = new PlayerInfoReq() { 
				playerId = 1001, 
				playerName = "지호",
				nickName = "태사단"
			};

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
