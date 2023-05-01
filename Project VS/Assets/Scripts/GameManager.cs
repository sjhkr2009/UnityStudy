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

    private GameController controller;
    private PlayerController player;

    private void Start() {
        // TODO: 유니티 이벤트로 자동 시작하는 대신, 게임 시작 신호를 받아서 시작할 수 있게 변경
        StartGame();
    }

    public void StartGame() {
        player = GlobalData.Player;
        controller = new GameController(Setting);
        controller.StartGame();
        OnGameStart?.Invoke();
        
        InvokeRepeating(nameof(CallOnEverySecond), 0f, 1f);
    }

    void CallOnEverySecond() {
        OnEverySecond?.Invoke();
    }

    private void Update() {
        controller?.Update(Time.deltaTime);
    }

    public void CallLevelUp() {
        OnLevelUp?.Invoke();
    }
    
    public void CallEnemyDead(EnemyStatus deadEnemy) {
        controller?.OnDeadEnemy(deadEnemy);
        OnDeadEnemy?.Invoke();
    }

    public void CallEndGame() {
        controller.EndGame();
        OnGameEnd?.Invoke();
    }
}
