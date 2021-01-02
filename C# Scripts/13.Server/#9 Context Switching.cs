using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	// 일정 시간 대기하는 Lock과 문맥 교환
	
	// 컨텍스트 스위칭 (Context Switching, 문맥 교환)
	// : 프로그램 전환 시 기존 프로그램의 내용을 저장해두고 새로운 프로그램을 위한 정보를 복원하는 작업.

	// CPU의 레지스터들에 기록되는 내용은 제각기 다르다.
	// 연산중인 내용을 임시로 저장하기 위한 레지스터, 코드를 어디까지 실행했는지 추적하지 위한 레지스터, 메모리 주소 접근을 위해 사용되는 레지스터 등...
	
	// 프로그램(작업)에 대한 정보(Context)는 메모리에 모두 저장되어 있다.
	// CPU가 프로그램에 접근하여 일을 처리할 때는 필요한 정보들을 메모리에서 읽어야 하고, 일부는 레지스터에 복원되어야 한다. (어떤 상태였고 어디까지 실행중이었는지 등)
	// 다른 프로그램으로 전환할 때는 레지스터의 정보들을 다시 메모리에 저장해야 한다.

	// 여기서, 한 프로그램에서 다른 프로그램으로 작업을 전환할 때, 메모리에 공간이 없다면 기존 작업은 보조기억장치(SSD/HDD, 또는 가상 메모리)로 이동된다.
	// 또한 새로운 작업에 대한 정보가 메모리에 없다면 보조기억장치에서 메모리와 레지스터로 읽어와야 한다.

	// 따라서 쓰레드가 점유중인 임계 구역에 접근 시, 다른 작업으로 전환시키는 것이 반드시 효율적이지는 않다.
	// 경우에 따라서는(특히 간단한 작업인 경우) 스핀 락처럼 계속 대기하는 것이 낫다.

	class Lock
	{
		volatile int _locked = 0;
		
		// 이전의 스핀 락에서, 쓰레드가 락 해제까지 기다리지 않고 일정 시간 대기하도록 변경한다.
		public void Acquire(int waitWay)
		{
			while (true)
			{
				// 임계 구역 접근을 시도하는 부분은 스핀 락과 동일.
				int expected = 0;
				int desired = 1;
				if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected)
					break;

				// 다만 시도에 실패 시, 스핀 락과 달리 바로 재시도하는 대신 일정 시간 후에 재시도하도록 처리하면 된다.
				// 일정 시간 쉬는 방법은 크게 3가지가 있다.

				// 1. 무조건 휴식:	해당 시간만큼 쉰다. Thread.Sleep에 양의 정수(밀리초)를 입력하여 사용한다.
				if (waitWay > 0)
					Thread.Sleep(1);

				// 2. 조건부 양보:	나보다 우선순위가 같거나 높은 쓰레드가 있을 경우에만 양보한다. Thread.Sleep에 0을 입력하여 사용한다.
				else if (waitWay < 0)
					Thread.Sleep(0);

				// 3. 관대한 양보:	지금 실행가능한 쓰레드가 있으면 누구든 실행하도록 양보한다. Thread.Yield로 사용한다.
				else if (waitWay == 0)
					Thread.Yield();
			}
		}

		public void Release()
		{
			_locked = 0;
		}
	}

	class Program
	{
		static int _num = 0;
		static Lock _lock = new Lock();
		static void Main(string[] args)
		{
			Task t1 = new Task(Thread1);
			Task t2 = new Task(Thread2);
			t1.Start();
			t2.Start();

			Task.WaitAll(t1, t2);

			Console.WriteLine($"Ver1. {_num}");

			//-------------------------------------------

			_num = 0;

			Task t3 = new Task(Thread3);
			Task t4 = new Task(Thread4);
			t3.Start();
			t4.Start();

			Task.WaitAll(t3, t4);

			Console.WriteLine($"Ver2. {_num}");

			//-------------------------------------------

			_num = 0;

			Task t5 = new Task(Thread5);
			Task t6 = new Task(Thread6);
			t5.Start();
			t6.Start();

			Task.WaitAll(t5, t6);

			Console.WriteLine($"Ver3. {_num}");
		}
		
		static void Thread1()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire(1);
				_num++;
				_lock.Release();
			}
		}
		static void Thread2()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire(1);
				_num--;
				_lock.Release();
			}
		}
		static void Thread3()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire(-1);
				_num++;
				_lock.Release();
			}
		}
		static void Thread4()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire(-1);
				_num--;
				_lock.Release();
			}
		}
		static void Thread5()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire(0);
				_num++;
				_lock.Release();
			}
		}
		static void Thread6()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire(0);
				_num--;
				_lock.Release();
			}
		}
	}
}
