using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public enum PlayerType { Star, Planet }

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public event Action<Player> EventPlayerDead = (P) => { };
    public event Action<int, Player> EventHpChanged = (h,P) => { };
    public event Action<int, Player> EventMaxHpChanged = (h,P) => { };

    [BoxGroup("Basic")] public PlayerType playerType;
    [BoxGroup("Basic")] [SerializeField] protected int _hp;
    [BoxGroup("Basic")] [SerializeField] protected int _maxHp;

    public int Hp
    {
        get => _hp;
        set
        {
            _hp = Mathf.Clamp(value, 0, MaxHp);
            EventHpChanged(_hp, this);
        }
    }
    public int MaxHp
    {
        get => _maxHp;
        set
        {
            _maxHp = value;
            EventMaxHpChanged(_maxHp, this);
        }
    }

    bool isPlaying = false;

    protected virtual void Start()
    {
        MaxHp = _maxHp;
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
    private void OnDisable()
    {
        EventPlayerDead(this);
    }
}
