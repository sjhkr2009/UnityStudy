using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	class Program
	{
		// Lock 기초
		// 상호 배제(Mutual Exclusive) : 특정 영역을 한 스레드만 접근하도록 설정하는 것
		// 임계 구역(Critiacal Section): 둘 이상의 스레드가 동시에 접근하면 안 되는 공유 영역

		// 이전에 다룬 Interlocked는 속도는 빠르지만 정수에 대한 특정 연산만 수행한다는 단점이 있다.

		static int number = 0;

		// 임계구역 설정을 위해 오브젝트가 하나 필요하다.
		// 두 쓰레드에서 동일한 오브젝트를 사용해야 한다.
		static object _obj = new object();

		static void Main(string[] args)
		{
			Task t1 = new Task(Thread1);
			Task t2 = new Task(Thread2);

			t1.Start();
			t2.Start();

			Task.WaitAll(t1, t2);

			Console.WriteLine($"Monitor 클래스 사용 : {number}");

			//-------------------------------------------------------------------------

			number = 0;

			Task t3 = new Task(Thread3);
			Task t4 = new Task(Thread4);

			t3.Start();
			t4.Start();

			Task.WaitAll(t3, t4);

			Console.WriteLine($"lock 키워드 사용 : {number}");
		}
		
		static void Thread1()
		{
			for (int i = 0; i < 10000; i++)
			{
				// Critiacal Section을 설정하여, 여기부터는 하나의 스레드만 접근할 수 있는 영역을 지정한다.
				// (C++에서는 std::mutex, 윈도우 프로그래밍의 경우 CritiacalSection)
				Monitor.Enter(_obj);

				number++;

				// 해당 오브젝트를 통해 잠근 Lock을 해제한다.
				Monitor.Exit(_obj);

				// 문제점:	Monitor.Exit를 호출하지 않고 중간에 return하는 등 잠금을 해제하지 않으면, 다음으로 대기중인 스레드는 영원히 대기상태가 된다.
				//			이를 '데드락(Deadlock)' 이라고 한다.
			}
		}
		static void Thread2()
		{
			for (int i = 0; i < 10000; i++)
			{
				Monitor.Enter(_obj);

				number--;

				Monitor.Exit(_obj);
			}
		}
		static void Thread3()
		{
			for (int i = 0; i < 10000; i++)
			{
				// Monitor.Enter - Monitor.Exit는 반드시 짝을 맞춰 실행되어야 한다.
				// 하지만 이 과정이 번거롭기 때문에, 실제로는 Monitor.Enter 대신 lock 키워드를 사용한다.

				lock(_obj) // _obj가 잠겨있지 않다면 여기서 잠궈준다. 내부적으로 Monitor.Enter(_obj) 처럼 동작하며, 중괄호가 끝나면 해제된다.
				{
					number++;
				}
			}
		}
		static void Thread4()
		{
			for (int i = 0; i < 10000; i++)
			{
				lock (_obj)
				{
					number--;
				}
			}
		}
	}
}
