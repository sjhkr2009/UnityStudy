using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

// * 패킷 모아 보내기

// 모든 유저의 패킷 정보를 받아 다시 모든 유저에게 뿌리면 시간 복잡도는 O(N^2)가 된다.
// 따라서 서버에서 모든 유저에게 for문을 돌며 하나하나 패킷을 전송하는 것은 비효율적이다.
// 패킷을 일정량 모아서 Send() 처리하면 효율을 높일 수 있다.

// 크게 두 가지 방법이 있다.
// 1. (서버 쪽) Session.Send() 함수를 패킷을 바로 전송하지 않고, 일정량 쌓이면 보내도록 수정
// 2. (컨텐츠 쪽) GameRoom에서 자체적으로 패킷을 일정량 모아서 Send()를 호출하기
// 여기선 두 번째 방법으로 구현해보기로 한다.

namespace Server
{
	// 클라이언트 세션이 입장할 게임 채널의 역할을 하는 클래스.
	// 입장과 퇴장, 채팅 메시지 전송이 가능하다.
	class GameRoom : IJobQueue
	{
		List<ClientSession> _sessions = new List<ClientSession>();
		JobQueue _jobQueue = new JobQueue();

		// 패킷을 모을 리스트를 생성한다.
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

		/// 작업을 JobQueue에 보내서 순차적으로 실행시킵니다.
		public void Push(Action job)
		{
			_jobQueue.Push(job);
		}

		public void Enter(ClientSession session)
		{
			_sessions.Add(session);
			session.Room = this;
		}

		public void Leave(ClientSession session)
		{
			_sessions.Remove(session);
		}

		// * 수정된 부분
		// 이제 Broadcast는 패킷을 전송하지 않고, 전송할 패킷을 조립해서 리스트에 넣어두는 역할만 한다.
		public void Broadcast(ClientSession session, string chat)
		{
			S_Chat packet = new S_Chat();
			packet.playerId = session.SessionId;
			packet.chat = $"[Player {packet.playerId}] : {chat}";
			ArraySegment<byte> segment = packet.Serialize();

			// 바로 전송하는 대신 리스트에 패킷을 넣는다.
			//foreach (ClientSession s in _sessions)
			//	s.Send(segment);
			_pendingList.Add(segment);
		}

		// + 추가된 부분
		// 각 클라이언트에 패킷 리스트를 전송한다. 외부에서 일정 간격으로 호출된다.
		public void Flush()
		{
			foreach (ClientSession s in _sessions)
				s.Send(_pendingList);

			Console.WriteLine($"[GameRoom] Flushed {_pendingList.Count} items");
			_pendingList.Clear();
		}
	}
}
