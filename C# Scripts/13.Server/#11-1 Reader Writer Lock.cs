using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	// 기존에 다룬 락(lock) 정리
	// 1. object를 이용한 락: lock(obj){}, Monitor 클래스 사용
	// 2. 데드락 해결을 위한 클래스: SpinLock(+ 대기시간), event, Mutex 등

	// 락은 상황에 따라 성능이 오가므로 테스트하면서 사용하며, 직접 만들기도 하고 제공되는 라이브러리를 이용하기도 한다.

	
	class Program
	{
		static int _num = 0;
		static object _obj = new object();

		// ※ SpinLock 클래스
		//	.Enter(), .Exit()으로 잠금 및 해제한다.
		//	내부적으로 SpinLock과 동일하게 임계 구역에 접근을 계속 시도한다. 단 일정 횟수 시도가 실패하면 어느정도 대기 후 다시 사용한다.
		static SpinLock _spinLock = new SpinLock();

		// Reader Writer Lock
		// : 데이터를 쓰기 위해 접근하는 경우와 읽기 위해 접근하는 경우를 분리하여, 읽는 스레드끼리는 상호 배제하지 않는 방식.

		// RPG의 퀘스트처럼 특정 조건을 완료했을 때 보상을 받는다고 가정한다.
		// 3종류의 아이템을 고정으로 보상을 받는데, 운영 팀에서 추가적으로 2개의 보상을 추가로 주는 한시적 이벤트를 시행한다고 하자.
		class Reward { }

		// 완전히 고정 보상은 아니고 운영 툴로 보상을 추가할 수 있으니, 서버가 관리하며 락이 들어가야 한다.
		// lock은 읽기에는 필요없지만, 데이터 쓰기/수정에서는 여러 스레드에서 데이터가 중복 작성되는 것을 피하기 위해 필요하기 때문.

		// 그런데 대부분의 경우에는 데이터 시트에 있는 고정 보상만 주어지고, 운영 툴로 보상을 추가하는건 가끔씩만 일어나는데 매번 Lock을 거는 것은 비효율적이다.
		// 따라서 평소에 Get할때는 동시에 접근할 수 있다가, 변경이 필요할 때만 상호 배타적으로 동작하게 한다면 효율적일 것이다.

		// 이를 Reader Writer Lock 이라고 한다.

		// ReaderWriterLock 보다 ReaderWriterLockSlim이 더 최신 버전이니 이쪽을 사용하면 됨
		static ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
		static Reward _reward = new Reward();
		static Reward GetRewardById(int id)
		{
			// 평상시에는 락이 있는지 확인하고, 없으면 진입한다. 락이 걸려있지 않는 한 여러 스레드가 진입할 수 있다.
			_rwLock.EnterReadLock();

			_rwLock.ExitReadLock();

			return _reward;
		}

		static void AddReward(Reward reward)
		{
			// 추가 보상 작성이 필요할 때는 Writer로써 락을 걸고 진입한다.
			// 이미 1개 이상의 스레드가 점유중일 경우 대기하는데, 대기하는 동안 다른 Reader가 계속 진입해서 점유가 끊이지 않으면 Writer는 오랫동안 실행되지 못할 수도 있다.
			// 이러한 'Write-Starvation' 문제 때문에, 경우에 따라 Writer가 대기중인 경우 더 이상 Reader가 진입하지 못 하게 막기도 한다.
			_rwLock.EnterWriteLock();

			_rwLock.ExitWriteLock();
		}

		static void Main(string[] args)
		{
			Task t1 = new Task(Thread1);
			Task t2 = new Task(Thread2);
			t1.Start();
			t2.Start();

			Task.WaitAll(t1, t2);

			Console.WriteLine($"Ver1. SpinLock 클래스: {_num}");

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
				// 락 성공 여부를 반환하는 bool 인자가 하나 필요하다.
				bool lockToken = false;

				try
				{
					_spinLock.Enter(ref lockToken);
					_num++;
				}
				finally
				{
					// 락에 성공했다면 작업이 끝날 때 해제해준다.
					if(lockToken)
						_spinLock.Exit();
				}
			}
		}
		static void Thread2()
		{
			for (int i = 0; i < 10000; i++)
			{
				bool lockToken = false;

				try
				{
					_spinLock.Enter(ref lockToken);
					_num--;
				}
				finally
				{
					if (lockToken)
						_spinLock.Exit();
				}
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
