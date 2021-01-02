using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

// * 가변 데이터 직렬화 2 - 리스트 전송

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

		// 구조체로 된 리스트를 전달한다고 가정한다.
		public struct SkillInfo
		{
			public int id;
			public short level;
			public float duration; 

			// 구조체 내에 직렬화/역직렬화 함수를 만든다.
			// 과정은 다른 자료형과 동일하며, 이 구조체 내의 변수에 대해서만 처리하면 된다.
			public bool Serialize(Span<byte> span, ref ushort count)
			{
				bool success = true;

				success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), id);
				count += sizeof(int);
				success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), level);
				count += sizeof(short);
				success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), duration);
				count += sizeof(float);

				return success;
			}

			public void DeSerialize(ReadOnlySpan<byte> span, ref ushort count)
			{
				id = BitConverter.ToInt32(span.Slice(count, span.Length - count));
				count += sizeof(int);
				level = BitConverter.ToInt16(span.Slice(count, span.Length - count));
				count += sizeof(short);
				duration = BitConverter.ToSingle(span.Slice(count, span.Length - count));
				count += sizeof(float);
			}
		}
		
		public List<SkillInfo> skills = new List<SkillInfo>();

		public PlayerInfoReq()
		{
			packetId = (ushort)PacketID.PlayerInfoReq;
		}

		public override ArraySegment<byte> Serialize()
		{
			ArraySegment<byte> arr = SendBufferHelper.Open(4096);

			ushort count = 0;
			bool success = true;

			Span<byte> span = new Span<byte>(arr.Array, arr.Offset, arr.Count);

			count += sizeof(ushort);
			success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), packetId);
			count += sizeof(ushort);
			success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), playerId);
			count += sizeof(long);

			// string
			ushort nameLen = (ushort)Encoding.Unicode.GetBytes(playerName, 0, playerName.Length, arr.Array, arr.Offset + count + sizeof(ushort));
			success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), nameLen);
			count += sizeof(ushort);
			count += nameLen;

			// * 추가된 부분
			//-------------------------------------------------------------------------------
			// 리스트는 기본적으로 string처럼 요소 개수를 먼저 계산하고, 컨테이너를 순회하며 직렬화를 반복하면 된다.
			success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort)skills.Count);
			count += sizeof(ushort);
			// count는 참조형(ref)으로 받으므로 따로 여기서 제어할 필요는 없다.
			foreach (SkillInfo skill in skills)
				success &= skill.Serialize(span, ref count);
			//-------------------------------------------------------------------------------

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

			ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(arr.Array, arr.Offset, arr.Count);
			playerId = BitConverter.ToInt64(span.Slice(count, span.Length - count));
			count += sizeof(long);

			// string
			ushort nameLen = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
			count += sizeof(ushort);
			playerName = Encoding.Unicode.GetString(span.Slice(count, nameLen));
			count += nameLen;

			// * 추가된 부분
			//-------------------------------------------------------------------------------
			// 리스트를 비우고, 리스트 내 요소 개수를 먼저 읽는다.
			// 해당 횟수만큼 반복하며 리스트 내 요소 단위로 데이터를 받아와서 리스트에 추가한다.
			skills.Clear();
			ushort skillLen = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
			count += sizeof(ushort);
			for (int i = 0; i < skillLen; i++)
			{
				SkillInfo skill = new SkillInfo();
				skill.DeSerialize(span, ref count);
				skills.Add(skill);
			}
			//-------------------------------------------------------------------------------
		}
	}

	class ServerSession : Session
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}에 연결되었습니다.");

			PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, playerName = "태사단"};
			packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 101, level = 5, duration = 0.3f });
			packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 102, level = 7, duration = 20f });
			packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 105, level = 1, duration = 3f });
			packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 201, level = 2, duration = 0.05f });

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
