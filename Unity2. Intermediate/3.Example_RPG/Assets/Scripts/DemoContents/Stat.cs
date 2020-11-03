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

	protected virtual void Start()
	{
		if (Level <= 0)
		{
            Level = 1;
            MaxHp = 100;
            Attack = 10;
            Defense = 5;
            MoveSpeed = 5f;
		}
        Hp = MaxHp;

    }
}
