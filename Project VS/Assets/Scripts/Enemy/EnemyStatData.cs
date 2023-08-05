using System;
using System.Collections.Generic;

[Serializable]
public struct EnemyStatData {
    public EnemyTier tier;
    public float attackDamage;
    public float speed;
    public float hp;
    public float mass;
    public List<DropTable> dropTables;
}
