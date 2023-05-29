using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameController : IDisposable {
    private GameSetting Setting { get; }
    private GameBroadcaster Broadcaster { get; }
    
    public GameController(GameSetting setting) {
        Setting = setting;
        Broadcaster = new GameBroadcaster();
    }

    public int Level { get; protected set; }
    public int KillCount { get; protected set; }
    public int Exp { get; protected set; }
    public int RequiredExp { get; protected set; }

    public bool IsPause => pauseCount > 0;
    private int pauseCount = 0;

    private float prevGameTime;
    public float GameTime { get; protected set; }

    public void StartGame() {
        Level = 1;
        KillCount = 0;
        prevGameTime = 0f;
        GameTime = 0f;
        pauseCount = 0;
        RequiredExp = Setting.GetRequiredExp(Level);
    }

    public void Update(float deltaTime) {
        if (IsPause) return;

        prevGameTime = GameTime;
        GameTime += deltaTime;

        if (Mathf.FloorToInt(prevGameTime) != Mathf.FloorToInt(GameTime)) {
            Broadcaster?.CallOnEverySecond();
        }
        
        if (GameTime > Setting.maxGameTime) {
            EndGame();
        }
    }

    private void OnDeadEnemy(EnemyStatus deadEnemy) {
        KillCount++;
        var gainExp = Setting.GetGainExp(deadEnemy);
        GainExp(gainExp);
    }

    public CurrentGameStatus GetCurrentStatus() {
        return new CurrentGameStatus() {
            level = Level,
            killCount = KillCount,
            exp = Exp,
            requiredExp = RequiredExp,
            gameTime = GameTime
        };
    }

    private void GainExp(int value) {
        Exp += value;
        
        if (Exp >= RequiredExp) {
            LevelUp();
        }
    }

    private void LevelUp() {
        Exp -= RequiredExp;
        Level++;
        RequiredExp = Setting.GetRequiredExp(Level);
        
        PauseGame();
        Broadcaster.CallLevelUp();
    }

    public void PauseGame() {
        if (pauseCount == 0) Broadcaster.CallPauseGame();
        pauseCount++;
    }
    
    public void ResumeGame() {
        if (pauseCount <= 0) {
            Debugger.Error($"[GameController.ResumeGame] PauseCount already < 0 ({pauseCount})");
            return;
        }
        
        pauseCount--;
        if (pauseCount == 0) Broadcaster.CallResumeGame();
    }

    public void CallUpdateItem(ItemBase updatedItem) {
        Broadcaster.CallUpdateItem(updatedItem);
    }

    public void CallEnemyDead(EnemyStatus deadEnemy) {
        OnDeadEnemy(deadEnemy);
        Broadcaster.CallEnemyDead(deadEnemy);
    }

    public void CallSelectItem() {
        ResumeGame();
        Broadcaster.CallSelectItem();
    }
    
    public void RegisterListener(IGameListener listener) {
        Broadcaster.RegisterListener(listener);
    }

    public void RemoveListener(IGameListener listener) {
        Broadcaster.RemoveListener(listener);
    }

    public void EndGame() {
        PauseGame();
        Broadcaster?.CallEndGame();
    }

    public void Dispose() {
        Broadcaster?.Dispose();
    }
}
