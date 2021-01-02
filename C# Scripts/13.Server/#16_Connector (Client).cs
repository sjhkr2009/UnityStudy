using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace DummyClient
{
	class GameSession : Session
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}에 연결되었습니다.");

			// 서버에 요청 전송은 이쪽에서 처리한다. (연결 및 수신, 해제는 클래스 내에서 처리된다)
			for (int i = 1; i <= 5; i++)
			{
				byte[] sendBuffer = Encoding.UTF8.GetBytes($"Hello, World! {i}");
				Send(sendBuffer);
			}
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}의 접속이 종료되었습니다.");
		}

		public override void OnReceive(ArraySegment<byte> buffer)
		{
			// 매개변수로 넘어온 버퍼와 시작점, 크기를 넣어 변환한다. ArraySegment의 Count는 지정한 범위의 개수가 된다.
			string receivedData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
			Console.WriteLine($"[From Server] {receivedData}");
		}

		public override void OnSend(int byteCount)
		{
			Console.WriteLine($"전송된 바이트: {byteCount}");
		}
	}

	class Program
	{
		// 자세한 설명은 Server쪽 코드 참고
		static void Main(string[] args)
		{
			Console.WriteLine("----- Client -----");

			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAdr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAdr, 7777);

			Connector connector = new Connector();
			connector.Connect(endPoint, () => new GameSession());


			while (true)
			{
				Thread.Sleep(1000);
			}
		}
	}
}
