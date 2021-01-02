using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
	class Listener
	{
		Socket _listenSocket;
		Action<Socket> OnAcceptHandler;

		public void Init(IPEndPoint endPoint, Action<Socket> onAcceptHandler, int backlog = 10)
		{
			_listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			_listenSocket.Bind(endPoint);
			_listenSocket.Listen(backlog);

			// 비동기 방식으로 접속을 구현할 경우 추가 코드
			OnAcceptHandler += onAcceptHandler;

			// 이벤트를 생성하여 최초의 RegisterAccept를 실행시켜준다.
			// 각 이벤트는 독립적으로 동작하므로, 매우 많은 유저들이 동시에 접속해야 한다면 아래 부분을 for문으로 반복하면 된다.
			SocketAsyncEventArgs args = new SocketAsyncEventArgs();
			args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
			RegisterAccept(args);
		}

		// [Deprecated]
		public Socket Accept()
		{
			// 문제점:	Accept()는 클라이언트가 입장 요청(Connect)을 하기 전까지 무한으로 대기한다.
			//			이런 함수를 블로킹 계열 함수라고 하는데, 게임과 같이 상호작용이 활발한 곳에는 논 블로킹 방식(비동기)의 함수를 사용해야 한다.

			// 해결책:	비동기 계열의 AcceptAsync를 사용한다. 요청이 있든 없든 대기하지 않고 return하므로 상태에 따른 처리는 SocketAsyncEventArgs 이벤트를 통해 한다.
			// 해당 방식은 RegisterAccept 참고.
			return _listenSocket.Accept();
		}

		void RegisterAccept(SocketAsyncEventArgs args)
		{
			// 연결된 클라이언트 정보가 이미 있으면 안 되니 null로 초기화해준다.
			args.AcceptSocket = null;
			
			// 작업의 지연 여부를 반환한다.
			// 작업이 지연되면 작업 완료 시 알아서 OnAcceptCompleted이 호출된다.
			bool pending = _listenSocket.AcceptAsync(args);

			// 작업이 바로 완료되면 직접 콜백 함수인 OnAcceptCompleted를 호출해줘야 한다.
			// pending이 계속 걸려서 OnAcceptCompleted - RegisterAccept이 재귀적으로 계속 호출되어 Stack Overflow가 일어나는 상황은, 많은 유저들이 의도적으로 공격하지 않는 한 거의 발생하지 않음.
			if (!pending)
				OnAcceptCompleted(null, args);
		}

		// 콜백 함수. 멀티스레드로 동작함에 주의할 것. 다른 동작이 추가될 경우 멀티스레드에서 발생할 수 있는 동기화 문제를 해결해야 한다. (lock을 거는 등)
		void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
		{
			if(args.SocketError == SocketError.Success)
			{
				// 클라이언트가 에러 없이 접속했을 경우 할 일을 적는다.
				OnAcceptHandler(args.AcceptSocket);
			}

			else Console.WriteLine(args.SocketError.ToString());

			RegisterAccept(args);
		}
	}
}
