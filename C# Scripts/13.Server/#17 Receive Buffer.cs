using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
	// ReceiveBuffer
	// 받아온 데이터를 작성할 버퍼 공간을 관리한다.
	// 세션 클래스에 통합하는 경우도 있으나, 별도로 관리하는 편이 편리하다.
	
	// * 읽고 쓸 데이터의 위치 관리 및 커서 초기화
	// * 버퍼의 남은 공간과 잔여 데이터의 관리 및 예외 처리

	class ReceiveBuffer
	{
		ArraySegment<byte> _buffer;

		// 버퍼에서 현재 읽거나 쓰고 있는 위치의 index
		int _readPos;
		int _writePos;

		public ReceiveBuffer(int bufferSize)
		{
			_buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
		}

		// 아직 처리되지 않은 데이터의 크기 (버퍼에 썼지만 읽지 않은 바이트 수)
		public int DataSize => (_writePos - _readPos);
		// 버퍼에 남은 공간 (아직 쓰지 않은 바이트 수)
		public int FreeSize => _buffer.Count - _writePos;
		// 읽을 데이터 배열. 버퍼에서 현재 읽고 있는 지점부터 아직 읽지 않은 데이터만큼의 배열을 반환한다.
		public ArraySegment<byte> ReadSegment 
			=> new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize);
		// 쓸 데이터 공간. 버퍼에서 현재 쓰고 있는 지점부터 남은 공간만큼의 배열을 반환한다.
		public ArraySegment<byte> WriteSegment 
			=> new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize);


		// 데이터의 읽고 쓰기 위치를 초기화한다.
		// 작업하다보면 읽기/쓰기 위치가 계속 뒤로 밀리므로 주기적으로 실행한다.
		public void Clear()
		{
			int dataSize = DataSize;
			if (dataSize == 0)
			{
				// 남은 데이터가 없으면 커서를 모두 첫 인덱스로 옮긴다.
				_readPos = _writePos = 0;
			}
			else
			{
				// 남은 데이터가 있다면 버퍼 맨 앞으로 복사해온다.
				// Array.Copy(복사할 배열, 복사 시작 지점, 붙여넣기할 배열, 붙여넣기 시작 지점, 옮길 요소의 개수)
				//	ㄴ 버퍼의 읽기 커서(_readPos)에서 남은 데이터의 바이트 수(datasize)만큼을 배열의 첫 부분(Offset)으로 옮긴다.
				Array.Copy(_buffer.Array, (_buffer.Offset + _readPos), _buffer.Array, _buffer.Offset, dataSize);

				// 읽기 위치는 처음으로, 쓰기 위치는 옮겨온 데이터의 끝 부분으로 옮긴다.
				_readPos = 0;
				_writePos = dataSize;   // DataSize 프로퍼티 호출 시 변경된 _readPos에 의해 예기치 않은 값이 반환되니 주의.
			}
		}

		/// <summary>
		/// 성공적으로 데이터 읽기 처리를 완료했을 경우 호출된다.
		/// </summary>
		/// <param name="readByteCount">읽은 데이터의 크기(byte)</param>
		/// <returns></returns>
		public bool OnRead(int readByteCount)
		{
			// 남은 데이터 크기보다 더 많이 읽은 경우는 비정상적인 동작이므로 false를 반환한다.
			if (readByteCount > DataSize)
				return false;

			// 그 외의 경우는 읽기 위치를 읽은 바이트 수만큼 옮긴다.
			_readPos += readByteCount;
			return true;
		}

		/// <summary>
		/// 클라이언트에서 받은 데이터를 썼을 때 호출된다.
		/// </summary>
		/// <param name="wroteByteCount">작성한 데이터의 크기(byte)</param>
		/// <returns></returns>
		public bool OnWrite(int wroteByteCount)
		{
			// 남은 버퍼 공간보다 많이 썼다면 위와 마찬가지로 false를 반환
			if (wroteByteCount > FreeSize)
				return false;

			_writePos += wroteByteCount;
			return true;
		}
	}
}
