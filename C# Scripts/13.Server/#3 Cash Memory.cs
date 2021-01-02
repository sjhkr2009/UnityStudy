using System;

namespace ServerCore
{
	class Program
	{
		// 캐시(cash): CPU에 장착된 고속 메모리. 메모리나 저장장치에 데이터를 옮기는 시간을 줄이기 위해 CPU에 내장된 임시 메모리 공간이다.

		// 캐시 철학
		// 1. Temporal Locality (잠정 구역성) : 시간적으로, 방금 사용된 변수가 다시 사용될 확률이 높다.
		// 2. Spacial Locality (공간 연관성) : 공간적으로, 방금 사용된 변수 근처에 있는 변수들이 다시 사용될 확률이 높다.
		
		static void Main(string[] args)
		{
			int[,] arr = new int[10000, 10000];
			// 참고: 2차 배열은 구조상 1차 배열을 일정 간격으로 나누어 놓은 것이다. (여기서는 1억 개의 원소를 1만개 단위로 구분해둔 것)

			// 모든 변수가 캐시 메모리에 올라가는 것이 아니므로, 서로 연관이 있는 변수가 캐시 메모리에 올라갈 가능성이 높다.
			
			// #case 1: [0,0], [1,0], [2,0], ... 순으로 접근한다.
			{
				long start = DateTime.Now.Ticks;

				for (int y = 0; y < 10000; y++)
					for (int x = 0; x < 10000; x++)
						arr[x, y] = 1;

				long end = DateTime.Now.Ticks;
				Console.WriteLine($"(y,x) 순서 걸린 시간: {end - start}");
			}

			// #case 2: [0,0], [0,1], [0,2], ... 순으로 접근한다.
			{
				long start = DateTime.Now.Ticks;

				for (int x = 0; x < 10000; x++)
					for (int y = 0; y < 10000; y++)
						arr[x, y] = 1;

				long end = DateTime.Now.Ticks;
				Console.WriteLine($"(x,y) 순서 걸린 시간: {end - start}");
			}

			// 2차 배열을 1차 배열로 생각해보면, case 1에서는 배열의 원소에 띄엄띄엄 접근하지만, case 2에서는 순차적으로 접근한다.
			// 따라서 수학적으로는 같은 시간복잡도를 갖는 동작임에도 속도에 차이가 생긴다. (후자가 더 빠름)
		}
	}
}
