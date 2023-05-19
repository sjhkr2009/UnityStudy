using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameController {
    private GameSetting Setting { get; }
    
    public GameController(GameSetting setting) {
        Setting = setting;
        GameManager.Controller = this;
    }

    public int Level { get; protected set; }
    public int KillCount { get; protected set; }
    public int Exp { get; protected set; }
    public int RequiredExp { get; protected set; }

    public bool IsPause => pauseCount > 0;
    private int pauseCount = 0;
    
    public float GameTime { get; protected set; }

    public void StartGame() {
        Level = 1;
        KillCount = 0;
        GameTime = 0f;
        pauseCount = 0;
        RequiredExp = Setting.GetRequiredExp(Level);
         
        GameManager.OnLevelUp += PauseGame;
        GameManager.OnSelectItem += ResumeGame;
    }
    
    public void Update(float deltaTime) {
        if (IsPause) return;
        
        GameTime += deltaTime;
        if (GameTime > Setting.maxGameTime) {
            GameManager.Instance.CallEndGame();
        }
    }

    public void OnDeadEnemy(EnemyStatus deadEnemy) {
        KillCount++;
        var gainExp = Setting.GetGainExp(deadEnemy);
        GainExp(gainExp);
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
        
        GameManager.Instance.CallLevelUp();
    }

    public void PauseGame() {
        if (pauseCount == 0) GameManager.Instance.CallPauseGame();
        pauseCount++;
    }
    
    public void ResumeGame() {
        if (pauseCount <= 0) {
            Debugger.Error($"[GameController.ResumeGame] PauseCount already < 0 ({pauseCount})");
            return;
        }
        
        pauseCount--;
        if (pauseCount == 0) GameManager.Instance.CallResumeGame();
    }

    public void EndGame() {
        PauseGame();
        
        GameManager.OnLevelUp -= PauseGame;
        GameManager.OnSelectItem -= ResumeGame;
    }
}
