using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameListener {
    void OnGameStart();
    void OnDeadEnemy(EnemyStatus deadEnemy);
    void OnHitPlayer();
    void OnDeadPlayer();
    void OnLevelUp();
    void OnPauseGame();
    void OnResumeGame();
    void OnGainDropItem(DropItemIndex dropItemIndex);
    void OnSelectItem();
    void OnSkill1();
    void OnSkill2();
    void OnUpdateItem(AbilityBase updatedAbility);
    void OnEverySecond();
    void OnGameEnd();
}
