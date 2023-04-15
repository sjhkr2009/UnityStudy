using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public List<EnemySpawnSetting> spawnSettings;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private float globalSpawnDelay = 0.2f;
    
    private Timer timer;
    private float lastSpawnTime;
    private HashSet<int> spawnableTypes;

    private void Start() {
        if (spawnPoint == null || spawnPoint.Length == 0) {
            Debugger.Error($"[EnemySpawner.Start] SpawnPoint is Empty!!");
            spawnPoint = new[] { transform };
        }
        timer = Timer.StartNew();
        lastSpawnTime = 0f;

        InitSpawnLevels();
    }

    void InitSpawnLevels() {
        // TODO: 게임 시작 시마다 소환해야하는 레벨이 정해지면 수정. 지금은 모든 타입을 스폰 가능한걸로 간주한다.
        spawnableTypes = new HashSet<int>();
        foreach (var setting in spawnSettings) {
            spawnableTypes.Add(setting.enemyType);
        }
    }

    private void Update() {
        timer.Update();
        spawnSettings.ForEach(setting => setting.Update(Time.deltaTime));

        var elapsedGameTime = timer.GetElapsedSeconds;
        var elapsedTimeFromLastSpawn = elapsedGameTime - lastSpawnTime;

        if (elapsedTimeFromLastSpawn > globalSpawnDelay) {
            elapsedTimeFromLastSpawn = 0f;
            //elapsedTimeFromLastSpawn -= globalSpawnDelay;
            Spawn();

            lastSpawnTime = elapsedGameTime;
        }
    }

    void Spawn() {
        if (spawnSettings == null || spawnSettings.Count == 0) {
            Debugger.Error("[EnemySpawner.Spawn] Spawn Setting is Empty!");
            return;
        }
        
        var setting = PickSpawnSetting(spawnSettings.Where(IsSpawnable));
        if (setting == null) return;
        
        var enemy = PoolManager.Get<EnemyControllerBase>(setting.prefabName);
        
        if (!enemy) {
            Debugger.Error($"[EnemySpawner.Spawn] SpawnSetting prefab name '{setting.prefabName}' is Invalid!");
            return;
        }
        
        enemy.transform.position = spawnPoint.PickRandom().position;
        if (setting.useCustomStat) enemy.StatusHandler.Initialize(setting.stat);
        
        setting.OnSpawn();
    }

    bool IsSpawnable(EnemySpawnSetting setting) {
        if (setting.IsCooldown()) return false;
        if (!spawnableTypes.Contains(setting.enemyType)) return false;

        return true;
    }

    EnemySpawnSetting PickSpawnSetting(IEnumerable<EnemySpawnSetting> settings) {
        // TODO: 스폰 쿨타임이나 게임 상황에 따라 적절한 세팅을 반환할 것. 게임에 따라 여러 방식이 필요하다면 이런 알고리즘을 Pick Strategy로 분리하는것도 고려.
        return settings.PickRandom();
    }
}
