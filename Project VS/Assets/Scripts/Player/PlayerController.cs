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
    
    public PlayerStatus Status { get; private set; }
    protected bool isPaused = false;
    
    public PlayerMoveController MoveController  { get; private set; }
    public PlayerSkillController SkillController  { get; private set; }
    public PlayerView View  { get; private set; }

    private void Awake() {
        GameManager.Player = this;
        
        Status = new PlayerStatus(gameObject, components);
        MoveController = new PlayerMoveController(components, Status);
        SkillController = new PlayerSkillController(components, Status);
        View = new PlayerView(components, Status);
    }

    private void Update() {
        SkillController?.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate() {
        if (isPaused) return;

        Status.DeltaMove = Vector2.zero;
        MoveController?.Move();
    }

    private void LateUpdate() {
        if (isPaused) return;
        
        View?.Render();
        View?.UpdateAnimator();
    }

    public override void OnUpdateItem(AbilityBase updatedAbility) {
        Status.UpdateStat();
    }

    public override void OnPauseGame() {
        isPaused = true;
    }
    
    public override void OnResumeGame() {
        isPaused = false;
    }

    public override void OnSkill1() {
        SkillController.OnUseSkill1();
    }
    
    public override void OnSkill2() {
        SkillController.OnUseSkill2();
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
        Status.InputVector = inputValue.Get<Vector2>();
    }
}
