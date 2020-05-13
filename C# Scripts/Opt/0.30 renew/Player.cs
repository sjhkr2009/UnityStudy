using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{
    [SerializeField] [BoxGroup("Status")] private float moveSpeed;
    [SerializeField] [BoxGroup("Status")] private int maxHp;
    [SerializeField] [BoxGroup("Status")] [ReadOnly] private int _hp;
    public int Hp
    {
        get => _hp;
        set
        {
            _hp = Mathf.Clamp(value, 0, maxHp);
            EventOnHpChanged(_hp);
        }
    }
    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = value;
            EventOnMaxHpChanged(maxHp);
        }
    }

    public event Action<int> EventOnHpChanged = (int n) => { };
    public event Action<int> EventOnMaxHpChanged = (int n) => { };

    private void Start()
    {
        MaxHp = maxHp;
        Hp = MaxHp;
    }

    public void PlayerMove(Vector2 velocity)
    {
        transform.Translate(velocity * moveSpeed * Time.deltaTime);
    }

    [Button]
    public void TestDamage(int damage = 1)
    {
        Hp -= damage;
    }
}
