using System;
using System.Net;
using System.Threading;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Game;
using ServerCore;

namespace Server {
	class Program {
		static Listener _listener = new Listener();

		static void FlushRoom() {
			JobTimer.Instance.Push(FlushRoom, 250);
		}

		static void Main(string[] args) {
			RoomManager.Create(1);
			
			// DNS (Domain Name System)
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");

			//FlushRoom();
			//JobTimer.Instance.Push(FlushRoom);

			while (true) {
				//JobTimer.Instance.Flush();
				
				// TODO: 일단 0.1초마다 반복, 추후 별도의 Task로 뺄 것 
				RoomManager.Find(1).Update();
				Thread.Sleep(100);
			}
		}
	}
}
