using System;
using System.Collections.Generic;
using System.Text;
using ServerCore;

class PacketManager
{
	#region Singleton
	static PacketManager _instance;
	public static PacketManager Instance
	{
		get
		{
			if (_instance == null)
				_instance = new PacketManager();
			return _instance;
		}
	}
	#endregion

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onReceive = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

	// 서버의 Main 함수 시작 부분에서 호출해준다.
	public void Register()
	{
		_onReceive.Add((ushort)PacketID.C_PlayerInfoReq, MakePacket<C_PlayerInfoReq>);
		_handler.Add((ushort)PacketID.C_PlayerInfoReq, PacketHandler.C_PlayerInfoReqHandler);

	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		// 무슨 패킷인지는 파악해야 하니 헤더 부분은 직접 읽어야 한다.
		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += sizeof(ushort);
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += sizeof(ushort);

		Action<PacketSession, ArraySegment<byte>> action = null;
		if (_onReceive.TryGetValue(id, out action))
			action(session, buffer);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{
		T packet = new T();
		packet.DeSerialize(buffer);

		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(packet.Protocol, out action))
			action(session, packet);
	}
}