using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
	// 서버 패킷 처리 - 커맨드 패턴 구현
	// 개별 작업마다 lock을 걸고 처리하면, 패킷이 몰릴 때 수많은 스레드가 대기하고 있게 된다.
	// 이로 인해 기존 스레드에서 작업이 끝나지 않아서 수많은 스레드가 만들어지게 되는 악순환이 일어난다.

	// 개별 작업을 보통 Task 또는 Job 이라고 부르는데, 여기선 JobQueue에 작업을 저장하고 하나씩 실행하게 한다.
	// 기존 스레드들은 JobQueue에 작업을 저장하고 다른 일을 할 수 있으므로 위의 현상이 일어나지 않는다.
	public interface IJobQueue
	{
		void Push(Action job);
	}
	public class JobQueue : IJobQueue
	{
		Queue<Action> _jobQueue = new Queue<Action>();
		object _lock = new object();
		bool _flush = false;

		// lock을 걸고 한 작업씩 큐에 넣는다.
		// 작업이 처리중인지 임계 구역에서 체크하고, 작업이 가능한 경우 Flush()를 호출한다.
		public void Push(Action job)
		{
			bool flush = false;
			lock(_lock)
			{
				_jobQueue.Enqueue(job);
				if (_flush == false)
					flush = _flush = true;
			}

			if (flush)
				Flush();
		}

		// 큐에 쌓인 작업들을 순서대로 모두 처리한다.
		// Push/Pop에서 lock을 걸어 하나씩 처리되니 Flush() 또는 외부에서 Push/Pop 호출 시 별도의 lock은 불필요하다.
		void Flush()
		{
			while(true)
			{
				Action action = Pop();
				if (action == null)
					return;

				action.Invoke();
			}
		}

		Action Pop()
		{
			lock(_lock)
			{
				if(_jobQueue.Count == 0)
				{
					_flush = false;
					return null;
				}
				
				return _jobQueue.Dequeue();
			}
		}
	}
}
