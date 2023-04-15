using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private EmenySpawnSetting[] spawnSettings;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private float spawnDelay = 0.2f;
    
    private Timer timer;
    private float lastSpawnTime;
    

    private void Start() {
        if (spawnPoint == null || spawnPoint.Length == 0) {
            Debug.LogError($"[EnemySpawner.Start] SpawnPoint is Empty!!");
            spawnPoint = new[] { transform };
        }
        timer = Timer.StartNew();
        lastSpawnTime = 0f;
    }

    private void Update() {
        var elapsedGameTime = timer.GetElapsedSeconds;
        var elapsedTimeFromLastSpawn = elapsedGameTime - lastSpawnTime;

        while (elapsedTimeFromLastSpawn > spawnDelay) {
            elapsedTimeFromLastSpawn -= spawnDelay;
            Spawn();

            lastSpawnTime = elapsedGameTime;
        }
    }

    int GetSpawnLevel() {
        var elapsedGameTime = timer.GetElapsedSeconds;
        var level = Mathf.CeilToInt(elapsedGameTime / 10f);
        return level;
    }

    void Spawn() {
        var point = spawnPoint.PickRandom();
        var index = (GetSpawnLevel() % 2 + 1).Clamp(1, 2);
        var enemy = PoolManager.Get<EnemyControllerBase>($"Enemy{index:00}");
        enemy.transform.position = point.position;
        
        // 스폰세팅 추가되면 초기화하기
        // if(spawnSetting.useCustomStat) enemy.StatusHandler.Initialize(spawnSetting.Stat);
    }
}
