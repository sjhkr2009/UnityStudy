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

			while (true)
			{
				// + 추가된 부분
				// 0.25초에 한 번씩 쌓인 패킷들을 전송하게 한다.
				// 이 동작 또한 JobQueue를 통해 실행한다. 
				Room.Push(Room.Flush);
				Thread.Sleep(250);
			}
		}
	}
}
