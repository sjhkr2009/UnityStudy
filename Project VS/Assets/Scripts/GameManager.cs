using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : Singleton<GameManager> {
    [SerializeField] private GameSetting setting;

    public GameSetting Setting => setting ??= GameSetting.Load();

    private GameController controller;

    private void Start() {
        // TODO: 유니티 이벤트로 자동 시작하는 대신, 게임 시작 신호를 받아서 시작할 수 있게 변경
        StartGame();
    }

    public void StartGame() {
        controller = new GameController(Setting);
    }

    public void OnDeadEnemy(EnemyStatus deadEnemy) {
        controller?.OnDeadEnemy(deadEnemy);
    }
}
