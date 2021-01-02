using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	// 이벤트 (event)

	// 운영체제의 커널에 이벤트를 요청한다. 락이 해제되면 대기중인 스레드가 이벤트를 통해 호출된다.
	// 스레드는 기다리는 동안 다른 작업을 할 수 있다는 장점이 있으나, 커널에 접근해야 하므로 다른 락에 비해 속도가 훨씬 느리다.

	// 1. Auto Reset Event : 한 스레드가 접근하면 자동으로 락이 걸린다.
	// 2. Manual Reset Event : 수동으로 락을 걸지 않으면 잠기지 않는다.

	class Lock
	{
		// 현재 임계구역에 접근이 가능한지를 나타낸다. 내부적으로는 bool 값 하나와 동일하지만, 커널 수준에서 관리된다.
		// .Set()과 .Reset()으로 true 또는 false 값을 할당할 수 있다.
		AutoResetEvent _availableAuto = new AutoResetEvent(true);
		ManualResetEvent _availableManual = new ManualResetEvent(true);

		public void Acquire(bool auto)
		{
			if (auto)
			{
				_availableAuto.WaitOne(); // Lock에 접근을 시도한다.
				// 오토 리셋 이벤트는 누군가 점유한 경우 자동으로 false로 변환되니, 별도로 Reset()을 호출할 필요는 없다.
			}
			else
			{
				_availableManual.WaitOne();
				_availableManual.Reset();
				// 메뉴얼 리셋 이벤트는 락을 잠그는 동작을 직접 실행해야 한다. 여기서 접근과 잠금 사이에 다른 스레드가 접근하여 함께 임계 구역을 점유하는 상황이 발생할 수 있다.
				// 따라서 메뉴얼 리셋은 이러한 상호 배제 상황보다는, 여러 스레드가 함께 접근해도 되는 영역의 접근 여부를 통제할 때 사용된다.
			}
		}

		public void Release(bool auto)
		{
			if (auto)
			{
				_availableAuto.Set(); // 다시 true로 바꿔서 접근가능한 상태로 바꿔준다.
			}
			else
			{
				_availableManual.Set();
			}
		}
	}

	class Program
	{
		static int _num = 0;
		static Lock _lock = new Lock();

		// Mutex 클래스는 위에서 만든 AutoResetEvent와 유사하지만, 호출 횟수 등 좀 더 많은 정보를 담고 있다.
		// 프로그램 간 동기화도 가능하다는 장점이 있으나 게임은 한 프로그램 내에서 작동하니 유용하지는 않다. 무겁다는 단점이 더 부각되는 편.
		static Mutex _mutexLock = new Mutex();
		static void Main(string[] args)
		{
			Task t1 = new Task(Thread1);
			Task t2 = new Task(Thread2);
			t1.Start();
			t2.Start();

			Task.WaitAll(t1, t2);

			Console.WriteLine($"Ver1. Auto Reset Event: {_num}");

			//-------------------------------------------

			_num = 0;

			Task t3 = new Task(Thread3);
			Task t4 = new Task(Thread4);
			t3.Start();
			t4.Start();

			Task.WaitAll(t3, t4);

			Console.WriteLine($"Ver2. Manual Reset Event: {_num}");

			//-------------------------------------------

			_num = 0;

			Task t5 = new Task(Thread5);
			Task t6 = new Task(Thread6);
			t5.Start();
			t6.Start();

			Task.WaitAll(t5, t6);

			Console.WriteLine($"Ver3. Mutex: {_num}");
		}
		
		static void Thread1()
		{
			for (int i = 0; i < 10000; i++)
			{
				_lock.Acquire(true);
				_num++;
				_lock.Release(true);
			}
		}
		static void Thread2()
		{
			for (int i = 0; i < 10000; i++)
			{
				_lock.Acquire(true);
				_num--;
				_lock.Release(true);
			}
		}
		static void Thread3()
		{
			for (int i = 0; i < 10000; i++)
			{
				_lock.Acquire(false);
				_num++;
				_lock.Release(false);
			}
		}
		static void Thread4()
		{
			for (int i = 0; i < 10000; i++)
			{
				_lock.Acquire(false);
				_num--;
				_lock.Release(false);
			}
		}
		static void Thread5()
		{
			for (int i = 0; i < 10000; i++)
			{
				_mutexLock.WaitOne();
				_num++;
				_mutexLock.ReleaseMutex();
			}
		}
		static void Thread6()
		{
			for (int i = 0; i < 10000; i++)
			{
				_mutexLock.WaitOne();
				_num--;
				_mutexLock.ReleaseMutex();
			}
		}
	}
}
