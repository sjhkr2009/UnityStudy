using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
	class Program
	{
		// 경합 조건 (Race Condition)

		// 전역 변수 사용의 문제점.
		// 둘 이상의 명령어가 같은 메모리 공간(변수)을 참조할 때, 서로 경쟁함으로써 순서가 맞물려 수행 결과를 예측할 수 없게 되는 것.

		// 따라서, 어떤 동작들은 도중에 다른 연산이 개입하지 않고 한 덩어리처럼 (원자적으로) 수행되어야 한다.
		// 원자성(atomic): 더 세분화될 수 없음

		static int number = 0;

		static void Main(string[] args)
		{
			Task t1 = new Task(Thread1);
			Task t2 = new Task(Thread2);

			t1.Start();
			t2.Start();

			Task.WaitAll(t1, t2);

			// 같은 횟수만큼 더하고 뺐는데 숫자는 0이 나오지 않는다.
			Console.WriteLine(number);

			//-------------------------------------------------------------------------

			number = 0;

			Task t3 = new Task(Thread3);
			Task t4 = new Task(Thread4);

			t3.Start();
			t4.Start();

			Task.WaitAll(t3, t4);

			// Interlocked 사용 시 연산 중 다른 연산이 경합하지 않으므로 항상 0이 나온다.
			Console.WriteLine(number);
		}
		
		static void Thread1()
		{
			for (int i = 0; i < 10000; i++)
				number++;

			// number++의 연산 과정은 실제로는 다음과 같다.
			// int temp = number;
			// temp += 1;
			// number = temp;

			// 따라서 연산이 3단계로 나누어져 있어서 순서가 꼬일 경우 1씩 더하거나 뺀 temp가 제대로 반영되지 않을 수 있다.
			// ex) number가 0일 때 Thread1,2에서 각각 0을 받아가서, +1, -1을 한 후, 나온 값을 Thread1,2에서 차례로 number에 대입하면 최종 결과는 0이 아닌 -1이 될 것
		}
		static void Thread2()
		{
			for (int i = 0; i < 10000; i++)
				number--;
		}
		static void Thread3()
		{
			for (int i = 0; i < 10000; i++)
			{
				// Interlocked 계열 함수는 특정 변수에 대한 연산을 원자적으로 처리한다. 연산이 끝나면 값을 반환한다.
				// 하나의 Interlocked가 끝날 때까지 다른 Interlocked 연산은 대기해야 하므로 시간은 더 오래 걸릴 수 있지만 기본적으로 빠르고, 실행 시작 시점에 따라 실행 순서가 보장된다.
				// 다만 정수형 변수에 정해진 연산만 가능하다는 단점이 있다.
				Interlocked.Increment(ref number); // 지정한 변수를 1 증가시킨다.
			}
		}
		static void Thread4()
		{
			for (int i = 0; i < 10000; i++)
				Interlocked.Decrement(ref number); // 지정한 변수를 1 감소시킨다.
		}
	}
}
