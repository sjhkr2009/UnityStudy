using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : IEnemyView {
    private SpriteRenderer SpriteRenderer { get; }
    
    public EnemyView(GameObject target) {
        SpriteRenderer = target.GetOrAddComponent<SpriteRenderer>();
    }

    public void Update(EnemyStatus status) {
        if (status.CurrentDirection == Direction.Right) SpriteRenderer.flipX = true;
        else if (status.CurrentDirection == Direction.Left) SpriteRenderer.flipX = false;
    }
}
