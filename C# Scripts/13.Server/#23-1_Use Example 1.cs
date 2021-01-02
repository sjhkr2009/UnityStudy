using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace Server
{
	class ClientSession : PacketSession
	{
		public int SessionId { get; set; }

		public GameRoom Room { get; set; }

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}가 접속했습니다.");

			// * 수정된 부분
			// 이제 Enter를 바로 실행하지 않고, 해야 할 작업으로 JobQueue에 넘겨준다.
			Program.Room.Push(() => Program.Room.Enter(this));
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			SessionManager.Instance.Remove(this);
			if(Room != null)
			{
				// * 수정된 부분
				// 이제 Leave를 바로 실행하지 않고, 해야 할 작업으로 JobQueue에 넘겨준다.
				// GameRoom을 별고로 선언, Room을 저장해둬서 클라이언트가 종료되어도 참조 카운트가 유지되게 한다.
				// (그렇지 않으면 클라 종료 후 Room이 null이 되어 JobQueue에서 작업을 처리하려 할 때 크래시가 난다)
				GameRoom room = Room;
				room.Push(() => room.Leave(this));
				Room = null;
			}

			Console.WriteLine($"{endPoint}의 접속이 종료되었습니다.");
		}

		public override void OnReceivePacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnSend(int byteCount)
		{
			//Console.WriteLine($"전송된 바이트: {byteCount}");
		}
	}

}
