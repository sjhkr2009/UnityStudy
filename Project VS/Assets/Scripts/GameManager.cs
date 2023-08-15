using System;
using DG.Tweening;
using UnityEngine;

public sealed class GameManager : SingletonMonoBehavior<GameManager> {
    [SerializeField] private GameSetting setting;

    public GameSetting Setting => setting ??= GameSetting.Load();
    private Camera _camera;

    public static GameController Controller { get; set; }
    public static PlayerController Player { get; set; }
    public static AbilityController Ability { get; set; }
    public static Scanner EnemyScanner { get; set; }
    public static Camera CurrentCamera => Instance._camera ? Instance._camera : Instance._camera = Camera.main; 
    public static bool IsPause => Controller == null || Controller.IsPause;

    public void StartGame() {
        Controller?.Dispose();
        
        Controller = new GameController(Setting);
        CardManager.OnStartGame();
        Controller.StartGame();
    }

    public void ClearController() {
        Controller?.Dispose();
        Controller = null;
    }

    public void ContinueGame() {
        Controller.ResumeGame();
        Player.Status.HealHp(Player.Status.MaxHp);
        Player.Status.IsDead = false;
    }

    public void ClearAll() {
        EnemySpawnManager.Clear();
        DropItemManager.Clear();
        PoolManager.AbandonAll();
        GameBroadcaster.UnregisterAll();
        DOTween.timeScale = 1f;
        Ability.Dispose();
    }

    public int GetScore() {
        if (Controller == null) return 0;
        
        var score = Controller.PlayingTimeSecond * 2f;
        if (Controller.PlayingTimeSecond > 60f) score += (Controller.PlayingTimeSecond - 60f) * 2f;
        if (Controller.PlayingTimeSecond > 120f) score += (Controller.PlayingTimeSecond - 120f) * 3f;
        if (Controller.PlayingTimeSecond > 240f) score += (Controller.PlayingTimeSecond - 240f) * 3f;
        if (Controller.PlayingTimeSecond > 360f) score += (Controller.PlayingTimeSecond - 360f) * 10f;
        return Mathf.RoundToInt(score);
    }

    private void Update() {
        Controller?.Update(Time.deltaTime);
    }
}
