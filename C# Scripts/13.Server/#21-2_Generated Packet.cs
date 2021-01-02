using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ServerCore;

public enum PacketID 
{
	C_PlayerInfoReq = 1,
	S_Test = 2,
	
}

interface IPacket
{
	ushort Protocol { get; }
	void DeSerialize(ArraySegment<byte> arr);
	ArraySegment<byte> Serialize();
}


class C_PlayerInfoReq : IPacket
{
	public ushort Protocol => (ushort)PacketID.C_PlayerInfoReq;

	public byte testByte;
	public long playerId;
	public string playerName;
	public struct Skill
	{
		public int id;
		public short level;
		public float duration;
	
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
			this.id = BitConverter.ToInt32(span.Slice(count, span.Length - count));
			count += sizeof(int);
			this.level = BitConverter.ToInt16(span.Slice(count, span.Length - count));
			count += sizeof(short);
			this.duration = BitConverter.ToSingle(span.Slice(count, span.Length - count));
			count += sizeof(float);
		}
	}
	public List<Skill> skills = new List<Skill>();

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		Span<byte> span = new Span<byte>(arr.Array, arr.Offset, arr.Count);

		ushort count = 0;
		bool success = true;

		count += sizeof(ushort);
		success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort)PacketID.C_PlayerInfoReq);
		count += sizeof(ushort);
		
		arr.Array[arr.Offset + count] = (byte)this.testByte;
		count += sizeof(byte);
		success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), playerId);
		count += sizeof(long);
		ushort playerNameLen = (ushort)Encoding.Unicode.GetBytes(playerName, 0, playerName.Length, arr.Array, arr.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), playerNameLen);
		count += sizeof(ushort);
		count += playerNameLen;
		success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort)skills.Count);
		count += sizeof(ushort);
		foreach (Skill skill in skills)
			success &= skill.Serialize(span, ref count);

		success &= BitConverter.TryWriteBytes(span, count);

		if (!success)
			return null;

		return SendBufferHelper.Close(count);
	}
	public void DeSerialize(ArraySegment<byte> arr)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(arr.Array, arr.Offset, arr.Count);

		this.testByte = (byte)arr.Array[arr.Offset + count];
		count += sizeof(byte);
		this.playerId = BitConverter.ToInt64(span.Slice(count, span.Length - count));
		count += sizeof(long);
		ushort playerNameLen = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
		count += sizeof(ushort);
		this.playerName = Encoding.Unicode.GetString(span.Slice(count, playerNameLen));
		count += playerNameLen;
		this.skills.Clear();
		ushort skillLen = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
		count += sizeof(ushort);
		for (int i = 0; i < skillLen; i++)
		{
			Skill skill = new Skill();
			skill.DeSerialize(span, ref count);
			skills.Add(skill);
		}
	}
}

class S_Test : IPacket
{
	public ushort Protocol => (ushort)PacketID.S_Test;

	public int testInt;

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		Span<byte> span = new Span<byte>(arr.Array, arr.Offset, arr.Count);

		ushort count = 0;
		bool success = true;

		count += sizeof(ushort);
		success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort)PacketID.S_Test);
		count += sizeof(ushort);
		
		success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), testInt);
		count += sizeof(int);

		success &= BitConverter.TryWriteBytes(span, count);

		if (!success)
			return null;

		return SendBufferHelper.Close(count);
	}
	public void DeSerialize(ArraySegment<byte> arr)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(arr.Array, arr.Offset, arr.Count);

		this.testInt = BitConverter.ToInt32(span.Slice(count, span.Length - count));
		count += sizeof(int);
	}
}

