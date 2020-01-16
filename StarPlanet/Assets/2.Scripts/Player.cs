using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Player : MonoBehaviour
{
    public event Action<int> EventHpChanged;

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
            //UI 조정
        }
    }
    public int MaxHp
    {
        get => _maxHp;
        set
        {
            _maxHp = value;
            //UI조정
        }
    }


    protected virtual void Start()
    {
        MaxHp = MaxHp;
        Hp = MaxHp;
    }

    public virtual void Processing()
    {

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
