using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EnemyControllerBase : MonoBehaviour, IPoolHandler {
    [SerializeField] private EnemyStat defaultStat = new EnemyStat();
    [ShowInInspector, ReadOnly] public EnemyStatusHandler StatusHandler { get; private set; }
    
    protected IEnemyView view;
    protected IEnemyMoveStrategy moveStrategy;
    
    protected virtual void Awake() {
        StatusHandler = new EnemyStatusHandler(gameObject);
        StatusHandler.Initialize(defaultStat);
        OnInitialize();
    }

    protected virtual void FixedUpdate() {
        moveStrategy?.Update();
    }

    protected virtual void LateUpdate() {
        view?.Update();
    }
    
    public virtual void OnInitialize() {
        StatusHandler.Initialize(defaultStat);
        view = new EnemyView(StatusHandler);
        moveStrategy = new EnemyMoveStrategy(StatusHandler);
    }

    public virtual void OnRelease() { }
}
