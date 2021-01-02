using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	// 스핀 락 (Spin Lock)

	// 임계 구역을 누가 점유중일 때, 점유가 해제될 때까지 기다리는 방식.

	class SpinLock
	{
		volatile int _locked = 0;

		public void Acquire1()
		{
			// 아래와 같이 구현하면 두 스레드가 거의 동시에 임계 구역에 접근,
			// 한 스레드가 while문을 통과하고 락을 걸기 전 다른 스레드가 while문을 통과해서 함께 임계 구역에 진입하는 상황이 발생할 수 있다.
			while(_locked > 0) 
			{

			}

			_locked = 1;
		}
		public void Acquire2()
		{
			// Interlocked.Exchange(ref a, b)는 a를 참조형으로 받아 b값을 a에 대입하고, 대입하기 전의 a 값을 반환한다.
			// 기존 값을 저장해두고 새 값을 대입하는 동작이 한 번에 실행되도록 한 것.
			while (true)
			{
				// 락을 걸고, 내가 락을 걸었다면 점유에 성공한 것이므로 반복문을 빠져나온다.
				int origin = Interlocked.Exchange(ref _locked, 1);
				if (origin == 0)
					break;
			}
		}
		public void Acquire3()
		{
			// 2번 버전과 같이 무작정 값을 집어넣는 방식은 경우에 따라 위험할 수 있다.
			// 락이 걸려있는지 확인하고, 없다면 내가 락을 거는 방식이 더 범용적이다.

			// Interlocked.CompareExchange(ref a, b, c)는 a를 참조형으로 받아서, a와 c가 같으면 a에 b를 대입한다. 작동 전의 a값을 반환한다.
			// 이러한 계열의 함수를 CAS(Compare-And-Swap) 라고 하며, 언어마다 함수의 형태나 순서가 다르므로 설명을 읽어보고 사용해야 한다. (ex: C++에서는 성공 여부를 bool로 반환)
			while (true)
			{
				int expected = 0;
				int desired = 1;
				if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected)
					break;
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
		static SpinLock _lock = new SpinLock();
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
				_lock.Acquire1();
				_num++;
				_lock.Release();
			}
		}
		static void Thread2()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire1();
				_num--;
				_lock.Release();
			}
		}
		static void Thread3()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire2();
				_num++;
				_lock.Release();
			}
		}
		static void Thread4()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire2();
				_num--;
				_lock.Release();
			}
		}
		static void Thread5()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire3();
				_num++;
				_lock.Release();
			}
		}
		static void Thread6()
		{
			for (int i = 0; i < 100000; i++)
			{
				_lock.Acquire3();
				_num--;
				_lock.Release();
			}
		}
	}
}
