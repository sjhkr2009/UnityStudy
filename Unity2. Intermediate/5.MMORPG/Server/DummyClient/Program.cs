using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace DummyClient
{
	
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

			// 여러 개의 클라이언트 테스트를 위해, 세션 매니저를 이용하여 여러 개의 클라를 만든다.
			connector.Connect(endPoint, SessionManager.Instance.Generate, 10);

			while (true)
			{
				try
				{
					SessionManager.Instance.SendForEach();
				}
				catch (Exception e)
				{
					Console.WriteLine($"Client Chat Exception({e}) : {e.Message}");
				}

				
				Thread.Sleep(250);
			}
		}
	}
}
