using System;
using System.Threading; // 스레드 및 스레드 풀 사용을 위해 필요하다.
using System.Threading.Tasks; // Task 사용을 위해 필요하다.

namespace ServerCore
{
	class Program
	{
		// 각 스레드는 자신만의 스택 메모리를 할당받지만, 힙 영역(new로 생성된 객체)과 데이터 영역(static 변수)은 공유한다.
		// 전역 변수는 모든 스레드가 공통으로 접근할 수 있다.

		static void Main(string[] args)
		{
			// 스레드 생성
			Thread t = new Thread(MainThread1);
			
			// 스레드의 이름을 지정할 수 있다. 기본적으로는 이름 없는 스레드가 생성된다.
			t.Name = "TestThread";

			// 백그라운드 실행 여부를 설정할 수 있다. 기본적으로는 false로 생성된다.
			// 백그라운드 실행 시 Main 함수가 종료되면 해당 스레드의 처리가 끝나든 아니든 프로그램을 종료한다.
			t.IsBackground = true;
			t.Start();

			// Join()을 사용하면 (백그라운드 실행 여부와 무관하게) 해당 스레드의 작업이 끝날 때까지 기다린다. (C++도 Join()을 사용함)
			Console.WriteLine("Waiting for Thread...");
			t.Join();
			Console.WriteLine("Hello World!");

			// 스레드 생성 개수에는 제한이 없으나, 실제로 동시에 작동하는 스레드 수는 물리적인 코어 수를 넘을 수 없으므로 스레드들을 오가며 작업을 처리하게 된다.
			// 지나치게 많은 스레드 생성은 스레드 사이를 오가는 비용만 커지므로, 물리적인 CPU 코어 수를 넘지 않도록 맞춰주는 것이 효율적이다.

			Console.WriteLine("------------------ ThreadPool ------------------");

			// 스레드 생성은 비용이 크기 때문에, 간단한 작업은 ThreadPool을 통해 실행할 수 있다.
			// object 타입 매개변수를 하나 받는 함수여야 하고, 기본적으로 백그라운드로 실행됨에 유의할 것.
			ThreadPool.QueueUserWorkItem(DoSomething);

			// 스레드 풀은 최대 스레드 수가 정해져 있어서, 많은 작업을 넘기더라도 일정 개수가 넘어가면 기존 작업을 끝낸 후 다음 작업을 시작하게 된다.
			// 이는 스레드 수에 신경쓰지 않아도 된다는 장점이 있으나, 간단한 작업이 대기중인데 기존의 긴 작업을 처리하느라 시간이 많이 지연될 수 있다는 단점이 있다.
			ThreadPool.SetMinThreads(1, 1); 
			ThreadPool.SetMaxThreads(2, 2); // 테스트를 위해 스레드 수를 2개로 제한한다.

			ThreadPool.QueueUserWorkItem((obj) => { for (int i = 0; i < 500; i++) if ((i+1) % 100 == 0) Console.WriteLine($"Do Something Hard 1... {(i + 1) / 100} / 5"); });
			ThreadPool.QueueUserWorkItem((obj) => { for (int i = 0; i < 500; i++) if ((i+1) % 100 == 0) Console.WriteLine($"Do Something Hard 2... {(i + 1) / 100} / 5"); });

			// 앞의 두 무한루프가 2개의 스레드를 차지하고 있어서, 간단한 작업을 바로 할 수 없다.
			ThreadPool.QueueUserWorkItem((obj) => { Console.WriteLine("Do Something Easy in ThreadPool!"); });

			//------------------ Task ------------------

			// Task를 통해 수행할 일을 함수 단위로 실행할 수 있다. 생성된 Task는 ThreadPool의 설정을 따라 자동으로 스레드에 배분되어 실행된다.
			// TaskCreationOptions.LongRunning : 해당 동작이 오래 걸린다고 선언한다. 이러면 가벼운 작업들은 별도로 처리할 수 있어서, 위의 ThreadPool이 가지는 문제점을 해결할 수 있다.
			Task task = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning);
			task.Start();

			// 최대 스레드를 2개로 설정했으므로 오래 걸린다고 명시하지 않으면 이 동작이 끝날 때까지 다음 동작이 실행되지 않는다.
			//new Task(() => { while (true) { } }).Start();
			//new Task(() => { while (true) { } }).Start();

			// 무한루프를 2개 실행했지만 오래 걸리는 작업임을 표기했으므로, 끝날 때까지 기다리지 않고 다음 작업도 수행한다.
			new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning).Start();
			new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning).Start();
			new Task(() => { Console.WriteLine("Do Something Easy in Task!"); }).Start();

			// 프로그램이 끝나지 않게 하기 위한 임시 코드
			while (true) { }
		}

		static void MainThread1()
		{
			for (int i = 1; i <= 100; i++)
				Console.WriteLine($"Hello Thread 1 - {i}");

			Console.WriteLine("Thread 1 Complete!");
		}

		static void DoSomething(object state)
		{
			for (int i = 0; i < 10; i++)
				Console.WriteLine($"아주 가벼운 작업 {i + 1}");
		}
	}
}
