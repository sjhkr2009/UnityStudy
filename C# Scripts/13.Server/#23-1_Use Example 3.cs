using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_ChatHandler(PacketSession packetSession, IPacket packet)
	{
		C_Chat chatPacket = packet as C_Chat;
		ClientSession clientSession = packetSession as ClientSession;

		if (clientSession.Room == null)
			return;

		// * 수정된 부분
		// 이제 Broadcast를 바로 실행하지 않고, 해야 할 작업으로 JobQueue에 넘겨준다.
		// clientSession의 Room이 메모리 해제되거나 변경될 수 있으니, clientSession.Room을 별도의 변수에 저장하여 참조 카운트를 유지한다.
		GameRoom room = clientSession.Room;
		room.Push(() =>
		{
			room.Broadcast(clientSession, chatPacket.chat);
		});
	}
}
