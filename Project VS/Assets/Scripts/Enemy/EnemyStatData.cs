using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStatData {
    public EnemyIndex enemyIndex;
    public EnemyTier tier;
    public float attackDamage;
    public float speed;
    public float hp;
    public float mass;
    public List<DropTable> dropTables;
}
