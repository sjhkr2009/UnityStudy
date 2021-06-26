using DummyClient;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

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
    }

    void Update()
    {
        List<IPacket> packets = PacketQueue.Instance.PopAll();
		foreach (IPacket packet in packets)
		{
            PacketManager.Instance.HandlePacket(_session, packet);
		}
    }

    public void Send(ArraySegment<byte> sendBuff)
	{
        _session.Send(sendBuff);
    }
}
