using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    MyPlayer _myPlayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance { get; } = new PlayerManager();

    public void Add(S_PlayerList packet)
	{
        Object obj = Resources.Load("Player");

		foreach (S_PlayerList.Player player in packet.players)
		{
            GameObject go = Object.Instantiate(obj) as GameObject;

            if(player.isSelf)
			{
                _myPlayer = go.AddComponent<MyPlayer>();
                _myPlayer.PlayerId = player.playerId;
                _myPlayer.transform.position = new Vector3(player.posX, player.posY, player.posZ);
            }
			else
			{
                Player p = go.AddComponent<Player>();
                p.PlayerId = player.playerId;
                p.transform.position = new Vector3(player.posX, player.posY, player.posZ);
                _players.Add(player.playerId, p);
            }
        }
	}

    public void EnterGame(S_BroadcastEnterGame packet)
	{
        if (packet.playerId == _myPlayer.PlayerId)
            return;
        
        Object obj = Resources.Load("Player");
        GameObject go = Object.Instantiate(obj) as GameObject;

        Player player = go.AddComponent<Player>();
        player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        _players.Add(packet.playerId, player);
    }

    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if(_myPlayer.PlayerId == packet.playerId)
		{
            Object.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
            return;
		}

        Player player = null;
        if (_players.TryGetValue(packet.playerId, out player))
        {
            Object.Destroy(player.gameObject);
            _players.Remove(player.PlayerId);
        }
    }

    public void Move(S_BroadcastMove packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
		{
            _myPlayer.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
            return;
		}
		else
		{
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
            }
        }
    }
}
