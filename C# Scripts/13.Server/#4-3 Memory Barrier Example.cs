using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	class Program
	{
		// 메모리 베리어 사용 예시
		// 1) 코드 재배치 억제를 위해 사용
		// 2) 가시성을 위해 사용

		static int answer = 0;
		static bool complete = false;

		static void Main(string[] args)
		{
			while (true)
			{
				answer = 0;
				complete = false;

				Task t1 = new Task(Thread1);
				Task t2 = new Task(Thread2);
				t1.Start();
				t2.Start();

				Task.WaitAll(t1, t2);

				if (answer != 123)
					break;
			}
		}
		static void Thread1()
		{
			answer = 123;
			Thread.MemoryBarrier();	// answer에 값을 입력(store)했으니 해당 전역 변수를 갱신하고, complete보다 항상 먼저 실행되도록 메모리 배리어 사용 (가시성 + 코드 재배치 억제)
			complete = true;
			Thread.MemoryBarrier(); // 입력된 값 갱신 (가시성)
		}
		static void Thread2()
		{
			Thread.MemoryBarrier(); // 전역 변수 확인 (가시성)
			if (complete)
			{
				Thread.MemoryBarrier(); // 최신 버전의 answer를 가져오도록 입력된 값 갱신 (가시성)
				Console.WriteLine(answer);
			}
		}
	}
}
