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
		
		static void Main(string[] args)
		{
			Console.WriteLine("----- Server -----");

			// DNS (Domain Name System) : 특정 도메인 주소를 숫자로 된 아이피 주소로 변경해주는 시스템
			string host = Dns.GetHostName();					// 내 로컬 컴퓨터의 호스트 이름
			IPHostEntry ipHost = Dns.GetHostEntry(host);        // 호스트 이름을 통해 호스트 주소 정보를 가져온다.
			IPAddress ipAdr = ipHost.AddressList[0];			// .AddressList로 ip 주소를 가져온다. 반환형이 배열인 이유는 경우에 따라 여러 개의 아이피를 가질 수 있기 때문.

			IPEndPoint endPoint = new IPEndPoint(ipAdr, 7777);  // 최종적으로 서버 주소를 생성한다. 생성자에는 ip 주소와 포트 번호를 입력한다.
																// 클라이언트가 포트 번호를 정확히 입력해야 접속할 수 있다.

			// 소켓을 생성한다. TCP로 만들 경우 2,3번째 인자는 SocketType.Stream, ProtocolType.Tcp를 입력한다.
			// 서버를 건물에 비유하면 문지기나 안내원과 같은 역할.
			Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			// 문지기 교육
			listenSocket.Bind(endPoint);

			// 소켓이 수용할 수 있는 최대 대기인원. 이를 초과하면 나머지 인원은 접속에 실패한다.
			listenSocket.Listen(10);

			while (true)
			{
				Console.WriteLine("Listening...");

				// 클라이언트를 입장시키고 손님의 소켓을 저장한다. 서버와의 소통은 이 소켓을 통해서 이루어진다.
				// Accept 함수는 손님이 입장할 때까지 대기하므로 별도로 반복문을 돌릴 필요는 없다.
				Socket clientSocket = listenSocket.Accept();

				// 클라이언트의 메시지을 받는다. 받을 때는 데이터를 읽어올 공간이 필요하며, 받아온 데이터를 해석하려면 알맞은 타입으로 변환이 필요하다.
				byte[] receiveBuffer = new byte[1024];
				int size = clientSocket.Receive(receiveBuffer);	// Receive는 몇 바이트를 받아왔는지 int로 반환한다.
				string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, size);// UTF8로 해석한다. 매개변수로 해석할 바이트 배열, 시작 인덱스, 읽어올 바이트 개수가 들어간다.

				Console.WriteLine($"[From Client] {receivedData}");

				// 클라이언트에 메시지를 보낸다. 마찬가지로 인코딩이 필요하다.
				byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome to Jiho's Server.");
				clientSocket.Send(sendBuffer);

				// 클라이언트를 내보낸다. Shutdown이 없어도 똑같이 동작하지만, 미리 종료할 것임을 예상하게 할 수 있다.
				clientSocket.Shutdown(SocketShutdown.Both);
				clientSocket.Close();
			}
		}
		
	}
}
