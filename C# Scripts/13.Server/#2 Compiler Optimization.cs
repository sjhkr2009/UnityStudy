using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	class Program
	{
		// 컴파일러 최적화: 코드를 기계어로 변환하는 과정에서, 컴파일러에 의해 일부 코드가 최적화에 맞게 변경되는 것.
		//					컴파일러는 멀티스레드를 생각하지 않는다. 따라서 디버그 모드에선 일어나지 않던 에러들이 발생할 수 있다.

		// volatile 키워드를 붙인 변수는 여러 스레드에 의해 언제 변할지 모름을 암시하므로, 이 키워드가 붙으면 최적화하지 않는다.
		// 다만 C#에서 volatile는 특이한 동작 방식을 가지며, 이 키워드를 대체할 요소들이 있어서 실제로는 잘 사용되지 않는다.
		volatile static bool _stop = false;

		static void Main(string[] args)
		{
			Task t = new Task(ThreadMain);
			t.Start();

			// 1000ms(= 1초) 만큼 기다린다.
			Thread.Sleep(1000);
			
			_stop = true;

			Console.WriteLine("Stop 호출");
			Console.WriteLine("종료 대기중");

			// Task 종료 시까지 대기할 때는 Wait()을 사용한다. Thread의 Join()과 비슷한 기능.
			t.Wait();

			Console.WriteLine("종료 성공");
		}
		static void ThreadMain()
		{
			Console.WriteLine("쓰레드 시작!");

			while (!_stop) 
			{
				// 누군가 stop 신호를 줄 때까지 기다린다
			}
			// 'volatile' 키워드가 없다면, Release 모드에서 위 코드는 컴파일러에 의해 아래와 같이 변경된다.
			/*
			 * if (!_stop)
			 * {
			 *		while(true) { }
			 * }
			 */
			// while문 내에서 조건식인 _stop에 대한 처리 부분이 없으니, 매번 조건을 체크하지 않도록 if문으로 변경하여 내부에서 반복시키게 한다.
			// 컴파일러는 멀티스레드 개념이 없어서, 다른 스레드가 변수를 변경한다고 생각하지 못 하기 때문.
			
			Console.WriteLine("쓰레드 종료!");
		}
	}
}
