using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryMethod {
    public class DragonHandler : MonoBehaviour {
        [HideInInspector] public DragonImplBase dragon;
        [ShowInInspector, ReadOnly] private GameObject Target => CurrentTarget?.GetGameObject();
        [SerializeField, ReadOnly] private float attackCooldown;
        [ShowInInspector, ReadOnly] private float dragonHp => dragon?.Hp ?? -1;
        
        public IUnit CurrentTarget { get; private set; }
        
        [Button("드래곤 체력 설정")]
        void SetHp(float value = 2000f) {
            dragon.SetHp(value);
        }
        
        public void SetTarget(IUnit target) {
            CurrentTarget = target;
        }

        public void ReleaseTarget() {
            CurrentTarget = null;
        }

        private void Update() {
            if (dragon?.Hp <= 0f) {
                ReleaseTarget();
                dragon = null;
                return;
            }
            
            if (attackCooldown > 0f) {
                attackCooldown -= Time.deltaTime;
                return;
            }

            if (CurrentTarget == null || dragon == null) {
                return;
            }
            
            dragon.Attack(CurrentTarget);
            attackCooldown += (1f / dragon.AttackSpeed);
        }
    }
}
