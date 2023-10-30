using System;
using System.Net;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;

namespace Server {
	class Program {
		static Listener _listener = new Listener();

		static void FlushRoom() {
			JobTimer.Instance.Push(FlushRoom, 250);
		}

		static void Main(string[] args) {
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

		static void Example() {
			// 구글 Protobuf 튜토리얼을 활용한 예제 (https://protobuf.dev/getting-started/csharptutorial/)
			Person person = new Person() {
				Id = 1234,
				Name = "John Doe",
				Email = "jdoe@example.com",
				Phones = { new Person.Types.PhoneNumber { Number = "555-4321", Type = Person.Types.PhoneType.Home } }
			};
			
			// 보낼 정보의 크기에, 메타 정보를 넣기 위해 4바이트를 추가
			ushort size = (ushort)person.CalculateSize();
			byte[] sendBuffer = new byte[size + 4];
			
			// sendBuffer의 첫 2바이트는 버퍼 크기를 쓰고...
			Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
			
			// 그 다음 2바이트는 프로토콜 ID를,
			ushort protocolId = 1;
			Array.Copy(BitConverter.GetBytes(protocolId), 0, sendBuffer, 2, sizeof(ushort));
			
			// 그 이후에 보낼 정보를 기입한다.
			Array.Copy(person.ToByteArray(), 0, sendBuffer, 4, size);

			Person person2 = new Person();
			person2.MergeFrom(sendBuffer);
		}
	}
}
