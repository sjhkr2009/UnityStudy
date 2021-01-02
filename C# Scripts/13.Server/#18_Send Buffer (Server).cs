using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace Server
{
	class GameSession : Session
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}가 접속했습니다.");
			// * 수정된 부분
			// (기존) Send("Welcome to Jiho's Server.");
			//----------------------------------------------------------------------
			// 이제 Send Buffer를 받아서 그쪽에 보낼 내용을 입력한다.
			byte[] buffer1 = Encoding.UTF8.GetBytes("Welcome to Jiho's Server.\n Your ID : ");
			byte[] buffer2 = BitConverter.GetBytes(7777);

			// 보낼 내용이 작성될 버퍼. 넉넉한 크기로 요청한다.
			ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
			
			// 버퍼에 내용을 작성한다.
			Array.Copy(buffer1, 0, openSegment.Array, openSegment.Offset, buffer1.Length);
			Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer1.Length, buffer2.Length);

			// 작성한 내용의 크기를 입력하고 버퍼를 닫은 후 작성한 내용이 있는 부분의 ArraySegment를 반환받는다.
			ArraySegment<byte> sendBuff = SendBufferHelper.Close(buffer1.Length + buffer2.Length);
			
			// 해당 내용을 전송한다.
			Send(sendBuff);
			//----------------------------------------------------------------------
			Thread.Sleep(1000);

			Disconnect();
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}의 접속이 종료되었습니다.");
		}

		public override int OnReceive(ArraySegment<byte> buffer)
		{
			string receivedData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
			Console.WriteLine($"[From Client] {receivedData}");

			return buffer.Count;
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
