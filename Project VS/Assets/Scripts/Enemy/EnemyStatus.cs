using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus {
    public GameObject GameObject { get; }

    public Direction CurrentDirection { get; set; } = Direction.None;
    public bool IsDead { get; set; } = false;

    public EnemyStatus(GameObject enemyObject) {
        GameObject = enemyObject;
    }
}
