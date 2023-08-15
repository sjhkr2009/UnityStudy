using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class PlayerController : GameListenerBehaviour, IDamagableEntity {
    [Serializable]
    public class ComponentHolder {
        public AbilityController abilityController;
        public Animator animator;
        public Collider2D collider;
        public Rigidbody2D rigidbody;
        public Transform modelTransform;
        public Transform spiritTransform;
        public Transform spiritDestination;
        public Joystick joystick;
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
        Status.InputVector = components.joystick.Direction;
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

    /** PlayerInput 컴포넌트에 의해 매 프레임 자동으로 호출됩니다. */
    [Preserve]
    void OnMove(InputValue inputValue) {
        // 세팅에 의해 normalized Vector2 값이 들어온다.
        components.joystick.Input = inputValue.Get<Vector2>();
    }

    public AttackResult OnAttacked(IAttackableCollider attacker, Vector2 attackPos) {
        if (attacker == null) return AttackResult.None;
        if (isPaused) return AttackResult.None;
        if (!Status.IsHittable()) return AttackResult.None;

        var data = new DamageData(attacker, this, attackPos);
        ApplyDamage(data);

        if (Status.Hp <= 0) {
            Status.IsDead = true;
            GameBroadcaster.CallDeadPlayer();
            return AttackResult.Dead;
        }
        
        return AttackResult.Hit;
    }

    public override void OnDeadPlayer() {
        GameManager.Controller.PauseGame();
    }

    void ApplyDamage(DamageData data) {
        Status.Hit(data.damage);
        GameBroadcaster.CallHitPlayer();
    }
}
