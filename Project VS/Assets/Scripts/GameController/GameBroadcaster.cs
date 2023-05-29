using System;
using System.Collections.Generic;

public class GameBroadcaster : IDisposable {
    private LinkedList<IGameListener> Listeners { get; } = new LinkedList<IGameListener>();
    
    public void RegisterListener(IGameListener listener) {
        if (Listeners.Contains(listener)) {
            Debugger.Error($"[GameBroadcaster.RegisterListener] {listener.GetType().Name} already registered!!");
            return;
        }

        Listeners.AddLast(listener);
    }

    public void RemoveListener(IGameListener listener) {
        if (!Listeners.Contains(listener)) {
            Debugger.Warning($"[GameBroadcaster.RemoveListener] {listener.GetType().Name} not registered!");
            return;
        }
        
        Listeners.Remove(listener);
    }
    
    public void CallOnEverySecond() {
        Listeners.ForEach(l => l.OnEverySecond());
    }

    public void CallLevelUp() {
        Listeners.ForEach(l => l.OnLevelUp());
    }
    
    public void CallSelectItem() {
        Listeners.ForEach(l => l.OnSelectItem());
    }

    public void CallUpdateItem(ItemBase updatedItem) {
        Listeners.ForEach(l => l.OnUpdateItem(updatedItem));
    }

    public void CallPauseGame() {
        Listeners.ForEach(l => l.OnPauseGame());
    }

    public void CallResumeGame() {
        Listeners.ForEach(l => l.OnResumeGame());
    }
    
    public void CallEnemyDead(EnemyStatus deadEnemy) {
        Listeners.ForEach(l => l.OnDeadEnemy(deadEnemy));
    }

    public void CallEndGame() {
        Listeners.ForEach(l => l.OnGameEnd());
    }

    public void Dispose() {
        Listeners.Clear();
    }
}
