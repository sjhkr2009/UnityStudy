using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{
    [BoxGroup("Basic")] [SerializeField] private int _hp;
    [BoxGroup("Basic")] [SerializeField] private int _maxHp;


    public int Hp
    {
        get => _hp;
        set
        {
            _hp = Mathf.Clamp(value, 0, MaxHp);
            if (_hp == 0) GameManager.Instance.gameState = GameManager.GameState.GameOver;
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
}
