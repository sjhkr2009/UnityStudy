using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : Singleton<GameManager> {
    [SerializeField] private GameSetting setting;
    
    public static event Action OnGameStart;
    public static event Action OnDeadEnemy;
    public static event Action OnHitPlayer;
    public static event Action OnLevelUp;
    public static event Action OnEverySecond;
    public static event Action OnGameEnd;

    public GameSetting Setting => setting ??= GameSetting.Load();
    
    public static GameController Controller { get; set; }
    public static PlayerController Player { get; set; }
    public static ItemController Item { get; set; }

    private void Start() {
        // TODO: 유니티 이벤트로 자동 시작하는 대신, 게임 시작 신호를 받아서 시작할 수 있게 변경
        StartGame();
    }

    public void StartGame() {
        Controller = new GameController(Setting);
        Controller.StartGame();
        OnGameStart?.Invoke();
        
        InvokeRepeating(nameof(CallOnEverySecond), 0f, 1f);
    }

    void CallOnEverySecond() {
        OnEverySecond?.Invoke();
    }

    private void Update() {
        Controller?.Update(Time.deltaTime);
    }

    public void CallLevelUp() {
        OnLevelUp?.Invoke();
    }
    
    public void CallEnemyDead(EnemyStatus deadEnemy) {
        Controller?.OnDeadEnemy(deadEnemy);
        OnDeadEnemy?.Invoke();
    }

    public void CallEndGame() {
        Controller.EndGame();
        OnGameEnd?.Invoke();
    }
}
