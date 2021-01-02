using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
	// 클라이언트 세션이 입장할 게임 채널의 역할을 하는 클래스.
	// 입장과 퇴장, 채팅 메시지 전송이 가능하다.
	class GameRoom : IJobQueue
	{
		List<ClientSession> _sessions = new List<ClientSession>();

		JobQueue _jobQueue = new JobQueue();
		public void Push(Action job)
		{
			_jobQueue.Push(job);
		}

		// - 삭제된 부분
		// JobQueue에서 lock을 걸어 한 번에 한 작업씩 큐에 넣고 있으므로, 별도의 lock은 필요없다.
		// _lock 오브젝트 및 모든 함수의 lock 삭제
		public void Enter(ClientSession session)
		{
			_sessions.Add(session);
			session.Room = this;
		}

		public void Leave(ClientSession session)
		{
			_sessions.Remove(session);
		}

		public void Broadcast(ClientSession session, string chat)
		{
			S_Chat packet = new S_Chat();
			packet.playerId = session.SessionId;
			packet.chat = $"[Player {packet.playerId}] : {chat}";
			ArraySegment<byte> segment = packet.Serialize();

			foreach (ClientSession s in _sessions)
				s.Send(segment);
		}
	}
}
