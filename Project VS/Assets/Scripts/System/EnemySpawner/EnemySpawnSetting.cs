using System;

[Serializable]
public class EnemySpawnSetting {
    public EnemyIndex enemyIndex;
    public float spawnStartTimeSecond;
    public float spawnEndTimeSecond = 360f;
    public float spawnCooldown;

    private float elapsedTimeFromSpawn;

    public bool CanSpawnNow() {
        var controller = GameManager.Controller;
        if (controller == null) return false;

        var gameTime = controller.PlayingTimeSecond;
        if (elapsedTimeFromSpawn < spawnCooldown) return false;
        if (gameTime < spawnStartTimeSecond || spawnEndTimeSecond < gameTime) return false;

        return true;
    }

    public void OnSpawn() {
        elapsedTimeFromSpawn = 0f;
    }

    public void Update(float deltaTime) {
        elapsedTimeFromSpawn += deltaTime;
    }
}
