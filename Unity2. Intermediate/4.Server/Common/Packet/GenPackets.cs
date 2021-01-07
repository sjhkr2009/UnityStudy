using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ServerCore;

public enum PacketID 
{
	S_BroadcastEnterGame = 1,
	C_LeaveGame = 2,
	S_BroadcastLeaveGame = 3,
	S_PlayerList = 4,
	C_Move = 5,
	S_BroadcastMove = 6,
	
}

public interface IPacket
{
	ushort Protocol { get; }
	void DeSerialize(ArraySegment<byte> arr);
	ArraySegment<byte> Serialize();
}


public class S_BroadcastEnterGame : IPacket
{
	public ushort Protocol => (ushort)PacketID.S_BroadcastEnterGame;

	public int playerId;
	public float posX;
	public float posY;
	public float posZ;

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastEnterGame), 0, arr.Array, arr.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		
		Array.Copy(BitConverter.GetBytes(playerId), 0, arr.Array, arr.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(posX), 0, arr.Array, arr.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(posY), 0, arr.Array, arr.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(posZ), 0, arr.Array, arr.Offset + count, sizeof(float));
		count += sizeof(float);

		Array.Copy(BitConverter.GetBytes(count), 0, arr.Array, arr.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
	public void DeSerialize(ArraySegment<byte> arr)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		this.playerId = BitConverter.ToInt32(arr.Array, arr.Offset + count);
		count += sizeof(int);
		this.posX = BitConverter.ToSingle(arr.Array, arr.Offset + count);
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(arr.Array, arr.Offset + count);
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(arr.Array, arr.Offset + count);
		count += sizeof(float);
	}
}

public class C_LeaveGame : IPacket
{
	public ushort Protocol => (ushort)PacketID.C_LeaveGame;

	

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_LeaveGame), 0, arr.Array, arr.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		
		

		Array.Copy(BitConverter.GetBytes(count), 0, arr.Array, arr.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
	public void DeSerialize(ArraySegment<byte> arr)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		
	}
}

public class S_BroadcastLeaveGame : IPacket
{
	public ushort Protocol => (ushort)PacketID.S_BroadcastLeaveGame;

	public int playerId;

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastLeaveGame), 0, arr.Array, arr.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		
		Array.Copy(BitConverter.GetBytes(playerId), 0, arr.Array, arr.Offset + count, sizeof(int));
		count += sizeof(int);

		Array.Copy(BitConverter.GetBytes(count), 0, arr.Array, arr.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
	public void DeSerialize(ArraySegment<byte> arr)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		this.playerId = BitConverter.ToInt32(arr.Array, arr.Offset + count);
		count += sizeof(int);
	}
}

public class S_PlayerList : IPacket
{
	public ushort Protocol => (ushort)PacketID.S_PlayerList;

	public struct Player
	{
		public bool isSelf;
		public int playerId;
		public float posX;
		public float posY;
		public float posZ;
	
		public bool Serialize(ArraySegment<byte> arr, ref ushort count)
		{
			bool success = true;
	
			Array.Copy(BitConverter.GetBytes(isSelf), 0, arr.Array, arr.Offset + count, sizeof(bool));
			count += sizeof(bool);
			Array.Copy(BitConverter.GetBytes(playerId), 0, arr.Array, arr.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(posX), 0, arr.Array, arr.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(posY), 0, arr.Array, arr.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(posZ), 0, arr.Array, arr.Offset + count, sizeof(float));
			count += sizeof(float);
	
			return success;
		}
	
		public void DeSerialize(ArraySegment<byte> arr, ref ushort count)
		{
			this.isSelf = BitConverter.ToBoolean(arr.Array, arr.Offset + count);
			count += sizeof(bool);
			this.playerId = BitConverter.ToInt32(arr.Array, arr.Offset + count);
			count += sizeof(int);
			this.posX = BitConverter.ToSingle(arr.Array, arr.Offset + count);
			count += sizeof(float);
			this.posY = BitConverter.ToSingle(arr.Array, arr.Offset + count);
			count += sizeof(float);
			this.posZ = BitConverter.ToSingle(arr.Array, arr.Offset + count);
			count += sizeof(float);
		}
	}
	public List<Player> players = new List<Player>();

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_PlayerList), 0, arr.Array, arr.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		
		Array.Copy(BitConverter.GetBytes((ushort)players.Count), 0, arr.Array, arr.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Player player in players)
			player.Serialize(arr, ref count);

		Array.Copy(BitConverter.GetBytes(count), 0, arr.Array, arr.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
	public void DeSerialize(ArraySegment<byte> arr)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		this.players.Clear();
		ushort playerLen = BitConverter.ToUInt16(arr.Array, arr.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < playerLen; i++)
		{
			Player player = new Player();
			player.DeSerialize(arr, ref count);
			players.Add(player);
		}
	}
}

public class C_Move : IPacket
{
	public ushort Protocol => (ushort)PacketID.C_Move;

	public float posX;
	public float posY;
	public float posZ;

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Move), 0, arr.Array, arr.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		
		Array.Copy(BitConverter.GetBytes(posX), 0, arr.Array, arr.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(posY), 0, arr.Array, arr.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(posZ), 0, arr.Array, arr.Offset + count, sizeof(float));
		count += sizeof(float);

		Array.Copy(BitConverter.GetBytes(count), 0, arr.Array, arr.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
	public void DeSerialize(ArraySegment<byte> arr)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		this.posX = BitConverter.ToSingle(arr.Array, arr.Offset + count);
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(arr.Array, arr.Offset + count);
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(arr.Array, arr.Offset + count);
		count += sizeof(float);
	}
}

public class S_BroadcastMove : IPacket
{
	public ushort Protocol => (ushort)PacketID.S_BroadcastMove;

	public int playerId;
	public float posX;
	public float posY;
	public float posZ;

	public ArraySegment<byte> Serialize()
	{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastMove), 0, arr.Array, arr.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		
		Array.Copy(BitConverter.GetBytes(playerId), 0, arr.Array, arr.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(posX), 0, arr.Array, arr.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(posY), 0, arr.Array, arr.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(posZ), 0, arr.Array, arr.Offset + count, sizeof(float));
		count += sizeof(float);

		Array.Copy(BitConverter.GetBytes(count), 0, arr.Array, arr.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
	public void DeSerialize(ArraySegment<byte> arr)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		this.playerId = BitConverter.ToInt32(arr.Array, arr.Offset + count);
		count += sizeof(int);
		this.posX = BitConverter.ToSingle(arr.Array, arr.Offset + count);
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(arr.Array, arr.Offset + count);
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(arr.Array, arr.Offset + count);
		count += sizeof(float);
	}
}

