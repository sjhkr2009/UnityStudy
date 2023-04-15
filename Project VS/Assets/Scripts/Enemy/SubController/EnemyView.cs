using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : IEnemyView {
    private SpriteRenderer SpriteRenderer { get; }
    private EnemyStatusHandler StatusHandler { get; }
    
    public EnemyView(EnemyStatusHandler statusHandler) {
        StatusHandler = statusHandler;
        SpriteRenderer = StatusHandler.GameObject.GetOrAddComponent<SpriteRenderer>();
    }

    public void Update() {
        if (StatusHandler.CurrentDirection == Direction.Right) SpriteRenderer.flipX = true;
        else if (StatusHandler.CurrentDirection == Direction.Left) SpriteRenderer.flipX = false;
    }
}
