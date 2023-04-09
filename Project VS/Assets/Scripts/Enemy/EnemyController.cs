using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class EnemyController : EnemyControllerBase, IRepositionTarget {
    public float speed;
    public Rigidbody2D target;

    protected override void Awake() {
        base.Awake();
        
        moveController.Speed = speed;
        if (moveController is ITargetTracker tracker) {
            if (!target) target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
            tracker.SetTarget(target);
        }
    }

    public virtual void Reposition(Transform pivotTransform) {
        if (Status.IsDead) return;
        
        var playerDir = GameManager.Instance.Player.GetStatus.InputVector;
        
        var randomVector = CustomUtility.GetRandomVector(-3f, 3f);
        var moveDelta = (playerDir * Define.EnvironmentSetting.TileMapSize) + randomVector;
        
        transform.Translate(moveDelta);
    }
}
