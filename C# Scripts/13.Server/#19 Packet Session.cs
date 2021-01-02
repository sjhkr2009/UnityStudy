using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerCore
{
	public abstract class PacketSession : Session
	{
		public static readonly int HeaderSize = 2;
		
		// sealed: 다른 클래스가 이 함수를 override하지 않도록 막는다. (C++의 final 키워드와 유사함)
		// buffer에는 패킷들의 배열이 들어오며, 각 패킷 데이터는 패킷의 크기(ushort)와 ID(ushort)로 시작한다.
		public sealed override int OnReceive(ArraySegment<byte> buffer)
		{
			// 이 프로세스에서 처리한 데이터 크기 (누적)
			int processLen = 0;

			while (true)
			{
				// 최소한 헤더(여기선 ushort를 사용하므로 첫 2바이트가 된다)는 파싱할 수 있는지 확인하고, 불가능할 경우 break
				if (buffer.Count < HeaderSize)
					break;

				// 패킷의 헤더는 패킷의 크기(byte)를 ushort로 저장하고 있다.
				// 첫 2바이트를 ushort로 변환해서, 받아오 버퍼의 크기(= 바이트 단위로 나열된 버퍼 배열의 원소 수)가 패킷의 크기와 일치하는지 확인한다.
				// 일치하지 않으면 데이터의 일부만 온 것이므로 break.
				ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
				if (buffer.Count < dataSize)
					break;

				// 패킷을 모두 받았다면 해당 패킷의 포인터를 OnReceivePacket에 넘기고 조립 및 처리하게 한다.
				OnReceivePacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

				// 받은 데이터 크기를 저장해두고, 처음 받은 ArraySegment에서 처리한 패킷 영역은 빼고 재할당한다.
				processLen += dataSize;
				buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
			}
			
			return processLen;
		}
		// 다른 클래스에서 OnReceive 처리를 할 때는 OnReceivePacket를 통해 추가적인 동작을 실행한다.
		public abstract void OnReceivePacket(ArraySegment<byte> buffer);
	}
	

	//----------------------------------------------------------------------------------------------

	public abstract class Session
	{
		Socket _socket;
		int _disconnected = 0;

		ReceiveBuffer _receiveBuffer = new ReceiveBuffer(1024);

		SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
		SocketAsyncEventArgs _receiveArgs = new SocketAsyncEventArgs();

		object _lock = new object();
		
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
				ArraySegment<byte> buffer = _sendQueue.Dequeue();
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
