using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	class Program
	{
		// 메모리 베리어

		// 사용 목적
		// 1) 코드 재배치 억제를 위해 사용
		// 2) 가시성을 위해 사용

		// 종류
		// 1) Full Memory Barrier:	Store와 Load를 모두 막는다. (Thread.MemoryBarrier로 사용, 어셈블리어로는 ASM WFENCE)
		//		ㄴ Store:	변수에 실제 값을 넣는 것
		//		ㄴ Load:	변수에서 값을 읽어오는 것
		// 2) Store Memory Barrier: Store만 막는다. (어셈블리어로 ASM SFENCE)
		// 3) Load Memory Barrier:	Load만 막는다. (어셈블리어로 ASM LFENCE)

		static int x = 0;
		static int y = 0;
		static int r1 = 0;
		static int r2 = 0;

		static void Main(string[] args)
		{
			int count = 0;
			while (true)
			{
				count++;
				x = y = r1 = r2 = 0;

				Task t1 = new Task(Thread1);
				Task t2 = new Task(Thread2);
				t1.Start();
				t2.Start();

				Task.WaitAll(t1, t2);

				if (r1 == 0 && r2 == 0)
					break;
			}

			Console.WriteLine($"{count}번만에 빠져나옴!");
		}
		static void Thread1()
		{
			// Store y
			y = 1;

			// 1. 코드 재배치 억제: 메모리 베리어를 선언하면 여기까지의 동작은 순서를 바꿀 수 없다.
			Thread.MemoryBarrier();

			// Load x
			r1 = x;
		}
		static void Thread2()
		{
			x = 1;

			// 2. 가시성: 여기서 지금까지 변경한 내용을 다른 스레드들과 공유하는 중앙 메모리에 전달하여, 다른 스레드에서 바뀐 값을 참고할 수 있게 한다.
			Thread.MemoryBarrier();

			r2 = y;
		}
		
		// volatile도 내부적으로 메모리 배리어를 간접적으로 사용한 것.
	}
}
