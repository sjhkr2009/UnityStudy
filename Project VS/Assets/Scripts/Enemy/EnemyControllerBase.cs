using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EnemyControllerBase : MonoBehaviour, IPoolHandler {
    [SerializeField] private EnemyStatData defaultStat = new EnemyStatData();
    [ShowInInspector, ReadOnly] public EnemyStatus Status { get; private set; }
    
    protected IEnemyView view;
    protected IEnemyMoveStrategy moveStrategy;
    
    protected virtual void Awake() {
        Status = new EnemyStatus(gameObject);
        OnInitialize();
    }

    protected virtual void FixedUpdate() {
        moveStrategy?.Update();
    }

    protected virtual void LateUpdate() {
        view?.Update();
    }
    
    public virtual void OnInitialize() {
        Status.Initialize(defaultStat);
    }

    public virtual void OnRelease() { }
}
