using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus {
    public GameObject GameObject { get; }
    public Transform Transform { get; }

    public Direction CurrentDirection { get; set; } = Direction.None;
    
    public EnemyTier Tier { get; set; }
    
    public bool IsDead { get; set; }
    public bool IsMovable { get; set; }
    public float Speed { get; set; }
    public float AttackDamage { get; set; }
    public List<DropTable> DropTables { get; private set; }
    
    private float _hp;
    public float Hp {
        get => _hp;
        set => _hp = value.Clamp(0, MaxHp);
    }

    public float MaxHp { get; private set; }

    public EnemyStatus(GameObject enemyObject) {
        GameObject = enemyObject;
        Transform = GameObject.transform;
    }

    public void Initialize(EnemyStatData statData) {
        CurrentDirection = Direction.None;
        IsDead = false;
        IsMovable = true;
        
        MaxHp = statData.hp;
        Hp = statData.hp;
        Speed = statData.speed;
        Tier = statData.tier;
        AttackDamage = statData.attackDamage;
        DropTables = statData.dropTables;
    }

    public void SetMaxHp(int maxHp) {
        MaxHp = maxHp;
    }
}
