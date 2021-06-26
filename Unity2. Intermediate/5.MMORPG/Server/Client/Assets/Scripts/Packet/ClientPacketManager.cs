using System;
using System.Collections.Generic;
using System.Text;
using ServerCore;

public class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance => _instance;

	private PacketManager()
	{
		Register();
	}
	#endregion
	
	Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makePktFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

	public void Register()
	{
		_makePktFunc.Add((ushort)PacketID.S_BroadcastEnterGame, MakePacket<S_BroadcastEnterGame>);
		_handler.Add((ushort)PacketID.S_BroadcastEnterGame, PacketHandler.S_BroadcastEnterGameHandler);
		_makePktFunc.Add((ushort)PacketID.S_BroadcastLeaveGame, MakePacket<S_BroadcastLeaveGame>);
		_handler.Add((ushort)PacketID.S_BroadcastLeaveGame, PacketHandler.S_BroadcastLeaveGameHandler);
		_makePktFunc.Add((ushort)PacketID.S_PlayerList, MakePacket<S_PlayerList>);
		_handler.Add((ushort)PacketID.S_PlayerList, PacketHandler.S_PlayerListHandler);
		_makePktFunc.Add((ushort)PacketID.S_BroadcastMove, MakePacket<S_BroadcastMove>);
		_handler.Add((ushort)PacketID.S_BroadcastMove, PacketHandler.S_BroadcastMoveHandler);

	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onReceiveCallback = null)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += sizeof(ushort);
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += sizeof(ushort);

		Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
		if (_makePktFunc.TryGetValue(id, out func))
		{
			IPacket packet = func.Invoke(session, buffer);
			
			// 따로 정의된 콜백 함수가 있으면 실행, 아니라면 패킷을 핸들로 바로 넘겨준다.
			if (onReceiveCallback != null)
				onReceiveCallback.Invoke(session, packet);
			else
				HandlePacket(session, packet);
		}
	}

	T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{
		T packet = new T();
		packet.DeSerialize(buffer);

		// MakePacket에서는 패킷 조립만 하고, 실행하는 부분은 아래의 HandlePacket으로 분리한다.

		return packet;
	}
	public void HandlePacket(PacketSession session, IPacket packet)
	{
		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(packet.Protocol, out action))
			action(session, packet);
	}
}