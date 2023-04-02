using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EnemyControllerBase : MonoBehaviour {
    [ShowInInspector, ReadOnly] public EnemyStatus Status { get; private set; }
    
    protected IEnemyView viewController;
    protected IEnemyMoveController moveController;
    
    protected virtual void Awake() {
        Status = new EnemyStatus(gameObject);
        
        viewController = new EnemyView(gameObject);
        moveController = new EnemyMoveController(gameObject);
    }

    protected virtual void FixedUpdate() {
        moveController?.Update(Status);
    }

    protected virtual void LateUpdate() {
        viewController?.Update(Status);
    }
}
