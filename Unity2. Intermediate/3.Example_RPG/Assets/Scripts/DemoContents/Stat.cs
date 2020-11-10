using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Stat : MonoBehaviour
{
    int _level;
    int _hp;
    int _maxHp;
    int _attack;
    int _defense;
    float _speed;

    [ShowInInspector, BoxGroup("Basic")] public int Level { get; set; }
    [ShowInInspector, BoxGroup("Basic")] public int Hp { get; set; }
    [ShowInInspector, BoxGroup("Basic")] public int MaxHp { get; set; }
    [ShowInInspector, BoxGroup("Basic")] public int Attack { get; set; }
    [ShowInInspector, BoxGroup("Basic")] public int Defense { get; set; }
    [ShowInInspector, BoxGroup("Basic")] public float MoveSpeed { get; set; }
    [ShowInInspector, BoxGroup("Basic")] public float AttackRange { get; set; }

    protected virtual void Start()
	{
		if (Level <= 0)
		{
            Level = 1;
            MaxHp = 100;
            Attack = 10;
            Defense = 0;
            MoveSpeed = 5f;
            AttackRange = 1.5f;
        }
        Hp = MaxHp;

    }

    public virtual void OnDamaged(Stat attacker)
	{
        int damage = Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        
        if(Hp <= 0)
		{
            Hp = 0;
            OnDead(attacker);
		}
    }

    protected virtual void OnDead(Stat attacker)
	{
        PlayerStat playerStat = (PlayerStat)attacker;
        if (playerStat != null)
        {
            playerStat.Exp += 5;
        }

        GameManager.Game.Despawn(gameObject);
	}
}
