using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Player : MonoBehaviour
{
    public event Action<int> EventHpChanged = n => { };
    public event Action<int> EventMaxHpChanged = n => { };

    [BoxGroup("Basic")] [SerializeField] private int _hp;
    [BoxGroup("Basic")] [SerializeField] private int _maxHp;

    bool isPlaying = false;


    public int Hp
    {
        get => _hp;
        set
        {
            _hp = Mathf.Clamp(value, 0, MaxHp);
            EventHpChanged(_hp);
        }
    }
    public int MaxHp
    {
        get => _maxHp;
        set
        {
            _maxHp = value;
            EventMaxHpChanged(_maxHp);
        }
    }


    protected virtual void Start()
    {
        MaxHp = MaxHp;
        Hp = MaxHp;
    }

    public virtual void Processing()
    {
        //상속받을 Star, Planet에서 각각 움직임을 추가
    }

    private void FixedUpdate()
    {
        if(isPlaying)
        {
            Processing();   
        }
    }

    public void OnGameStateChanged(GameState gameState)
    {
        isPlaying = gameState == GameState.Playing;
    }
}
