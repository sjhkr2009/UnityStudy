using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class EnemySpawnSetting {
    // TODO: 인스펙터에서 문자열 입력하는 방식은 불편함. 스크립터블 오브젝트로 세팅을 불러오고, 에디터에서 수정 가능하게 변경 예정.
    public string prefabName;
    public float spawnCooldown;
    public int enemyType;
    public bool useCustomStat;
    
    [ShowIf("useCustomStat")]
    public EnemyStatData stat;

    private float elapsedTimeFromSpawn;

    public bool IsCooldown() {
        return elapsedTimeFromSpawn < spawnCooldown;
    }

    public void OnSpawn() {
        elapsedTimeFromSpawn = 0f;
    }

    public void Update(float deltaTime) {
        elapsedTimeFromSpawn += deltaTime;
    }
}
