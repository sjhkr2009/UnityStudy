using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

// * PacketSession으로 인해 수정된 부분

namespace Server
{
	class Packet
	{
		public ushort packetSize;	// 이 패킷 클래스의 크기
		public ushort packetId;		// 이 패킷의 역할 (로그인 정보, 이동 정보, 클릭 메시지 등)

		// public int _dataA;
		// public int _dataB;
		// ...
	}
	// * 수정된 부분
	// Session이 아닌 PacketSession을 상속받도록 변경한다.
	class GameSession : PacketSession
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}가 접속했습니다.");

			// * 삭제된 부분
			// 클라이언트에서 패킷을 받는 부분을 실습하기 위해 전송하는 부분은 삭제한다.
			// DummyClient에서 패킷을 보내는 방식으로 변경.
			/*
			byte[] buffer1 = Encoding.UTF8.GetBytes("Welcome to Jiho's Server.\n Your ID : ");
			byte[] buffer2 = BitConverter.GetBytes(7777);

			ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
			Array.Copy(buffer1, 0, openSegment.Array, openSegment.Offset, buffer1.Length);
			Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer1.Length, buffer2.Length);

			ArraySegment<byte> sendBuff = SendBufferHelper.Close(buffer1.Length + buffer2.Length);
			
			Send(sendBuff);
			*/
			// 패킷을 모두 받는걸 확인하기 위해 대기시간을 5초로 늘려둔다.
			Thread.Sleep(5000);

			Disconnect();
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}의 접속이 종료되었습니다.");
		}

		// * 수정된 부분
		// OnReceive는 sealed 처리되었으니 구현하지 않고, 아래의 OnReceivePacket으로 대체한다.
		/*
		public override int OnReceive(ArraySegment<byte> buffer)
		{
			string receivedData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
			Console.WriteLine($"[From Client] {receivedData}");

			return buffer.Count;
		}
		*/
		// 추상 함수인 OnReceivePacket는 반드시 구현해야 한다.
		// 여기서는 받은 패킷의 헤더 부분을 ushort로 변환, ID와 크기를 로그로 찍어보기로 한다.
		public override void OnReceivePacket(ArraySegment<byte> buffer)
		{
			ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
			ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + HeaderSize);
			Console.WriteLine($"[Packet Received] ID : {id}, Size : {size}");
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
