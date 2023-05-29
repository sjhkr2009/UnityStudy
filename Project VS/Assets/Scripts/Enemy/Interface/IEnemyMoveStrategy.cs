using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveStrategy : IEnemyEventListener {
    void OnPauseGame();
    void OnResumeGame();
}
