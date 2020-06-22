using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseUnit : MonoBehaviour
{
    public event Action<float> EventOnHpChanged = n => { };
    public event Action<float> EventOnMaxHpChanged = n => { };

    [SerializeField] [BoxGroup("Status")] protected float moveSpeed;
    [SerializeField] [BoxGroup("Status")] protected float maxHp;
    [SerializeField] [BoxGroup("Status")] [ReadOnly] protected float _hp;
    public float Hp
    {
        get => _hp;
        set
        {
            _hp = Mathf.Clamp(value, 0f, maxHp);
            EventOnHpChanged(_hp);
        }
    }
    public float MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = value;
            EventOnMaxHpChanged(maxHp);
        }
    }

    private void Start()
    {
        MaxHp = maxHp;
        Hp = MaxHp;
    }

    public void GetDamage(float damage)
    {
        Hp -= damage;
    }
}
