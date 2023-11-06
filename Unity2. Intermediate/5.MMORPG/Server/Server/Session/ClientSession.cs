using System;
using ServerCore;
using System.Net;
using Google.Protobuf.Protocol;
using Google.Protobuf;

namespace Server {
	class ClientSession : PacketSession {
		public int SessionId { get; set; }

		public override void OnConnected(EndPoint endPoint) {
			Console.WriteLine($"OnConnected : {endPoint}");

			// PROTO Test
			S_Chat chat = new S_Chat() {
				Context = "안녕하세요"
			};

			Send(chat);
			

			//S_Chat chat2 = new S_Chat();
			//chat2.MergeFrom(sendBuffer, 4, sendBuffer.Length - 4);
			//////////////////////////
			//////////////////////////
			//Program.Room.Push(() => Program.Room.Enter(this));
		}

		public void Send(IMessage packet) {
			var msgName = packet.Descriptor.Name.Replace("_", string.Empty);
			if (!Enum.TryParse<MsgId>(msgName, out var msgId)) {
				Console.WriteLine($"[Error] Fail to send : {msgName} is not valid MsgId.");
				return;
			} 
			
			ushort size = (ushort)packet.CalculateSize();
			byte[] sendBuffer = new byte[size + 4];
			Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
			
			Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
			Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
			
			Send(new ArraySegment<byte>(sendBuffer));
		}

		public override void OnRecvPacket(ArraySegment<byte> buffer) {
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnDisconnected(EndPoint endPoint) {
			SessionManager.Instance.Remove(this);

			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes) {
			Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
