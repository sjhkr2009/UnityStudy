using System;
using ServerCore;
using System.Net;
using Google.Protobuf.Protocol;
using Google.Protobuf;
using Server.Game;

namespace Server {
	public class ClientSession : PacketSession {
		public Player MyPlayer { get; set; }
		public int SessionId { get; set; }

		public override void OnConnected(EndPoint endPoint) {
			Console.WriteLine($"OnConnected : {endPoint}");

			// PROTO Test
			MyPlayer = PlayerManager.Create();
			MyPlayer.Info.Name = $"Player{MyPlayer.Info.ObjectId:0000}";
			MyPlayer.Info.PosInfo.State = CreatureState.Idle;
			MyPlayer.Info.PosInfo.MoveDir = MoveDir.Down;
			MyPlayer.Info.PosInfo.PosX = 0;
			MyPlayer.Info.PosInfo.PosY = 0;
			MyPlayer.Session = this;

			RoomManager.First().EnterGame(MyPlayer);
		}

		public void Send(IMessage packet) {
			var msgName = packet.Descriptor.Name.Replace("_", string.Empty);
			if (!Enum.TryParse<MsgId>(msgName, out var msgId)) {
				Console.WriteLine($"[Error] Fail to send : {msgName} is not valid MsgId.");
				return;
			} 
			
			// 보낼 정보의 크기에, 메타 정보를 넣기 위해 4바이트를 추가
			ushort size = (ushort)packet.CalculateSize();
			byte[] sendBuffer = new byte[size + 4];
			
			// sendBuffer의 첫 2바이트는 버퍼 크기를 쓰고...
			Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
			
			// 그 다음 2바이트는 프로토콜 ID를, 그 이후에 보낼 정보를 기입한다.
			Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
			Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
			
			Send(new ArraySegment<byte>(sendBuffer));
		}

		public override void OnRecvPacket(ArraySegment<byte> buffer) {
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnDisconnected(EndPoint endPoint) {
			RoomManager.Find(MyPlayer.Room.RoomId)?.LeaveGame(MyPlayer.Info.ObjectId);
			SessionManager.Instance.Remove(this);

			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes) {
			Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
