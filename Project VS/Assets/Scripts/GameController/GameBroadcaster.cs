using System;
using System.Collections.Generic;

public static class GameBroadcaster {
    private static LinkedList<IGameListener> Listeners { get; } = new LinkedList<IGameListener>();

    public static void ClearAll() {
        Listeners.ForEach(UnregisterEvents);
        Listeners.Clear();
        
    }

    public static void RegisterListener(IGameListener listener) {
        if (Listeners.Contains(listener)) {
            Debugger.Error($"[GameBroadcaster.RegisterListener] {listener.GetType().Name} already registered!!");
            return;
        }

        RegisterEvents(listener);
        Listeners.AddLast(listener);
    }

    private static void RegisterEvents(IGameListener listener) {
        OnGameStart += listener.OnGameStart;
        OnEverySecond += listener.OnEverySecond;
        OnLevelUp += listener.OnLevelUp;
        OnSelectItem += listener.OnSelectItem;
        OnGainDropItem += listener.OnGainDropItem;
        OnUpdateItem += listener.OnUpdateItem;
        OnPauseGame += listener.OnPauseGame;
        OnResumeGame += listener.OnResumeGame;
        OnHitPlayer += listener.OnHitPlayer;
        OnHitEnemy += listener.OnHitEnemy;
        OnDeadPlayer += listener.OnDeadPlayer;
        OnDeadEnemy += listener.OnDeadEnemy;
        OnUseSkill1 += listener.OnSkill1;
        OnUseSkill2 += listener.OnSkill2;
        OnEndGame += listener.OnGameEnd;
    }

    public static void RemoveListener(IGameListener listener) {
        if (!Listeners.Contains(listener)) {
            Debugger.Warning($"[GameBroadcaster.RemoveListener] {listener.GetType().Name} not registered!");
            return;
        }

        UnregisterEvents(listener);
        Listeners.Remove(listener);
    }
    
    private static void UnregisterEvents(IGameListener listener) {
        OnGameStart -= listener.OnGameStart;
        OnEverySecond -= listener.OnEverySecond;
        OnLevelUp -= listener.OnLevelUp;
        OnSelectItem -= listener.OnSelectItem;
        OnGainDropItem -= listener.OnGainDropItem;
        OnUpdateItem -= listener.OnUpdateItem;
        OnPauseGame -= listener.OnPauseGame;
        OnResumeGame -= listener.OnResumeGame;
        OnHitPlayer -= listener.OnHitPlayer;
        OnHitEnemy -= listener.OnHitEnemy;
        OnDeadPlayer -= listener.OnDeadPlayer;
        OnDeadEnemy -= listener.OnDeadEnemy;
        OnUseSkill1 -= listener.OnSkill1;
        OnUseSkill2 -= listener.OnSkill2;
        OnContinueGame -= listener.OnContinueGame;
        OnEndGame -= listener.OnGameEnd;
    }

    public static void UnregisterAll() {
        Listeners.ForEach(UnregisterEvents);
        Listeners.Clear();
        
        OnGameStart = null;
        OnEverySecond = null;
        OnLevelUp = null;
        OnSelectItem = null;
        OnGainDropItem = null;
        OnUpdateItem = null;
        OnPauseGame = null;
        OnResumeGame = null;
        OnHitPlayer = null;
        OnHitEnemy = null;
        OnDeadPlayer = null;
        OnDeadEnemy = null;
        OnUseSkill1 = null;
        OnUseSkill2 = null;
        OnContinueGame = null;
        OnEndGame = null;
    }

    public static event Action OnGameStart;
    public static event Action OnEverySecond;
    public static event Action OnLevelUp;
    public static event Action OnSelectItem;
    public static event Action<DropItemIndex> OnGainDropItem;
    public static event Action<AbilityBase> OnUpdateItem;
    public static event Action OnPauseGame;
    public static event Action OnResumeGame;
    public static event Action OnHitPlayer;
    public static event Action OnDeadPlayer;
    public static event Action<DamageData, EnemyStatus> OnHitEnemy;
    public static event Action<EnemyStatus> OnDeadEnemy;
    public static event Action OnUseSkill1;
    public static event Action OnUseSkill2;
    public static event Action OnContinueGame;
    public static event Action<GameResult> OnEndGame;

    public static void CallOnGameStart() => OnGameStart?.Invoke();
    public static void CallOnEverySecond() => OnEverySecond?.Invoke();
    public static void CallLevelUp() => OnLevelUp?.Invoke();
    public static void CallSelectItem() => OnSelectItem?.Invoke();
    public static void CallGainDropItem(DropItemIndex dropItemIndex) => OnGainDropItem?.Invoke(dropItemIndex);
    public static void CallUpdateItem(AbilityBase updatedAbility) => OnUpdateItem?.Invoke(updatedAbility);
    public static void CallPauseGame() => OnPauseGame?.Invoke();
    public static void CallResumeGame() => OnResumeGame?.Invoke();
    public static void CallHitPlayer() => OnHitPlayer?.Invoke();
    public static void CallDeadPlayer() => OnDeadPlayer?.Invoke();
    public static void CallEnemyDead(EnemyStatus deadEnemy) => OnDeadEnemy?.Invoke(deadEnemy);
    public static void CallEnemyHit(DamageData data, EnemyStatus hitEnemy) => OnHitEnemy?.Invoke(data, hitEnemy);
    public static void CallSkill1() => OnUseSkill1?.Invoke();
    public static void CallSkill2() => OnUseSkill2?.Invoke();
    public static void CallContinueGame() => OnContinueGame?.Invoke();
    public static void CallEndGame(GameResult gameResult) => OnEndGame?.Invoke(gameResult);
}
