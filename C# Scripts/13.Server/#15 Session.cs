using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerCore
{
	// Session: 소켓을 통해 송/수신되는 데이터를 임시로 저장하고 관리하는 일종의 대리자.

	// 서버에 접속한 클라이언트의 소켓을 받아서 메시지를 수신한다.
	// 외부에서 해당 클라이언트에 메시지를 발신하거나 연결을 종료시킬 수 있다.

	// Receive: 메시지를 받을 버퍼를 만들어두고, Listener와 비슷하게 비동기로 메시지를 받아 이벤트를 호출한다.
	// Send:	Receive와 달리 보낼 내용을 매번 지정해야 하므로 매번 버퍼를 재설정해야 하며,
	//			멀티스레드 환경에서 동시 호출로 메시지를 중복해서 보내거나 다른 스레드가 보내서 더 보낼 내용이 없는데 접근하지 않도록 lock을 걸어야 한다.

	abstract class Session
	{
		Socket _socket;
		int _disconnected = 0;

		SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
		SocketAsyncEventArgs _receiveArgs = new SocketAsyncEventArgs();

		object _lock = new object();
		Queue<byte[]> _sendQueue = new Queue<byte[]>();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

		public abstract void OnConnected(EndPoint endPoint);
		public abstract void OnReceive(ArraySegment<byte> buffer);
		public abstract void OnSend(int byteCount);
		public abstract void OnDisconnected(EndPoint endPoint);



		public void Init(Socket socket, int bufferSize = 1024)
		{
			_socket = socket;

			_sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

			_receiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
			_receiveArgs.SetBuffer(new byte[bufferSize], 0, bufferSize);

			RegisterReceive();
		}

		public void Send(string msg) => Send(Encoding.UTF8.GetBytes(msg));
		public void Send(byte[] sendBuff)
		{
			lock (_lock)
			{
				_sendQueue.Enqueue(sendBuff);
				if (_pendingList.Count == 0)	// 내가 처음 Send를 호출하여 대기중인 리스트가 없다면 바로 RegisterSend 호출
					RegisterSend();
			}
		}

		public void Disconnect()
		{
			// 이미 연결이 끊긴 소켓이면 return 한다.
			if (Interlocked.Exchange(ref _disconnected, 1) == 1) 
				return;

			OnDisconnected(_socket.RemoteEndPoint);
			_socket.Shutdown(SocketShutdown.Both);
			_socket.Close();
		}

		#region 네트워크 통신

		void RegisterSend()
		{
			// 전송 도중 다른 스레드가 Send에 접근하지 못 하게 설정
			// Send()에서 lock을 걸었으니 별도의 lock은 필요없음.

			//-------------------------------------------------------------
			// SetBuffer를 써서 하나씩 보내도 되지만, Buffer List를 사용하기로 한다.
			//byte[] buff = _sendQueue.Dequeue();
			//_sendArgs.SetBuffer(buff, 0, buff.Length);
			//-------------------------------------------------------------

			// BufferList는 null이므로 Add로 값을 추가하지 말고, 새 리스트를 만들어서 대입할 것.

			while (_sendQueue.Count > 0)
			{
				// C#은 포인터가 없으므로 배열의 특정 원소에 접근하기 위해 ArraySegment를 사용한다.
				byte[] buff = _sendQueue.Dequeue();
				_pendingList.Add(new ArraySegment<byte>(buff, 0, buff.Length));
			}
			_sendArgs.BufferList = _pendingList;

			bool pending = _socket.SendAsync(_sendArgs);
			if (!pending)
				OnSendCompleted(null, _sendArgs);
		}

		void OnSendCompleted(object sender, SocketAsyncEventArgs args)
		{
			// 콜백 함수이므로 RegisterSend에서 호출된 게 아닐 수 있으니 lock을 걸어야 함
			lock (_lock)
			{
				if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
				{
					OnSend(args.BytesTransferred);

					_sendArgs.BufferList = null;
					_pendingList.Clear();
					
					// 전송 중 다른 스레드가 패킷을 추가하여 _sendQueue에 다른 패킷이 추가되어 있다면 전송한다
					if (_sendQueue.Count > 0)
						RegisterSend();
				}
				else Disconnect();
			}
		}

		void RegisterReceive()
		{
			bool pending = _socket.ReceiveAsync(_receiveArgs);

			if (!pending)
				OnReceiveCompleted(null, _receiveArgs);
		}

		void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
		{
			// 성공적으로 데이터를 가져왔는지 체크한다. 0바이트를 가져오는 경우(연결 끊김 등)도 확인할 것
			if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
			{
				try
				{
					OnReceive(new ArraySegment<byte>(args.Buffer, args.Offset, args.BytesTransferred));
					RegisterReceive();
				}
				catch (Exception e)
				{
					Console.WriteLine($"OnReceiveCompleted failed : {e}");
				}
			}
			else Disconnect();
		}

		#endregion
	}
}
