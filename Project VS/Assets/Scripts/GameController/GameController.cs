using System.Runtime.Versioning;
using DG.Tweening;
using UnityEngine;

public enum GameResult {
    None,
    Success,
    Die
}

public class GameController {
    private GameSetting Setting { get; }
    
    public GameController(GameSetting setting) {
        Setting = setting;
    }

    public int Level { get; protected set; }
    public int KillCount { get; protected set; }
    public int Exp { get; protected set; }
    public int RequiredExp { get; protected set; }

    public bool IsPause => pauseCount > 0;
    private int pauseCount = 0;

    private float prevGameTime;
    public float PlayingTimeSecond { get; protected set; }

    public void StartGame() {
        Level = 1;
        KillCount = 0;
        prevGameTime = 0f;
        PlayingTimeSecond = 0f;
        pauseCount = 0;
        RequiredExp = Setting.GetRequiredExp(Level);
        
        GameBroadcaster.CallOnGameStart();
    }

    public void Update(float deltaTime) {
        if (IsPause) return;

        prevGameTime = PlayingTimeSecond;
        PlayingTimeSecond += deltaTime;

        if (Mathf.FloorToInt(prevGameTime) != Mathf.FloorToInt(PlayingTimeSecond)) {
            GameBroadcaster.CallOnEverySecond();
        }
        
        if (PlayingTimeSecond > Setting.maxGameTime) {
            EndGame(GameResult.Success);
        }
    }

    private void OnDeadEnemy(EnemyStatus deadEnemy) {
        KillCount++;
        DropItemManager.SpawnByDropTable(deadEnemy.DropTables, deadEnemy.Transform.position);
    }

    public CurrentGameStatus GetCurrentStatus() {
        return new CurrentGameStatus() {
            level = Level,
            killCount = KillCount,
            exp = Exp,
            requiredExp = RequiredExp,
            gameTime = PlayingTimeSecond
        };
    }

    public void GainExp(int value) {
        Exp += value;

        while (Exp >= RequiredExp) {
            LevelUp();
        }
    }

    private void LevelUp() {
        Exp -= RequiredExp;
        Level++;
        RequiredExp = Setting.GetRequiredExp(Level);
        
        PauseGame();
        GameBroadcaster.CallLevelUp();
    }

    public void PauseGame() {
        if (pauseCount == 0) {
            GameBroadcaster.CallPauseGame();
            DOTween.timeScale = 0f;
        }
        pauseCount++;
    }
    
    public void ResumeGame() {
        if (pauseCount <= 0) {
            Debugger.Error($"[GameController.ResumeGame] PauseCount already < 0 ({pauseCount})");
            return;
        }
        
        pauseCount--;
        if (pauseCount == 0) {
            GameBroadcaster.CallResumeGame();
            DOTween.timeScale = 1f;
        }
    }

    public void CallEnemyDead(EnemyStatus deadEnemy) {
        OnDeadEnemy(deadEnemy);
        GameBroadcaster.CallEnemyDead(deadEnemy);
    }

    public void CallSelectItem() {
        ResumeGame();
        GameBroadcaster.CallSelectItem();
    }

    public void EndGame(GameResult result) {
        PauseGame();
        GameBroadcaster.CallEndGame(result);
    }

    public void Dispose() {
        Level = 1;
        KillCount = 0;
        prevGameTime = 0f;
        PlayingTimeSecond = 0f;
        pauseCount = 0;
        RequiredExp = Setting.GetRequiredExp(Level);
    }
}
