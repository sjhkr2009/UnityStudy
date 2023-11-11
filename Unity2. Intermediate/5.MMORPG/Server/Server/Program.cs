using System;
using System.Net;
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
			RoomManager.Create();
			
			// DNS (Domain Name System)
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");

			//FlushRoom();
			JobTimer.Instance.Push(FlushRoom);

			while (true) {
				JobTimer.Instance.Flush();
			}
		}
	}
}
