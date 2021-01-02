using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	// Thread Local Storage (TLS)

	// lock을 거는 것에도 부하가 걸리기 때문에, 게임 내 모든 요소에 lock을 걸면 한쪽에 대량의 부하가 몰릴 때 오히려 비효율적일 수 있다.
	// lock은 하나의 스레드만 접근 가능하므로, 모든 작업에 대해 lock을 잠그고 해제하는 작업이 추가로 필요하기 때문.
	// 이 때문에 부하가 몰릴 때는 여러 개의 작업을 모아서 임계 구역에 진입, 한 번에 처리하는 것이 효율적이다.

	// 각 스레드는 힙/데이터 영역은 공유하지만, 고유의 스택을 가진다.
	// 하지만 스택 메모리는 함수의 콜스택이나 지역 변수 등에 임시로 사용되고, 호출이 끝나면 해당 부분은 비워지므로 불안정하다.
	// 따라서 각 스레드는 'TLS'라는 별도의 공간에 처리할 데이터들을 저장한다.

	// 스레드 고유의 전역 변수처럼 사용한다.

	class Program
	{
		// ThreadLocal을 사용하면 스레드의 TLS 영역에 데이터를 저장할 수 있다. 지역 변수는 스택에 저장하면 되기 때문에 대부분 static으로 사용하게 된다.
		// .Value로 값을 대입하면 해당 값은 TLS에 저장된다. (또는 아래의 생성자 사용)
		static ThreadLocal<string> ThreadName1 = new ThreadLocal<string>();

		// 생성자를 이용하여 string을 받으면, 해당 스레드에서 'ThreadName2'를 생성할 때 값을 넣어준다.
		// IsValueCreated로 이 스레드의 TLS값 유뮤룰 체크해서, 이미 생성되어 있다면 매번 값을 대입하는 과정을 생략할 수 있으므로 작업량을 줄일 수 있다.
		static ThreadLocal<string> ThreadName2 = new ThreadLocal<string>(() => $"[TLS] My Name is {Thread.CurrentThread.ManagedThreadId}");
		static string sThreadName;

		static void WhoAmI_str()
		{
			sThreadName = $"[string] My Name is {Thread.CurrentThread.ManagedThreadId}";

			Thread.Sleep(500);
			// static string은 공유 영역인 데이터 영역에 있으므로, 각 스레드가 값을 대입할 때마다 값을 덮어쓰게 된다.
			// 0.5초 쉬었다가 출력해보면 마지막으로 대입한 값 하나만 나오게 될 것이다.
			Console.WriteLine(sThreadName);
		}

		static void WhoAmI_TL1()
		{
			ThreadName1.Value = $"[TLS1] My Name is {Thread.CurrentThread.ManagedThreadId}";

			Thread.Sleep(500);
			// 이 때는 각 스레드에 할당된 TLS 영역에 값이 저장되므로, 스레드마다 다른 값이 나온다.
			Console.WriteLine(ThreadName1.Value);
		}

		static void WhoAmI_TL2()
		{
			// 위의 예시에서 매번 대입하는 부분을 제거하려면 생성자를 사용한다.
			
			bool repeat = ThreadName2.IsValueCreated; // ThreadName2이 이 스레드의 TLS에 이미 만들어져 있으면 true, 아니라면 false를 반환한다.
			if (repeat)
				Console.WriteLine(ThreadName2.Value + "(repeat)");
			else
				Console.WriteLine(ThreadName2.Value);
		}

		static void Main(string[] args)
		{
			ThreadPool.SetMinThreads(1, 1);
			ThreadPool.SetMaxThreads(4, 4); // repeat 테스트를 위해 스레드 사용은 4개로 제한한다.

			// Parallel 클래스는 많은 양의 작업을 각 스레드에 분배하여 병렬적으로 처리할 수 있게 해 준다.
			// 여기선 스레드를 4개로 제한했으니, 8개의 명령을 병렬로 시키면 4개씩 실행될 것이다.
			Parallel.Invoke(WhoAmI_str, WhoAmI_str, WhoAmI_str, WhoAmI_str, WhoAmI_str, WhoAmI_str, WhoAmI_str, WhoAmI_str);
			Thread.Sleep(1000);
			Parallel.Invoke(WhoAmI_TL1, WhoAmI_TL1, WhoAmI_TL1, WhoAmI_TL1, WhoAmI_TL1, WhoAmI_TL1, WhoAmI_TL1, WhoAmI_TL1);
			Thread.Sleep(1000);
			Parallel.Invoke(WhoAmI_TL2, WhoAmI_TL2, WhoAmI_TL2, WhoAmI_TL2, WhoAmI_TL2, WhoAmI_TL2, WhoAmI_TL2, WhoAmI_TL2);

			// 사용이 끝났으면 .Dispose로 해제할 수 있다.
			ThreadName2.Dispose();
		}
		
	}
}
