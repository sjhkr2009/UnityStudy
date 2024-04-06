using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Decorator {
    public class Player07 : MonoBehaviour {
        private IPlayerModel currentPlayerData;
        
        // 인스펙터 변수 및 기능들이 플레이 중에만 보이게 하기 위한 내부 변수
        private bool isPlaying = false;
        
        [ShowInInspector, ReadOnly, ShowIf(nameof(isPlaying))] 
        private float PlayerHp => currentPlayerData?.GetStat().hp ?? 0;
        [ShowInInspector, ReadOnly, ShowIf(nameof(isPlaying))]
        private float PlayerAttackDamage => currentPlayerData?.GetStat().attackPower ?? 0;
        [ShowInInspector, ReadOnly, ShowIf(nameof(isPlaying))]
        private float PlayerDefense => currentPlayerData?.GetStat().defensePower ?? 0;
        [ShowInInspector, ReadOnly, ShowIf(nameof(isPlaying))]
        private bool CanMove => ((currentPlayerData?.GetBehaviorStatus() ?? BehaviorFlag.None) & BehaviorFlag.CanMove) > 0;
        [ShowInInspector, ReadOnly, ShowIf(nameof(isPlaying))]
        private bool CanSkill => ((currentPlayerData?.GetBehaviorStatus() ?? BehaviorFlag.None) & BehaviorFlag.CanSkill) > 0;
        [ShowInInspector, ReadOnly, ShowIf(nameof(isPlaying))]
        private bool IsInvulnerable => ((currentPlayerData?.GetBehaviorStatus() ?? BehaviorFlag.None) & BehaviorFlag.Invulnerable) > 0;

        private void Awake() {
            SetBasicStatus();
            isPlaying = true;
        }

        private void Start() {
#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = gameObject;
#endif
        }

        [Button("초기화"), ShowIf(nameof(isPlaying))]
        void SetBasicStatus() {
            currentPlayerData = new BasicPlayerModel();
        }

        [Button("존야의 모래시계 사용"), ShowIf(nameof(isPlaying))]
        void UseZhonya() {
            if (currentPlayerData is StasisDecorator) return;
            currentPlayerData = new StasisDecorator(currentPlayerData);
        }
        
        [Button("침묵에 걸림"), ShowIf(nameof(isPlaying))]
        void SetSilence() {
            if (currentPlayerData is SilenceDecorator) return;
            currentPlayerData = new SilenceDecorator(currentPlayerData);
        }
        
        [Button("공격력 30% 증가"), ShowIf(nameof(isPlaying))]
        void SetAdBuff() {
            if (currentPlayerData is DamageBuffDecorator) return;
            currentPlayerData = new DamageBuffDecorator(1.3f, currentPlayerData);
        }
        
        [Button("Hp 300 증가"), ShowIf(nameof(isPlaying))]
        void SetHpBuff() {
            if (currentPlayerData is HpBuffDecorator) return;
            currentPlayerData = new HpBuffDecorator(300f, currentPlayerData);
        }
    }
}
