using System;
using UnityEngine;

[Serializable]
public class EmenySpawnSetting {
    public GameObject prefab;
    public bool useCustomStat;
    public EnemyStat stat;
    public float spawnTime;
}
