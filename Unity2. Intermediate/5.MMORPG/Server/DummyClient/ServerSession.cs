using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace DummyClient
{
	// * 수정된 부분
	// PacketSession을 상속받도록 변경
	
	class ServerSession : PacketSession
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}에 연결되었습니다.");

		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"{endPoint}의 접속이 종료되었습니다.");
		}

		// PacketSession을 상속받음에 따라 void OnReceivePacket()도 구현해준다.
		// 기존의 OnReceive를 대체한다.
		public override void OnReceivePacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnSend(int byteCount)
		{
			// 채팅 테스트 시 Send가 많이 호출되므로 이 부분은 출력하지 않기로 한다.
			// Console.WriteLine($"전송된 바이트: {byteCount}");
		}
	}

}
