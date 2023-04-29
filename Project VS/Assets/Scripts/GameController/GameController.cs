using System.Linq;
using UnityEngine;

public class GameController {
    private GameSetting Setting { get; }
    
    public GameController(GameSetting setting) {
        Setting = setting;
        GlobalData.Controller = this;
    }

    public int Level { get; protected set; } = 1;
    public int KillCount { get; protected set; }
    public int Exp { get; protected set; }
    
    public bool IsPause { get; protected set; }
    public float GameTime { get; protected set; }

    public void Update(float deltaTime) {
        if (IsPause) return;
        
        GameTime += deltaTime;
        if (GameTime > Setting.maxGameTime) {
            // TODO: 게임 종료 처리 또는 시간 경과에 따른 이벤트
        }
    }

    public void OnDeadEnemy(EnemyStatus deadEnemy) {
        var gainExp = Setting.GetGainExp(deadEnemy);
        GainExp(gainExp);
    }

    private void GainExp(int value) {
        Exp += value;

        var requiredExp = Setting.GetRequiredExp(Level);
        if (Exp >= requiredExp) {
            Level++;
            Exp -= requiredExp;
        }
    }
}
