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
		public float PosX { get; set; }
		public float PosY { get; set; }
		public float PosZ { get; set; }

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}가 접속했습니다.");

			Program.Room.Push(() => Program.Room.Enter(this));
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			SessionManager.Instance.Remove(this);
			if(Room != null)
			{
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
