using DummyClient;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// 서버-유니티 연동
// 이제 DummyClient 대신 유니티가 클라이언트 역할을 하도록 만든다.

public class NetworkManager : MonoBehaviour
{
    ServerSession _session = new ServerSession();
    
    void Start()
    {
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAdr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAdr, 7777);

        Connector connector = new Connector();
        connector.Connect(endPoint, () => { return _session; }, 1);

        StartCoroutine(nameof(CoSendPacket));
    }

    void Update()
    {
        // 유니티 메인 스레드에서 실행하도록 패킷을 큐에 넣어두고, 하나씩 꺼내 사용한다.
        // 아래와 같이 하면 프레임당 하나의 패킷만 처리될텐데, 언제 얼마나 처리할지는 게임에 따라 결정하면 된다.
        IPacket packet = PacketQueue.Instance.Pop();
        if(packet != null)
		{
            PacketManager.Instance.HandlePacket(_session, packet);
		}
    }

    // 패킷 전송 테스트
    IEnumerator CoSendPacket()
	{
        while(true)
		{
            yield return new WaitForSeconds(3f);

            C_Chat chatPacket = new C_Chat();
            chatPacket.chat = "Hello Unity!";
            ArraySegment<byte> segment = chatPacket.Serialize();

            _session.Send(segment);
		}
	}
}
