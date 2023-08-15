using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private List<EnemySpawnSetting> spawnSettings;

    const float DistanceFromBorder = 1.0f; // 카메라 바깥 테두리에서 이 거리만큼 떨어진 곳에 소환됩니다

    private void Start() {
        spawnSettings = EnemySpawnSettingContainer.Instance.spawnSettings;
    }

    private void Update() {
        if (GameManager.IsPause) return;
        
        spawnSettings.ForEach(setting => setting.Update(Time.deltaTime));

        Spawn();
    }

    void Spawn() {
        if (spawnSettings == null || spawnSettings.Count == 0) {
            Debugger.Error("[EnemySpawner.Spawn] Spawn Setting is Empty!");
            return;
        }

        foreach (var spawnSetting in spawnSettings) {
            if (!spawnSetting.CanSpawnNow()) continue;
            
            EnemySpawnManager.Spawn(spawnSetting, GetRandomSpawnPos());
        }
    }
    
    /** 카메라 영역의 테두리에서 일정 거리 밖의 좌표를 구합니다. */
    private Vector3 GetRandomSpawnPos() {
        var cam = GameManager.CurrentCamera;
        
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
