using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameListenerBehavior : MonoBehaviour, IGameListener {
    protected virtual void Start() {
        GameBroadcaster.RegisterListener(this);
    }

    protected virtual void OnDestroy() {
        GameBroadcaster.RemoveListener(this);
    }

    public virtual void OnGameStart() { }
    public virtual void OnDeadEnemy(EnemyStatus deadEnemy) { }
    public virtual void OnHitPlayer() { }
    public virtual void OnDeadPlayer() { }
    public virtual void OnLevelUp() { }
    public virtual void OnPauseGame() { }
    public virtual void OnResumeGame() { }
    public virtual void OnSelectItem() { }
    public virtual void OnUpdateItem(ItemBase updatedItem) { }
    public virtual void OnEverySecond() { }
    public virtual void OnGameEnd() { }
}