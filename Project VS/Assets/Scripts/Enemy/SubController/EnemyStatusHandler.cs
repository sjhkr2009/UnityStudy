using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusHandler {
    public GameObject GameObject { get; }

    public Direction CurrentDirection { get; set; } = Direction.None;

    public bool IsDead { get; set; }
    public float Speed { get; set; }

    private float _hp;
    public float Hp {
        get => _hp;
        set => _hp = value.Clamp(0, MaxHp);
    }

    public float MaxHp { get; private set; }

    public EnemyStatusHandler(GameObject enemyObject) {
        GameObject = enemyObject;
    }

    public void Initialize(EnemyStat stat) {
        CurrentDirection = Direction.None;
        IsDead = false;
        
        MaxHp = stat.hp;
        Hp = stat.hp;
        Speed = stat.speed;
    }

    public void SetMaxHp(int maxHp) {
        MaxHp = maxHp;
    }
}
