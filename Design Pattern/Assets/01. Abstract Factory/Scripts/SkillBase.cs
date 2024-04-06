using UnityEngine;

namespace AbstractFactory {
    public abstract class SkillBase
    {
        public Character Character { get; private set; }

        public SkillBase SetCharacter(Character character) {
            Character = character;
            return this;
        }
        
        public abstract string Name { get; protected set; }
        public abstract int MaxLevel { get; }
        protected int currentLevel = 1;

        public virtual int CurrentLevel {
            get => currentLevel;
            set => currentLevel = Mathf.Clamp(value, 0, MaxLevel);
        }
        
        protected float cost = 0f;
        public virtual float Cost {
            get => cost;
            set => cost = Mathf.Max(value, 0f);
        }

        protected float remainCooldown = 0f;
        public virtual float RemainCoolDown {
            get => remainCooldown;
            set => remainCooldown = Mathf.Clamp(value, 0f, OriginCoolDown);
        }
    
        public abstract float OriginCoolDown { get; }

        public virtual void Update(float deltaTime) {
            RemainCoolDown -= deltaTime;
        }

        public virtual bool CanUseNow() {
            return Mathf.Approximately(RemainCoolDown, 0f);
        }
        public abstract void Activate();
    }
    
    public abstract class NormalSkillBase : SkillBase {
        public override int MaxLevel => 5;

        public override void Activate() {
            RemainCoolDown = OriginCoolDown * Character.GetAbilityReductionPercent();
            Debug.Log($"스킬 사용: {Name} --- 쿨타임 {RemainCoolDown:0.0}로 초기화됨");
        }
    }

    public abstract class QSkillHandler : NormalSkillBase {
        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            
            if (Input.GetKeyDown(KeyCode.Q)) {
                if (CanUseNow()) Activate();
                else Debug.Log($"{Name}(Q) 쿨타임이 {RemainCoolDown}초 남았습니다.");
            }
        }
    }
    public abstract class WSkillHandler : NormalSkillBase {
        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            if (Input.GetKeyDown(KeyCode.W)) {
                if (CanUseNow()) Activate();
                else Debug.Log($"{Name}(W) 쿨타임이 {RemainCoolDown}초 남았습니다.");
            }
        }
    }
    public abstract class ESkillHandler : NormalSkillBase {
        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            if (Input.GetKeyDown(KeyCode.E)) {
                if (CanUseNow()) Activate();
                else Debug.Log($"{Name}(E) 쿨타임이 {RemainCoolDown}초 남았습니다.");
            }
        }
    }
}
