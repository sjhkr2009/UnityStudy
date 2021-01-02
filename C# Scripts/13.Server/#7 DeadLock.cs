using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	class Program
	{
		// 데드락 (Deadlock)
		// 이전에 다룬 'lock을 해제하지 않아서 생기는' 데드락은 가장 기초적인 데드락이며, 단순히 프로그래머의 실수로 발생한 것이다.
		// 데드락이 걸리는 상황은 여러가지가 있다.

		// 데드락 조건
		// 1. 상호 배제(Mutual Exclusion):		자원에는 한 스레드만 접근 가능해야 한다
		// 2. 점유 상태로 대기(Hold and Wait):	한 자원의 점유를 해제하지 않은 상태에서 다른 자원을 점유하기 위해 기다린다
		// 3. 선점 불가(No Preemption):			한 스레드가 점유중인 자원은 다른 스레드가 뺏을 수 없다
		// 4. 순환 대기(Circular Wait):			2개 이상의 자원이 서로를 다음 작업으로 요구하여, 하나를 점유한 후 다른 하나에 접근하는 관계에 있다.

		// 임계 구역에 접근할 수 없을 때 (= 누가 점유중일 때) 3가지 동작을 취할 수 있다.
		// 1. 해당 자원의 점유가 가능해질 때까지 루프를 돌며 기다린다.
		// 2. 다른 작업을 하다가 일정 시간 경과 후 다시 접근을 시도한다.
		// 3. 해당 자원의 점유가 가능해지면 이벤트를 받도록 등록한 후 다른 작업을 한다.

		// 예시)
		// object 두 개(obj1, obj2)를 사용하여 잠근 임계 구역이 있다고 가정하자. 이 구역은 둘 다 lock을 설정해야 접근 가능하다.
		// 한 쓰레드가 obj1 -> obj2 순으로 잠그고 다른 쓰레드가 obj2 -> obj1 순서로 잠근다면,
		// 두 쓰레드가 동시에 접근하여 각각 obj1, obj2를 하나씩 잠근 채 다른 쓰레드가 잠금을 해제할 때까지 기다린다면 영원히 기다리는 상태가 된다.


		// 서로 자주 오가며 작업을 수행하는 두 매니저가 있다고 가정한다.
		class SessionManager
		{
			static object _lock = new object();

			public static void Test()
			{
				lock (_lock) { UserManager.TestUser(); }
			}
			public static void TestSession()
			{
				lock (_lock)
				{
					Console.WriteLine("Doing Session Test...");
				}
			}
		}
		class UserManager
		{
			static object _lock = new object();

			public static void Test()
			{
				lock (_lock) { SessionManager.TestSession(); }
			}
			public static void TestUser()
			{
				lock (_lock)
				{
					Console.WriteLine("Doing User Test...");
				}
			}
		}

		// 참고:	Monitor.TryEnter(obj, int)로 일정 시간 lock을 획득하지 못하면 false를 반환하게 처리할 수도 있다.
		//			다만 애초에 lock 구조에 문제가 있다면 TryEnter로 넘기기보단 데드락 발생을 확인하고 구조를 수정하는 게 좋다.
		//			데드락은 동시에 요청이 일어나지 않으면 확인할 수 없어서 예방이 어렵지만, 일단 발생하면 콜스택을 추적해서 대부분 쉽게 찾아낼 수 있기 때문.
		//			ㄴ (가령 아래 데드락 예시도 Thread.Sleep으로 t1, t2 실행에 0.1초정도 텀을 두면 발생하지 않는다)
		static void Main(string[] args)
		{
			Task t1 = new Task(Thread1);
			Task t2 = new Task(Thread2);

			Console.WriteLine("Case 1 Start");

			t1.Start();
			t2.Start();

			Task.WaitAll(t1, t2);

			// 데드락에 걸려 완료되지 않음
			Console.WriteLine("Case 1 Complete");

		}
		
		static void Thread1()
		{
			for (int i = 0; i < 1000; i++)
				SessionManager.Test();
		}
		static void Thread2()
		{
			for (int i = 0; i < 1000; i++)
				UserManager.Test();
		}
	}
}
