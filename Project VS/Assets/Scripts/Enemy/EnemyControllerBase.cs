using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EnemyControllerBase : GameListenerBehaviour, IPoolHandler {
    [ShowInInspector, ReadOnly] public EnemyStatus Status { get; private set; }
    [SerializeField] protected EnemyMoveComponentBase moveComponent;

    protected IEnemyView view;
    protected IEnemyMoveStrategy moveStrategy;

    protected virtual void Awake() {
        Status = new EnemyStatus(gameObject);
        OnInitialize();
    }

    protected virtual void FixedUpdate() {
        if (GameManager.IsPause) return;
        
        moveStrategy?.OnUpdate(Time.fixedDeltaTime);
    }

    protected virtual void LateUpdate() {
        if (GameManager.IsPause) return;
        
        view?.OnUpdate(Time.deltaTime);
    }

    public override void OnPauseGame() {
    }
    
    public override void OnResumeGame() {
    }
    
    public virtual void OnInitialize() {
        EnemySpawnManager.SpawnedEnemies.Add(this);
    }

    public virtual void OnRelease() {
        EnemySpawnManager.SpawnedEnemies.Remove(this);
    }
}
