using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
	// 서버끼리의 통신을 위해서는 클라이언트뿐 아니라 서버에도 Connector가 필요하다.

	public class Connector
	{
		// 컨텐츠단에서 요구한 세션 생성 함수가 들어갈 델리게이트
		Func<Session> _sessionFactory;

		public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory)
		{
			// 단말기 설정
			Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			_sessionFactory = sessionFactory;

			SocketAsyncEventArgs args = new SocketAsyncEventArgs();
			args.Completed += OnConnectCompleted;
			args.RemoteEndPoint = endPoint;

			// 원하는 토큰(object)을 UserToken에 저장해서 보낼 수 있다.
			// 여러 곳에서 연결될 수 있으므로 소켓은 클래스의 로컬 변수에 저장하지 않고 매개 변수로 넘긴다.
			args.UserToken = socket;

			RegisterConnect(args);
		}

		void RegisterConnect(SocketAsyncEventArgs args)
		{
			// (Socket) 형변환은 실패 시 예외를 출력한다. C++의 static_cast/dynamic_cast와 유사하다.
			Socket socket = args.UserToken as Socket;
			if (socket == null)
				return;

			bool pending = socket.ConnectAsync(args);
			if (!pending)
				OnConnectCompleted(null, args);
		}

		void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
		{
			if (args.SocketError == SocketError.Success)
			{
				Session session = _sessionFactory();
				// SocketAsyncEventArgs.ConnectSocket: 비동기 소켓 작업이 성공적으로 완료된 Socket 개체
				session.Init(args.ConnectSocket);
				session.OnConnected(args.RemoteEndPoint);
			}
			else Console.WriteLine($"Fail OnConnectCompleted: {args.SocketError}");
		}
	}
}
