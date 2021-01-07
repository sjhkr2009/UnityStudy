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
		
		public sealed override int OnReceive(ArraySegment<byte> buffer)
		{
			int processLen = 0;
			int packetCount = 0;

			while (true)
			{
				if (buffer.Count < HeaderSize)
					break;

				ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
				if (buffer.Count < dataSize)
					break;

				OnReceivePacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));
				packetCount++;

				processLen += dataSize;
				buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
			}

			if (packetCount > 1)
				Console.WriteLine($"[ServerCore::Session] {packetCount} packets Received.");
			
			return processLen;
		}
		public abstract void OnReceivePacket(ArraySegment<byte> buffer);
	}
	

	//----------------------------------------------------------------------------------------------

	public abstract class Session
	{
		Socket _socket;
		int _disconnected = 0;

		// * 수정된 부분
		// 전송하는 내용이 많아졌으니 ReceiveBuffer를 ushort가 표현할 수 있는 최대 크기로 늘려준다.
		// (SendBuffer쪽의 ChunkSize도 크게 늘려준다.)
		ReceiveBuffer _receiveBuffer = new ReceiveBuffer(65535);

		SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
		SocketAsyncEventArgs _receiveArgs = new SocketAsyncEventArgs();

		object _lock = new object();
		
		Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

		public abstract void OnConnected(EndPoint endPoint);
		public abstract int OnReceive(ArraySegment<byte> buffer);
		public abstract void OnSend(int byteCount);
		public abstract void OnDisconnected(EndPoint endPoint);

		void Clear()
		{
			lock(_lock)
			{
				_sendQueue.Clear();
				_pendingList.Clear();
			}
		}

		public void Init(Socket socket, int bufferSize = 1024)
		{
			_socket = socket;

			_sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
			_receiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);

			RegisterReceive();
		}

		// + 추가된 부분
		// 패킷을 리스트로 모아 보내는 형태에 대응하기 위해, Send()의 오버로드 버전을 만든다.
		public void Send(List<ArraySegment<byte>> sendBuffList)
		{
			// 아무 패킷도 없이 RegisterSend를 호출하지 않도록, 빈 리스트면 return 한다.
			if (sendBuffList.Count == 0)
				return;
			
			lock (_lock)
			{
				foreach (ArraySegment<byte> sendBuff in sendBuffList)
					_sendQueue.Enqueue(sendBuff);
				
				if (_pendingList.Count == 0)
					RegisterSend();
			}
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

			Clear();
		}

		#region 네트워크 통신

		void RegisterSend()
		{
			if (_disconnected == 1)
				return;

			while (_sendQueue.Count > 0)
			{
				ArraySegment<byte> buffer = _sendQueue.Dequeue();
				_pendingList.Add(buffer);
			}
			_sendArgs.BufferList = _pendingList;

			try
			{
				bool pending = _socket.SendAsync(_sendArgs);
				if (!pending)
					OnSendCompleted(null, _sendArgs);
			}
			catch(Exception e)
			{
				Console.WriteLine($"Register Send Failed! {e} : {e.Message}");
			}
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
			if (_disconnected == 1)
				return;

			_receiveBuffer.Clear();
			ArraySegment<byte> segment = _receiveBuffer.WriteSegment;
			_receiveArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

			try
			{
				bool pending = _socket.ReceiveAsync(_receiveArgs);
				if (!pending)
					OnReceiveCompleted(null, _receiveArgs);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Register Send Failed! {e} : {e.Message}");
			}
			
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
