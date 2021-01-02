using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

			Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			// 소켓 생성까지는 서버와 동일


			// 서버에 입장 문의
			socket.Connect(endPoint);
			Console.WriteLine($"Connected To {socket.RemoteEndPoint}"); // 연결된 곳의 정보를 출력해본다.

			// 서버에 요청 전송
			byte[] sendBuffer = Encoding.UTF8.GetBytes("Hello, World!");
			int sendBytes = socket.Send(sendBuffer);

			// 서버에서 응답 받기
			byte[] receiveBuffer = new byte[1024];
			int size = socket.Receive(receiveBuffer);
			string receivedData =  Encoding.UTF8.GetString(receiveBuffer, 0, size);

			Console.WriteLine($"[From Server] {receivedData}");

			// 서버에서 나간다.
			socket.Shutdown(SocketShutdown.Both);
			socket.Close();
		}
	}
}
