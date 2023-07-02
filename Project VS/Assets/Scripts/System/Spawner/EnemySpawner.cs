using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public List<EnemySpawnSetting> spawnSettings;
    
    [SerializeField] private float globalSpawnDelay = 0.2f;
    
    const float DistanceFromBorder = 1.0f; // 카메라 바깥 테두리에서 이 거리만큼 떨어진 곳에 소환됩니다
    
    private Timer timer;
    private float lastSpawnTime;
    private HashSet<int> spawnableTypes;

    private Camera _mainCamera;
    private Camera MainCamera => _mainCamera ? _mainCamera : _mainCamera = Camera.main;

    private void Start() {
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
        if (GameManager.IsPause) return;
        
        timer.Update();
        spawnSettings.ForEach(setting => setting.Update(Time.deltaTime));

        var elapsedGameTime = timer.GetElapsedSeconds;
        var elapsedTimeFromLastSpawn = elapsedGameTime - lastSpawnTime;

        if (elapsedTimeFromLastSpawn > globalSpawnDelay) {
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
        if (setting == null) return; // 지금 스폰가능한 세팅이 없으면 생략 
        
        var enemy = PoolManager.Get<EnemyControllerBase>(setting.prefabName);
        
        if (!enemy) {
            Debugger.Error($"[EnemySpawner.Spawn] SpawnSetting prefab name '{setting.prefabName}' is Invalid!");
            return;
        }
        
        enemy.transform.position = GetRandomSpawnPos();
        if (setting.useCustomStat) {
            enemy.Status.Initialize(setting.stat);
        }
        
        setting.OnSpawn();
    }

    bool IsSpawnable(EnemySpawnSetting setting) {
        if (setting.IsCooldown()) return false;
        if (!spawnableTypes.Contains(setting.enemyType)) return false;

        return true;
    }
    
    /** 카메라 영역의 테두리에서 일정 거리 밖의 좌표를 구합니다. */
    private Vector3 GetRandomSpawnPos() {
        var cam = MainCamera;
        
        var center = cam.transform.position;
        
        float cameraHeight, cameraWidth;
        if (cam.aspect < 1f) {
            cameraHeight = cam.orthographicSize * 2f;
            cameraWidth = cameraHeight * cam.aspect;
        } else {
            cameraWidth = cam.orthographicSize * 2f;
            cameraHeight = cameraWidth * cam.aspect;
        }
        
        bool isTopOrDown = Random.value < (cameraWidth / (cameraHeight + cameraWidth));
        var distX = (cameraWidth * 0.5f) + DistanceFromBorder;
        var distY = (cameraHeight * 0.5f) + DistanceFromBorder;
        
        if (isTopOrDown) {
            distX = Random.Range(-distX, distX);
            distY = Random.value < 0.5f ? distY : -distY;
        } else {
            distX = Random.value < 0.5f ? distX : -distX;
            distY = Random.Range(-distY, distY);
        }

        return new Vector3(
            center.x + distX,
            center.y + distY
        );
    }

    EnemySpawnSetting PickSpawnSetting(IEnumerable<EnemySpawnSetting> settings) {
        // TODO: 스폰 쿨타임이나 게임 상황에 따라 적절한 세팅을 반환할 것. 게임에 따라 여러 방식이 필요하다면 이런 알고리즘을 Pick Strategy로 분리하는것도 고려.
        return settings.PickRandom();
    }
}
