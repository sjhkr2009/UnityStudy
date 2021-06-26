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

			// 시작 시 일단 작업을 1회 실행한다.
			// 함수 내부에 0.25초 후 재실행하는 로직이 포함되어 있으니 1회만 실행해도 반복된다.
			JobTimer.Instance.Push(FlushRoom);

			while (true)
			{
				// * 수정된 부분
				// 기존처럼 0.25초를 쉬거나 작업마다 실행할 때가 되었는지 확인할 필요가 없다.
				//Room.Push(Room.Flush);
				//Thread.Sleep(250);

				// JobTimer의 실행 함수를 매번 호출한다.
				// 가장 먼저 실행할 함수와의 실행시간 비교 1회만 수행되므로 효율적이다.
				JobTimer.Instance.Flush();
			}
		}

		// 반복할 작업. GameRoom의 패킷 보내기 작업을 수행하고, 0.25초 후 이 함수를 실행하도록 예약한다.
		static void FlushRoom()
		{
			Room.Push(() => Room.Flush());
			JobTimer.Instance.Push(FlushRoom, 250);
		}
	}
}
