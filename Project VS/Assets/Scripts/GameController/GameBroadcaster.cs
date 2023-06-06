using System;
using System.Collections.Generic;

public static class GameBroadcaster {
    private static LinkedList<IGameListener> Listeners { get; } = new LinkedList<IGameListener>();
    
    public static void RegisterListener(IGameListener listener) {
        if (Listeners.Contains(listener)) {
            Debugger.Error($"[GameBroadcaster.RegisterListener] {listener.GetType().Name} already registered!!");
            return;
        }

        Listeners.AddLast(listener);
    }

    public static void RemoveListener(IGameListener listener) {
        if (!Listeners.Contains(listener)) {
            Debugger.Warning($"[GameBroadcaster.RemoveListener] {listener.GetType().Name} not registered!");
            return;
        }
        
        Listeners.Remove(listener);
    }
    
    public static void CallOnEverySecond() {
        Listeners.ForEach(l => l.OnEverySecond());
    }

    public static void CallLevelUp() {
        Listeners.ForEach(l => l.OnLevelUp());
    }
    
    public static void CallSelectItem() {
        Listeners.ForEach(l => l.OnSelectItem());
    }

    public static void CallUpdateItem(ItemBase updatedItem) {
        Listeners.ForEach(l => l.OnUpdateItem(updatedItem));
    }

    public static void CallPauseGame() {
        Listeners.ForEach(l => l.OnPauseGame());
    }

    public static void CallResumeGame() {
        Listeners.ForEach(l => l.OnResumeGame());
    }
    
    public static void CallHitPlayer() {
        Listeners.ForEach(l => l.OnHitPlayer());
    }
    
    public static void CallDeadPlayer() {
        Listeners.ForEach(l => l.OnDeadPlayer());
    }
    
    public static void CallEnemyDead(EnemyStatus deadEnemy) {
        Listeners.ForEach(l => l.OnDeadEnemy(deadEnemy));
    }

    public static void CallEndGame() {
        Listeners.ForEach(l => l.OnGameEnd());
    }

    public static void Dispose() {
        Listeners.Clear();
    }
}
