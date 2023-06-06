using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class PlayerController : GameListenerBehavior {
    [Serializable]
    public class ComponentHolder {
        public ItemController itemController;
        public Animator animator;
        public Collider2D collider;
        public Rigidbody2D rigidbody;
        public Transform modelTransform;
    }
    
    [SerializeField] private ComponentHolder components;

    private PlayerStatus _playerStatus;
    public PlayerStatus Status => _playerStatus;
    protected bool isPaused = false;
    
    private PlayerMoveController moveController;
    private PlayerView viewController;

    private void Awake() {
        GameManager.Player = this;

        var go = gameObject;
        _playerStatus = new PlayerStatus(gameObject, components);
        moveController = new PlayerMoveController(components, Status);
        viewController = new PlayerView(components, Status);
    }

    private void FixedUpdate() {
        if (isPaused) return;

        _playerStatus.DeltaMove = Vector2.zero;
        moveController?.Move();
    }

    private void LateUpdate() {
        if (isPaused) return;
        
        viewController?.Render();
        viewController?.UpdateAnimator();
    }

    public override void OnUpdateItem(ItemBase updatedItem) {
        Status?.UpdateStat();
    }

    public override void OnPauseGame() {
        isPaused = true;
    }
    
    public override void OnResumeGame() {
        isPaused = false;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (isPaused) return;
        if (!Status.IsHittable()) return;
        
        var weapon = other.collider.GetComponent<IAttackableCollider>();
        if (weapon == null || !weapon.IsValidTarget(gameObject)) return;
        
        // TODO: 데미지 구조체를 OnHit에 전달할 것
        Status.Hit(weapon.Damage);
        GameBroadcaster.CallHitPlayer();

        if (Status.Hp <= 0) {
            Status.IsDead = true;
            GameBroadcaster.CallDeadPlayer();
        }
    }

    /** PlayerInput 컴포넌트에 의해 매 프레임 자동으로 호출됩니다. */
    [Preserve]
    void OnMove(InputValue inputValue) {
        // 세팅에 의해 normalized Vector2 값이 들어온다.
        _playerStatus.InputVector = inputValue.Get<Vector2>();
    }
}
