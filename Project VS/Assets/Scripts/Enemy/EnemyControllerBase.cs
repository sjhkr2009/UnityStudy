using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EnemyControllerBase : GameListenerBehavior, IPoolHandler {
    [SerializeField] private EnemyStatData defaultStat = new EnemyStatData();
    [ShowInInspector, ReadOnly] public EnemyStatus Status { get; private set; }
    
    protected bool isPaused = false;
    
    protected IEnemyView view;
    protected IEnemyMoveStrategy moveStrategy;

    protected virtual void Awake() {
        Status = new EnemyStatus(gameObject);
        OnInitialize();
    }

    protected virtual void FixedUpdate() {
        if (isPaused) return;
        
        moveStrategy?.Update();
    }

    protected virtual void LateUpdate() {
        if (isPaused) return;
        
        view?.Update();
    }

    public override void OnPauseGame() {
        isPaused = true;
    }
    
    public override void OnResumeGame() {
        isPaused = false;
    }
    
    public virtual void OnInitialize() {
        isPaused = GameManager.IsPause;
        Status.Initialize(defaultStat);
    }

    public virtual void OnRelease() { }
}
