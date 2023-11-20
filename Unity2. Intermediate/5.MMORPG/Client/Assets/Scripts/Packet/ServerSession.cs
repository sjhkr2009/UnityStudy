using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ServerSession : PacketSession {
	public override void OnConnected(EndPoint endPoint) {
		Debug.Log($"OnConnected : {endPoint}");

		PacketManager.Instance.CustomHandler = (ss, msg, id) => {
			PacketQueue.Instance.Push(id, msg);
		};
	}

	public override void OnDisconnected(EndPoint endPoint) {
		Debug.Log($"OnDisconnected : {endPoint}");
	}

	public override void OnRecvPacket(ArraySegment<byte> buffer) {
		PacketManager.Instance.OnRecvPacket(this, buffer);
	}

	public override void OnSend(int numOfBytes) {
		//Console.WriteLine($"Transferred bytes: {numOfBytes}");
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
}
