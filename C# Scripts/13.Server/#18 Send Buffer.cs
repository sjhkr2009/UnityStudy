using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerCore
{
	// SendBuffer
	// 세션마다 내부에 고유의 ReceiveBuffer를 가지던 것과 달리, Send는 약간 더 복잡하다.
	// Send는 보낼 때마다 버퍼를 생성하는데, 세션 내부에 SendBuffer를 두면 이 버퍼 내용을 복사해서 저장하는 과정이 필요하다.
	// 과거엔 이렇게 쓰기도 했지만, 다수의 클라이언트에게 패킷을 전송해야 하는데 매번 이런 방식을 사용하면 성능상 불리하다.

	// 따라서 버퍼는 세션 외부에 생성하는 쪽이 효율적이다.

	// SendBuffer는 ReceiveBuffer와 달리 내용을 앞으로 당겨 재사용하지 않는다.
	// 한 곳에만 Send를 하는 것이 아니기 때문.
	public class SendBufferHelper
	{
		// 어디에서나 Send Buffer에 접근가능하도록 전역 변수로 만든다.
		// 단, 멀티스레드 환경이므로 Thread Local로 스레드의 TLS 영역에 저장하게 한다. 초기값은 null을 넣는다.
		// 데이터를 쓸 때 TLS를 통해 하므로 멀티스레드 처리는 별도로 하지 않아도 된다. (작성 후 읽을 때는 다수 스레드가 접근해도 상관없으므로)
		public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(() => null);

		public static int ChunkSize { get; set; } = 1024 * 1024;

		// 전송할 내용을 넣을 Send Buffer를 외부에서 요청할 때 사용한다.
		// 현재 버퍼의 Open()을 호출한다. 버퍼가 초기값(null)이거나 남은 공간이 부족하다면 새로 생성한다.
		public static ArraySegment<byte> Open(int reservedSize)
		{
			if (CurrentBuffer.Value == null)
				CurrentBuffer.Value = new SendBuffer(ChunkSize);

			if (CurrentBuffer.Value.FreeSize < reservedSize)
				CurrentBuffer.Value = new SendBuffer(ChunkSize);

			return CurrentBuffer.Value.Open(reservedSize);
		}
		public static ArraySegment<byte> Close(int usedSize)
		{
			return CurrentBuffer.Value.Close(usedSize);
		}
	}

	public class SendBuffer
	{
		byte[] _buffer;

		// 현재 채워져 있는 데이터 크기. 데이터를 쓸 때 _buffer의 인덱스로 사용한다.
		int _usedSize = 0;
		// 사용 가능한 공간의 크기.
		public int FreeSize => (_buffer.Length - _usedSize);

		public SendBuffer(int chunkSize)
		{
			_buffer = new byte[chunkSize];
		}

		public ArraySegment<byte> Open(int reservedSize)
		{
			if (reservedSize > FreeSize)
				return null;

			return new ArraySegment<byte>(_buffer, _usedSize, reservedSize);
		}

		public ArraySegment<byte> Close(int usedSize)
		{
			ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
			_usedSize += usedSize;
			return segment;
		}
	}
}
