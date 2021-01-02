using System;
using ServerCore;
using System.Collections.Generic;
using System.Text;

namespace Server
{
	// * JobTimer
	// 시간을 체크하여 일정 주기로 실행되는 작업들을 관리하는 클래스
	// Main 함수에서 작업마다 실행시간을 비교하는 대신, 우선순위 큐를 이용한다.
	// 가장 빨리 실행되어야 하는 작업을 log n의 시간복잡도로 구할 수 있다.

	struct JobTimerElem : IComparable<JobTimerElem>
	{
		public int executeTick;
		public Action action;
		
		public int CompareTo(JobTimerElem other)
		{
			// 실행 주기가 짧은 쪽이 우선된다. 비교대상보다 내가 더 짧다면 true(양수, 우선시됨)를 반환한다.
			return other.executeTick - executeTick;
		}
	}

	class JobTimer
	{
		PriorityQueue<JobTimerElem> _pq = new PriorityQueue<JobTimerElem>();
		object _lock = new object();

		public static JobTimer Instance { get; } = new JobTimer();

		// 실행할 동작과 지연시간을 입력하여, 작업을 큐에 추가한다.
		// 반복 실행되어야 한다면 작업 함수 내에 다시 작업을 추가하는 코드를 삽입해야 한다.
		public void Push(Action action, int tickAfter = 0)
		{
			JobTimerElem job;
			job.executeTick = System.Environment.TickCount + tickAfter;
			job.action = action;

			lock(_lock)
			{
				_pq.Push(job);
			}
		}

		// Main 함수에서 무한루프로 실행된다.
		// 가장 먼저 실행되어야 하는 작업의 실행 주기가 되었는지 확인한다.
		// 실행할 때가 되었다면 해당 작업을 큐에서 꺼내 Invoke, 아니라면 return한다.
		public void Flush()
		{
			while (true)
			{
				int now = System.Environment.TickCount;

				JobTimerElem job;

				lock(_lock)
				{
					if (_pq.Count == 0)
						break;

					job = _pq.Peek();
					if (job.executeTick > now)
						break;

					_pq.Pop();
				}

				job.action.Invoke();
			}
		}
	}

	// 경우에 따라 어느정도 간격을 두고 실행하는 작업은 위와 같이 PriorityQueue를 이용하고,
	// 빠른 주기로 실행하는 작업은 별도의 컨테이너에 넣어 시간을 재며(예를 들어 20ms 단위로) 하나씩 순차 실행시키기도 한다.
}
