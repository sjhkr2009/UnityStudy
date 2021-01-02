using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace Server
{
	
	class Program
	{
		static Listener _listener = new Listener();

		public static GameRoom Room = new GameRoom();

		static void Main(string[] args)
		{
			Console.WriteLine("----- Server -----");

			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAdr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAdr, 7777);

			_listener.Init(endPoint, SessionManager.Instance.Generate);

			// * 수정된 부분
			// 스레드가 0.25초 쉬는 것이 아니라, Tick을 체크해서 일정 시간이 지났으면 패킷을 보낼수도 있다.
			// 여러 작업이 있다면 체크할 Tick 변수를 추가하면 된다. 코드가 길어지지만 직관적이라는 장점이 있다.
			// 단점은 보내는 주기가 아무리 길어도 매 반복주기마다 Tick을 계산하고 비교하는 작업이 일어난다는 것.

			int roomTick = 0;
			// int ~~Tick = 0;
			// ...
			// ...
			while (true)
			{
				int now = System.Environment.TickCount;
				if(roomTick < now)
				{
					Room.Push(Room.Flush);
					roomTick = now + 250;
				}

				// if(~~Tick < now) ...
				// ...
			}
		}
	}
}
