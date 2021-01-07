﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
	// 서버에서 클라이언트 세션의 생성 및 관리 역할을 한다.
	class SessionManager
	{
		static SessionManager _sessionManager = new SessionManager();
		public static SessionManager Instance => _sessionManager;

		int _sessionId = 0;
		Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();

		object _lock = new object();

		public ClientSession Generate()
		{
			lock(_lock)
			{
				int sessionId = ++_sessionId;

				ClientSession session = new ClientSession();
				session.SessionId = sessionId;
				_sessions.Add(sessionId, session);

				Console.WriteLine($"Connected : {sessionId}");

				return session;
			}
		}

		public ClientSession Find(int id)
		{
			lock(_lock)
			{
				ClientSession session = null;
				_sessions.TryGetValue(id, out session);
				return session;
			}
		}

		public void Remove(ClientSession session)
		{
			lock(_lock)
			{
				_sessions.Remove(session.SessionId);
			}
		}
	}
}
