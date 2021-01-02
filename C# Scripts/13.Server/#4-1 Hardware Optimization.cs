using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	class Program
	{
		// 하드웨어 최적화에 따른 문제
		
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

				// t1, t2가 모두 실행되면 r1, r2 중 적어도 하나는 1이어야 하지만, 둘 다 0이 되는 순간이 존재한다. (전역 변수에 volatile가 있어도 마찬가지)
				// 컴파일러 최적화 외에, 하드웨어(CPU)도 최적화를 위해 실행 순서를 바꿀 수 있기 때문.
				if (r1 == 0 && r2 == 0)
					break;
			}

			Console.WriteLine($"{count}번만에 빠져나옴!");
		}
		static void Thread1()
		{
			// CPU가 보기에 이 두 동작은 서로 연관성이 없으므로, r1 = x와 y = 1의 실행 순서를 뒤바꿀 수 있다.
			// 이 경우 r1 = x, r2 = y가 먼저 실행된다면 둘 다 0이 될 수 있다.
			y = 1;
			r1 = x;
		}
		static void Thread2()
		{
			x = 1;
			r2 = y;
		}
	}
}
