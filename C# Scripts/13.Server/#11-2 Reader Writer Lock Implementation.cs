using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	// Reader Writer Lock 구현 연습

	// 락을 만들 때 규칙을 몇 가지 정해야 한다.

	// 1. 재귀적 락 허용 여부
	//		- Writer에서 락을 획득한 후, 같은 스레드가 다시 재귀적으로 락에 접근하는 것을 허용할 것인가?
	//		- 허용 시, WriteLock을 점유한 상태에서 같은 스레드가 다시 Read Lock 또는 Write Lock을 획득할 수 있다. 
	//			(단, Read는 상호 배제가 아니므로 Read Lock 획득 상태에서 Write를 바로 획득할수는 없다)

	// 2. 스핀락 규칙
	//		- 몇 번의 시도 후 대기상태로 들어갈 것인가?
	//		- 여기서는 5000번 시도 후 락 획득에 실패하면 Yield 상태로 대기하도록 한다.

	class Lock
	{
		const int EMPTY_FLAG = 0x00000000;
		const int WRITE_MASK = 0x7FFF0000;
		const int READ_MASK = 0x0000FFFF;
		const int MAX_SPIN_COUNT = 5000;

		// 4바이트(32비트)의 플래그를 만든다.
		// 플래그의 첫 비트는 사용하지 않고, 15비트는 Write Thread ID, 16비트는 Read Count를 위해 사용할 것이다.
		// Write Thread ID:	한 스레드만 Write가 가능하므로 해당 스레드의 ID를 기록.
		// Read Count:		여러 스레드가 동시에 Read가 가능하므로, 몇 개의 스레드가 동작 중인지 확인하기 위한 카운트.
		int _flag;

		// 재귀적 락 허용 시, 재귀적으로 획득된 Write Lock의 횟수를 기록하기 위해 사용하는 변수
		int _writeCount = 0;

		public void WriteLock()
		{
			// 재귀적 락 허용 시 추가할 부분
			//-----------------------------------------------------

			// 락을 획득한 스레드가 이 스레드일 경우, 카운트만 하나 늘리고 통과시킨다.
			int lockThreadId = (_flag & WRITE_MASK) >> 16;
			if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
			{
				_writeCount++;
				return;
			}

			//-----------------------------------------------------

			// 아무도 Read/Write Lock을 획득하고 있지 않을 경우 소유권을 획득한다.

			// 스핀락 제한 횟수만큼 시도하고, 실패하면 Yield로 대기한 후 반복한다.
			// 성공하면 Write Thread ID를 자신의 아이디로 입력하고 return 한다.

			// 스레드 ID를 가져와서 16비트만큼 왼쪽으로 민다. 혹시 모르니 WRITE_MASK와 곱연산하여 다른 비트는 0으로 만든다.
			int threadId = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;

			while (true)
			{
				for (int i = 0; i < MAX_SPIN_COUNT; i++)
				{
					// 락을 걸거나 해제하는 과정이 여러 동작으로 분리되지 않게 주의. Interlocked 계열의 함수를 이용할 것.
					if (Interlocked.CompareExchange(ref _flag, threadId, EMPTY_FLAG) == EMPTY_FLAG)
					{
						_writeCount++; // 재귀적 락 허용 시, _writeCount를 하나 늘리고 리턴한다.
						return;
					}
				}

				Thread.Yield();
			}
		}

		public void WriteUnlock()
		{
			// 재귀적 락 허용 시 추가할 부분
			//-----------------------------------------------------

			// Write Count를 하나 감소시키고, 카운트가 0일 경우에만 최종적으로 락을 풀어준다.
			if (--_writeCount > 0)
				return;

			//-----------------------------------------------------

			// WriteLock을 한 스레드만 해제를 포함한 다음 동작을 실행할 수 있을테니, Unlock에서 별도로 조건을 확인할 필요는 없다.
			Interlocked.Exchange(ref _flag, EMPTY_FLAG);
		}

		public void ReadLock()
		{
			// 재귀적 락 허용 시 추가할 부분
			//-----------------------------------------------------

			// 락을 획득한 스레드가 이 스레드일 경우, Writer ID는 체크할 필요 없으니 Read Count만 하나 늘리고 통과시킨다.
			int lockThreadId = (_flag & WRITE_MASK) >> 16;
			if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
			{
				Interlocked.Increment(ref _flag);
				return;
			}

			//-----------------------------------------------------

			// 아무도 Write Lock을 획득하고 있지 않으면 Read Count를 1 늘린다.

			while (true)
			{
				for (int i = 0; i < MAX_SPIN_COUNT; i++)
				{
					// WRITE_MASK에 해당되는 비트가 비어 있다면((_flag & WRITE_MASK) == 0인 경우에),
					// Write Lock을 획득한 스레드가 없다는 의미이므로 Read Count를 1 늘린다.

					int expected = (_flag & READ_MASK); // 플래그의 Read Count만 추출한 후

					// 1. flag의 Read Count만 추출한 게 전체 flag와 일치하면 (= Write Lock을 획득한 스레드가 없으면)
					// 2. 앞에서 추출한 Read Count가 현재 flag 상태와 일치하면 (= 혹시 그 사이에 동시에 진입한 다른 Reader가 카운트를 증가시키지 않았다면)
					if (expected == Interlocked.CompareExchange(ref _flag, expected + 1, expected)) // Read Count를 1 증가시키고 return
						return;
				}

				Thread.Yield();
			}
		}

		public void ReadUnlock()
		{
			Interlocked.Decrement(ref _flag);
		}
	}

	class Program
	{
		static volatile int count = 0;
		static Lock _lock = new Lock();

		static void Main(string[] args)
		{
			Task t1 = new Task(delegate ()
			{
				for (int i = 0; i < 100000; i++)
				{
					_lock.WriteLock();
					_lock.WriteLock(); // 재귀적 락 테스트
					count++;
					_lock.WriteUnlock();
					_lock.WriteUnlock();
				}
			});
			Task t2 = new Task(delegate ()
			{
				for (int i = 0; i < 100000; i++)
				{
					_lock.WriteLock();
					count--;
					_lock.WriteUnlock();
				}
			});

			t1.Start();
			t2.Start();

			Task.WaitAll(t1, t2);

			Console.WriteLine($"Write Lock: {count}");

			//-------------------------------------------

			// ReadLock은 상호 배제가 아니므로 별다른 효과는 없다.
			count = 0;

			Task t3 = new Task(() =>
			{
				for (int i = 0; i < 100000; i++)
				{
					_lock.ReadLock();
					count++;
					_lock.ReadUnlock();
				}
			});
			Task t4 = new Task(() =>
			{
				for (int i = 0; i < 100000; i++)
				{
					_lock.ReadLock();
					count--;
					_lock.ReadUnlock();
				}
			});

			t3.Start();
			t4.Start();

			Task.WaitAll(t3, t4);

			Console.WriteLine($"Read Lock: {count}");
		}
		
	}
}
