using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
	class GameSession : Session
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}가 접속했습니다.");

			Send("Welcome to Jiho's Server.");

			Thread.Sleep(1000);

			Disconnect();
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}의 접속이 종료되었습니다.");
		}

		public override void OnReceive(ArraySegment<byte> buffer)
		{
			// 매개변수로 넘어온 버퍼와 시작점, 크기를 넣어 변환한다. ArraySegment의 Count는 지정한 범위의 개수가 된다.
			string receivedData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
			Console.WriteLine($"[From Client] {receivedData}");
		}

		public override void OnSend(int byteCount)
		{
			Console.WriteLine($"전송된 바이트: {byteCount}");
		}
	}

	class Program
	{
		static Listener _listener = new Listener();
		static void Main(string[] args)
		{
			Console.WriteLine("----- Server -----");

			
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAdr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAdr, 7777);

			_listener.Init(endPoint, () => { return new GameSession(); });

			while (true)
			{
				// 프로그램 종료 방지를 위한 무한루프
			}
		}
	}
}
