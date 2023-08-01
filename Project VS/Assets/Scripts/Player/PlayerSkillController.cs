using UnityEngine;

public class PlayerSkillController {
    private PlayerStatus Status { get; }
    private Transform Transform { get; }

    private float _remainCooldown1 = 0f;
    private float _remainCooldown2 = 0f;

    private ISkillAbility skill1;
    private ISkillAbility skill2;

    public float RemainCooldown1 {
        get => _remainCooldown1;
        private set => _remainCooldown1 = value.ClampMin(0f);
    }
    public float RemainCooldown2 {
        get => _remainCooldown2;
        private set => _remainCooldown2 = value.ClampMin(0f);
    }

    public float Cooldown1 => skill1.OriginCooldown;
    public float Cooldown2 => skill2.OriginCooldown;

    public bool CanUseSkill1 => Mathf.Approximately(RemainCooldown1, 0f);
    public bool CanUseSkill2 => Mathf.Approximately(RemainCooldown2, 0f);

    public PlayerSkillController(PlayerController.ComponentHolder componentHolder, PlayerStatus status) {
        Status = status;
        Transform = componentHolder.modelTransform;
        skill1 = new FireballSkill();
        skill2 = new FireballSkill();
    }

    public void OnUpdate(float deltaTime) {
        if (Status.IsDead) return;

        ReduceSkill1Cooldown(deltaTime);
        ReduceSkill2Cooldown(deltaTime);
    }

    public void ReduceSkill1Cooldown(float reduceTime) {
        if (CanUseSkill1) return;

        RemainCooldown1 -= reduceTime;
    }
    
    public void ReduceSkill2Cooldown(float reduceTime) {
        if (CanUseSkill2) return;

        RemainCooldown2 -= reduceTime;
    }

    public void OnUseSkill1() {
        if (!CanUseSkill1) return;
        
        skill1.Run(Transform.position, Status.ShowDirection);
        RemainCooldown1 = Cooldown1;
    }
    
    public void OnUseSkill2() {
        if (!CanUseSkill2) return;
        
        skill2.Run(Transform.position, Status.ShowDirection);
        RemainCooldown2 = Cooldown2;
    }
}
