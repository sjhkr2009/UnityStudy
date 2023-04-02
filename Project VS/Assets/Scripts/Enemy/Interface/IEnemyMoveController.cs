using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveController {
    float Speed { get; set; }
    void Update(EnemyStatus status);
}
