using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

// * Receive Buffer 에서 수정

namespace ServerCore
{
	// 서버에서 비동기적으로 송/수신되는 데이터를 주고받으며, 메시지를 임시로 저장하는 세션.
	// 소켓 및 통신의 역할을 한다.

	// Receive: 메시지를 받을 버퍼를 만들어두고, Listener와 비슷하게 비동기로 메시지를 받아 이벤트를 호출한다.
	// Send:	Receive와 달리 보낼 내용을 매번 지정해야 하므로 매번 버퍼를 재설정해야 하며,
	//			멀티스레드 환경에서 동시 호출로 메시지를 중복해서 보내거나 다른 스레드가 보내서 더 보낼 내용이 없는데 접근하지 않도록 lock을 걸어야 한다.

	public abstract class Session
	{
		Socket _socket;
		int _disconnected = 0;

		// + 추가된 부분
		// 클라이언트에서 받은 메시지를 작성할 버퍼를 추가한다. 나중엔 훨씬 큰 크기를 쓰겠지만 지금은 1024바이트로 한다.
		ReceiveBuffer _receiveBuffer = new ReceiveBuffer(1024);

		SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
		SocketAsyncEventArgs _receiveArgs = new SocketAsyncEventArgs();

		object _lock = new object();
		Queue<byte[]> _sendQueue = new Queue<byte[]>();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

		public abstract void OnConnected(EndPoint endPoint);
		// * 수정된 부분
		// OnReceive의 처리 결과를 알기 위해, 처리한 데이터 크기를 void가 아닌 int로 반환하게 한다.
		// 서버 및 더미 클라이언트에서는 일단 받은 버퍼를 모두 처리했다고 가정하고, 받은 버퍼 크기를 반환하게 한다.
		public abstract int OnReceive(ArraySegment<byte> buffer);
		public abstract void OnSend(int byteCount);
		public abstract void OnDisconnected(EndPoint endPoint);



		public void Init(Socket socket, int bufferSize = 1024)
		{
			_socket = socket;

			_sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
			_receiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);

			// * 삭제된 부분
			// ReceiveBuffer 클래스를 추가했으니 버퍼는 RegisterReceive에서 처리한다. 이쪽에서는 삭제.
			//_receiveArgs.SetBuffer(new byte[bufferSize], 0, bufferSize);

			RegisterReceive();
		}

		public void Send(string msg) => Send(Encoding.UTF8.GetBytes(msg));
		public void Send(byte[] sendBuff)
		{
			lock (_lock)
			{
				_sendQueue.Enqueue(sendBuff);
				if (_pendingList.Count == 0)
					RegisterSend();
			}
		}

		public void Disconnect()
		{
			if (Interlocked.Exchange(ref _disconnected, 1) == 1) 
				return;

			OnDisconnected(_socket.RemoteEndPoint);
			_socket.Shutdown(SocketShutdown.Both);
			_socket.Close();
		}

		#region 네트워크 통신

		void RegisterSend()
		{
			while (_sendQueue.Count > 0)
			{
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
			lock (_lock)
			{
				if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
				{
					OnSend(args.BytesTransferred);

					_sendArgs.BufferList = null;
					_pendingList.Clear();
					
					if (_sendQueue.Count > 0)
						RegisterSend();
				}
				else Disconnect();
			}
		}

		void RegisterReceive()
		{
			// + 추가된 부분
			// ReceiveBuffer의 커서를 정리해준 후, 쓸 데이터 공간을 받아와서 버퍼를 세팅한다.
			_receiveBuffer.Clear();
			ArraySegment<byte> segment = _receiveBuffer.WriteSegment;
			_receiveArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);
			
			bool pending = _socket.ReceiveAsync(_receiveArgs);

			if (!pending)
				OnReceiveCompleted(null, _receiveArgs);
		}

		void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
		{
			if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
			{
				try
				{
					// + 추가된 부분
					// 1. 데이터 쓰기가 완료되었다면 OnWrite를 호출하여 버퍼의 Write 커서를 읽은 바이트 개수만큼 이동시킨다.
					if (!_receiveBuffer.OnWrite(args.BytesTransferred))
					{
						// OnWrite에 실패했다면 예기치 못 한 동작이 발생한 것이므로 바로 연결을 종료시키고 return한다.
						Disconnect();
						return;
					}
					// 2. 데이터를 쓴 후 OnReceive를 호출하고, 처리한 데이터 크기를 받아온다. (정상적인 경우 일부 또는 전부 처리된다)
					//  ㄴ 기존에 args 정보로 처리하던 OnReceive()는 아래와 같이 Receive Buffer를 통해 처리하도록 변경된다.
					int processLen = OnReceive(_receiveBuffer.ReadSegment);
					if(processLen < 0 || processLen > _receiveBuffer.DataSize)
					{
						// 하나도 처리하지 못 했거나, 처리할 데이터 크기를 초과하여 처리했다면 역시 연결을 종료시킨다.
						Disconnect();
						return;
					}
					// 3. 읽기가 성공적으로 완료되었다면 OnRead를 호출하여 Read 커서도 처리한 바이트 개수만큼 이동시킨다.
					if(!_receiveBuffer.OnRead(processLen))
					{
						// ReceiveBuffer에 남은 데이터 크기를 초과하여 읽었다면 연결을 종료한다.
						// 위에서 예외 처리를 했으니 일반적으론 호출될 일이 없다.
						Disconnect();
						return;
					}

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
