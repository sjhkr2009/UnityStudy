using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
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

			_listener.Init(endPoint, OnAcceptHandler);

			while (true)
			{
				// 프로그램 종료 방지를 위한 무한루프
			}
		}

		static void OnAcceptHandler(Socket clientSocket)
		{
			byte[] receiveBuffer = new byte[1024];
			int size = clientSocket.Receive(receiveBuffer);
			string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, size);

			Console.WriteLine($"[From Client] {receivedData}");

			byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome to Jiho's Server.");
			clientSocket.Send(sendBuffer);

			clientSocket.Shutdown(SocketShutdown.Both);
			clientSocket.Close();
		}
		
	}
}
