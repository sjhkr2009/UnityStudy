using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

// * PacketSession으로 인해 수정된 부분
// 클라이언트에서 서버로 패킷을 전송해본다.

namespace DummyClient
{
	// 패킷 전송 테스트를 위한 클래스
	class Packet
	{
		public ushort packetSize;
		public ushort packetId;
	}

	class GameSession : Session
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}에 연결되었습니다.");

			// * 수정된 부분
			// 패킷을 전송한다. Send Buffer때 패킷을 전송하던 방식을 이용한다.
			Packet packet = new Packet() { packetSize = 4, packetId = 7 };

			for (int i = 0; i < 5; i++)
			{
				byte[] buffer1 = BitConverter.GetBytes(packet.packetSize);
				byte[] buffer2 = BitConverter.GetBytes(packet.packetId);

				ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
				Array.Copy(buffer1, 0, openSegment.Array, openSegment.Offset, buffer1.Length);
				Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer1.Length, buffer2.Length);

				ArraySegment<byte> sendBuff = SendBufferHelper.Close(packet.packetSize);

				Send(sendBuff);
			}
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}의 접속이 종료되었습니다.");
		}

		public override int OnReceive(ArraySegment<byte> buffer)
		{
			string receivedData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
			Console.WriteLine($"[From Server] {receivedData}");

			return buffer.Count;
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
