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

			// 접속한 플레이어에게 모든 플레이어의 목록 전송
			S_PlayerList players = new S_PlayerList();
			foreach (ClientSession s in _sessions)
			{
				players.players.Add(new S_PlayerList.Player
				{
					isSelf = (s == session),
					playerId = s.SessionId,
					posX = s.PosX,
					posY = s.PosY,
					posZ = s.PosZ
				});
			}
			session.Send(players.Serialize());

			// 모든 플레이어에게 새로운 플레이어의 접속을 알림
			S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
			enter.playerId = session.SessionId;
			enter.posX = enter.posY = enter.posZ = 0f;
			Broadcast(enter.Serialize());
		}

		public void Leave(ClientSession session)
		{
			_sessions.Remove(session);

			// 모두에게 플레이어의 퇴장을 알림
			S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
			leave.playerId = session.SessionId;
			Broadcast(leave.Serialize());
		}

		// Broadcast 방식의 전송은 자주 사용되므로, 직렬화된 배열을 넘기면 _pendingList에 저장하는 전용 함수로 만들어준다.
		public void Broadcast(ArraySegment<byte> segment)
		{
			_pendingList.Add(segment);
		}

		public void Move(ClientSession session, C_Move packet)
		{
			session.PosX = packet.posX;
			session.PosY = packet.posY;
			session.PosZ = packet.posZ;

			S_BroadcastMove move = new S_BroadcastMove();
			move.playerId = session.SessionId;
			move.posX = session.PosX;
			move.posY = session.PosY;
			move.posZ = session.PosZ;

			Broadcast(move.Serialize());
		}

		public void Flush()
		{
			foreach (ClientSession s in _sessions)
				s.Send(_pendingList);

			//Console.WriteLine($"[GameRoom] Flushed {_pendingList.Count} items");
			_pendingList.Clear();
		}
	}
}
