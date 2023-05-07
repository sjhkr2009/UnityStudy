using System.Linq;
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
    
    public bool IsPause { get; protected set; }
    public float GameTime { get; protected set; }

    public void StartGame() {
        Level = 1;
        KillCount = 0;
        GameTime = 0f;
        IsPause = false;
        RequiredExp = Setting.GetRequiredExp(Level);
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

    public void EndGame() {
        IsPause = true;
    }
}
