using System;
using System.Collections.Generic;
using System.Text;

namespace DummyClient
{
	// DummyClient는 수많은 클라이언트들을 테스트하기 위한 것이므로 임시로 세션 매니저를 만들어준다.
	// 클라이언트 하나가 서버 세션을 여러 개 만들 일은 없으니 실제 상황에서는 매니저가 불필요하다.
	class SessionManager
	{
		static SessionManager _sessionManager = new SessionManager();
		public static SessionManager Instance => _sessionManager;

		List<ServerSession> _sessions = new List<ServerSession>();
		object _lock = new object();

		Random _rand = new Random();

		public ServerSession Generate()
		{
			lock(_lock)
			{
				ServerSession serverSession = new ServerSession();
				_sessions.Add(serverSession);
				return serverSession;
			}
		}

		// 테스트용으로 모든 클라이언트에서 메시지를 보내게 한다.
		public void SendForEach()
		{
			lock(_lock)
			{
				foreach (ServerSession s in _sessions)
				{
					C_Move movePacket = new C_Move();
					movePacket.posX = _rand.Next(-50, 50);
					movePacket.posY = 0;
					movePacket.posZ = _rand.Next(-50, 50);
					s.Send(movePacket.Serialize());
				}
			}
		}
	}
}
