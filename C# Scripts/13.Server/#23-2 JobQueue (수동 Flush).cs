using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
	// 참고용 : JobQueue 수동으로 실행하기
	// 저장된 작업을 특정 시점에 수동으로 처리할 경우 이런 방식으로 사용한다.
	// 이런 방식으로 구현하는 경우도 많다. (특히 람다식이 사용되지 않던 예전에는 더욱)
	
	// 작업이 공통적으로 상속받는 인터페이스
	interface ITask
	{
		void Execute();
	}
	// ITask를 상속받아 작업 클래스 생성
	class Broadcast : ITask
	{
		GameRoom _room;
		ClientSession _session;
		string _chat;

		public Broadcast(GameRoom room, ClientSession session, string chat)
		{
			_room = room;
			_session = session;
			_chat = chat;
		}

		public void Execute()
		{
			_room.Broadcast(_session, _chat); 
		}
	}
	// ITask를 큐에 쌓아두고 함수를 호출하여 수동으로 처리할 수 있다.
	// (클래스명은 JobQueue와 겹치지 않게 임의로 TaskQueue라고 한다)
	class TaskQueue
	{
		Queue<ITask> _tasks = new Queue<ITask>();

		public void Flush()
		{
			while(_tasks.Count > 0)
			{
				ITask task = _tasks.Dequeue();
				task.Execute();
			}
		}
	}
}
