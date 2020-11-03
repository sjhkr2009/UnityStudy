using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerStat : Stat
{
    [ShowInInspector, BoxGroup("Player")] public int Exp { get; set; }
    [ShowInInspector, BoxGroup("Player")] public int Gold { get; set; }

	protected override void Start()
	{
		base.Start();

		Exp = 0;
		Gold = 0;
	}
}
