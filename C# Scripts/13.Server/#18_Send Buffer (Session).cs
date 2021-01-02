using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

// * Send Buffer에서 수정

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

		ReceiveBuffer _receiveBuffer = new ReceiveBuffer(1024);

		SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
		SocketAsyncEventArgs _receiveArgs = new SocketAsyncEventArgs();

		object _lock = new object();
		// * 수정된 부분
		// 추가된 Send Buffer 타입에 맞게 _sendQueue의 원소도 byte[]에서 ArraySegment<byte>로 바꾼다.
		Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

		public abstract void OnConnected(EndPoint endPoint);
		public abstract int OnReceive(ArraySegment<byte> buffer);
		public abstract void OnSend(int byteCount);
		public abstract void OnDisconnected(EndPoint endPoint);



		public void Init(Socket socket, int bufferSize = 1024)
		{
			_socket = socket;

			_sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
			_receiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);

			RegisterReceive();
		}

		// * 삭제된 부분
		// public void Send(string msg) => Send(Encoding.UTF8.GetBytes(msg));
		// * 수정된 부분
		// Send Buffer 타입에 맞게 byte[]가 아닌 ArraySegment<byte>를 받게 바꾼다.
		public void Send(ArraySegment<byte> sendBuff)
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
				// * 수정된 부분
				// Send Buffer 타입에 맞게 byte[]가 아닌 ArraySegment<byte>로 바꾼다.
				ArraySegment<byte> buffer = _sendQueue.Dequeue();
				// 이제 ArraySegment를 만들어줄 필요 없이 꺼낸 버퍼를 그대로 넣는다.
				_pendingList.Add(buffer);
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
					if (!_receiveBuffer.OnWrite(args.BytesTransferred))
					{
						Disconnect();
						return;
					}

					int processLen = OnReceive(_receiveBuffer.ReadSegment);
					if(processLen < 0 || processLen > _receiveBuffer.DataSize)
					{
						Disconnect();
						return;
					}
					if(!_receiveBuffer.OnRead(processLen))
					{
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
