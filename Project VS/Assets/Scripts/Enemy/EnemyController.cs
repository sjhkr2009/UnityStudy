using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class EnemyController : EnemyControllerBase, IRepositionTarget {
    public float speed;
    public Rigidbody2D target;

    private void Start() {
        moveController.Speed = speed;
        if (moveController is ITargetTracker tracker) {
            if (!target) target = GlobalCachedData.Player.GetComponent<Rigidbody2D>();
            tracker.SetTarget(target);
        }
    }

    public virtual void Reposition(Transform pivotTransform) {
        if (Status.IsDead) return;
        
        var playerDir = GlobalCachedData.Player.GetStatus.InputVector;
        
        var randomVector = CustomUtility.GetRandomVector(-3f, 3f);
        var moveDelta = (playerDir * Define.EnvironmentSetting.TileMapSize) + randomVector;
        
        transform.Translate(moveDelta);
    }
}
