using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryMethod {
    public enum DragonType {
        None,
        Infernal,
        Cloud,
        Ocean,
        Mountain,
        Elder
    }
    
    // 클라이언트 단에서 정의한 DragonType 타입에 맞게 수정한 Base Class 
    public abstract class DragonImplBase : DragonBase {
        public DragonImplBase(DragonType type, float damage, float attackSpeed) {
            DragonType = type;
            AttackDamage = damage;
            AttackSpeed = attackSpeed;
        }

        public virtual DragonType DragonType {
            get => (DragonType) Type;
            protected set => Type = (int)value;
        }
        public override int Type { get; protected set; }
        public override float AttackDamage { get; protected set; }
        public override float AttackSpeed { get; protected set; }
        public override float Hp { get; protected set; } = 2000f;
        public override float RespawnCooldown { get; protected set; } = 300f;

        public override void Attack(IUnit target) {
            float damage = AttackDamage + target.GetHp() * 0.07f;
            Debug.Log($"<color=lime>{(DragonType)} 드래곤의 공격! | 피해량: {damage} = {AttackDamage} + {target.GetHp() * 0.07f}({target.GetGameObject().name} 현재 체력의 7%)</color>");
            target.OnHit(damage);
        }

        public override void OnHit(float damage) {
            base.OnHit(damage);
            if (Hp <= 0f) {
                Hp = 0f;
                DragonSpawnTimer.OnSlainDragon(this);
            }
        }

        public void SetHp(float value) {
            Hp = value;
        }
    }

    public class InfernalDragon : DragonImplBase {
        public InfernalDragon() : base(DragonType.Infernal, 100f, 0.5f) { }
        
        public override void Attack(IUnit target) {
            base.Attack(target);
            Debug.Log($"<color=orange>화염의 드래곤: {target.GetPosition()} 지점 주변의 적에게 {AttackDamage}의 스플래시 데미지를 입힙니다.</color>");
        }
    }
    public class CloudDragon : DragonImplBase {
        public CloudDragon() : base(DragonType.Cloud, 50f, 1f) { }
    }
    public class OceanDragon : DragonImplBase {
        public OceanDragon() : base(DragonType.Ocean, 100f, 0.5f) { }
        
        public override void Attack(IUnit target) {
            base.Attack(target);
            Debug.Log($"<color=cyan>바다의 드래곤: {target.GetGameObject().name}에게 2초동안 30%의 둔화 효과를 부여합니다.</color>");
        }
    }
    public class MountainDragon : DragonImplBase {
        public MountainDragon() : base(DragonType.Mountain, 150f, 0.25f) { }
    }
    public class ElderDragon : DragonImplBase {
        public ElderDragon() : base(DragonType.Elder, 100f, 0.5f) { }

        public override float RespawnCooldown { get; protected set; } = 360f;
    }
    
}
