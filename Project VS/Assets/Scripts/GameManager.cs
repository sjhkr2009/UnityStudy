using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : SingletonMonoBehavior<GameManager> {
    [SerializeField] private GameSetting setting;

    public GameSetting Setting => setting ??= GameSetting.Load();

    public static GameController Controller { get; set; }
    public static PlayerController Player { get; set; }
    public static AbilityController Ability { get; set; }
    public static Scanner EnemyScanner { get; set; }
    public static bool IsPause => Controller == null || Controller.IsPause;

    private void Start() {
        // TODO: 유니티 이벤트로 자동 시작하는 대신, 게임 시작 신호를 받아서 시작할 수 있게 변경
        StartGame();
    }

    public void StartGame() {
        Controller = new GameController(Setting);
        Controller.StartGame();
    }

    public void QuitGame() {
        Controller = null;
    }

    private void Update() {
        Controller?.Update(Time.deltaTime);
    }
}
