using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameListenerBehaviour : MonoBehaviour, IGameListener {
    protected virtual void Start() {
        GameBroadcaster.RegisterListener(this);
    }

    protected virtual void OnDestroy() {
        GameBroadcaster.RemoveListener(this);
    }

    public virtual void OnGameStart() { }
    public virtual void OnDeadEnemy(EnemyStatus deadEnemy) { }
    public virtual void OnHitPlayer() { }
    public virtual void OnHitEnemy(DamageData data, EnemyStatus hitEnemy) { }
    public virtual void OnDeadPlayer() { }
    public virtual void OnLevelUp() { }
    public virtual void OnPauseGame() { }
    public virtual void OnResumeGame() { }
    public virtual void OnGainDropItem(DropItemIndex dropItemIndex) { }
    public virtual void OnSelectItem() { }
    public virtual void OnSkill1() { }
    public virtual void OnSkill2() { }
    public virtual void OnUpdateItem(AbilityBase updatedAbility) { }
    public virtual void OnEverySecond() { }
    public virtual void OnGameEnd(GameResult gameResult) { }
    public virtual void OnContinueGame() { }
}
