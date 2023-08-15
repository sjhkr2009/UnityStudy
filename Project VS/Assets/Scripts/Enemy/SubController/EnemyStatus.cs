using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus {
    public GameObject GameObject { get; }
    public Transform Transform { get; }
    public Rigidbody2D Rigidbody { get; }

    public Direction CurrentDirection { get; set; } = Direction.None;
    
    public EnemyTier Tier { get; set; }
    
    public bool IsDead { get; set; }
    public bool IsMovable { get; set; }
    public float Speed { get; set; }
    public float AttackDamage { get; set; }
    public IReadOnlyList<DropTable> DropTables { get; private set; }
    
    private float _hp;
    public float Hp {
        get => _hp;
        set => _hp = value.Clamp(0, MaxHp);
    }

    public float MaxHp { get; private set; }

    public EnemyStatus(GameObject enemyObject) {
        GameObject = enemyObject;
        Transform = GameObject.transform;
        Rigidbody = GameObject.GetComponent<Rigidbody2D>();
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
        Rigidbody.mass = statData.mass;
    }

    public void IncreaseStat(float multiple) {
        MaxHp *= Mathf.Pow(multiple, 1.6f).ClampMin(1f);
        Speed *= multiple.ClampMin(1f);
        AttackDamage *= multiple.ClampMin(1f);
        Rigidbody.mass = (Rigidbody.mass + 5f) * multiple;
        Hp = MaxHp;
    }

    public void SetMaxHp(int maxHp) {
        MaxHp = maxHp;
    }
}
