using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameListener {
    void OnGameStart();
    void OnDeadEnemy(EnemyStatus deadEnemy);
    void OnHitPlayer();
    void OnLevelUp();
    void OnPauseGame();
    void OnResumeGame();
    void OnSelectItem();
    void OnUpdateItem(ItemBase updatedItem);
    void OnEverySecond();
    void OnGameEnd();
}
