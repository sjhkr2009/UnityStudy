using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerStat : Stat
{
    [ShowInInspector, BoxGroup("Player")] public int Exp
	{
        get => _exp;
		set
		{
            int total = _exp + value;
            if (total < RequiredExp)
			{
                _exp = total;
            }
            else
			{
                total -= RequiredExp;
                Level++;
                SetStat(Level);
                Exp = total;
			}


		}
	}
    int _exp;
    [ShowInInspector, BoxGroup("Player")] public int RequiredExp { get; set; }
    [ShowInInspector, BoxGroup("Player")] public int Gold { get; set; }

	protected override void Start()
	{
        if (Level <= 0)
		{
            Level = 1;

            SetStat(Level);

            Defense = 5;
            MoveSpeed = 5f;
            AttackRange = 1.5f;
        }
        Hp = MaxHp;

        Exp = 0;
		Gold = 0;
	}

    public void SetStat(int level)
	{
        Data.Stat stat = GameManager.Data.StatDict[level];

        MaxHp = stat.hp;
        Attack = stat.attack;
        RequiredExp = stat.totalExp;
	}

	protected override void OnDead(Stat attacker)
	{
        Debug.Log("Player Dead");
	}
}
