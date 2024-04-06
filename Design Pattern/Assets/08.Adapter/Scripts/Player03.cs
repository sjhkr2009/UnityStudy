using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Adapter {
    public class Player03 : MonoBehaviour {
        private CainSkillAdapter adapter;

        [SerializeField, HideIf("isPlaying")] private float attackDamage = 100;
        [ShowInInspector, ShowIf("isPlaying")]
        private float AttackDamage {
            get => weapon?.AttackPower ?? attackDamage;
            set {
                if (weapon != null) weapon.AttackPower = value;
            }
        }

        private IWeapon weapon;

        public enum WeaponType {
            Bow,
            Sword,
            Claw,
            Cain
        }

        private WeaponType type = WeaponType.Bow;

        [ShowInInspector] private WeaponType weaponType {
            get => type;
            set {
                type = value;
                weapon = type switch {
                    WeaponType.Bow => new Bow {AttackPower = AttackDamage},
                    WeaponType.Sword => new OneHandedSword {AttackPower = AttackDamage},
                    WeaponType.Claw => new Claw {AttackPower = AttackDamage},
                    WeaponType.Cain => new Cain {AttackPower = AttackDamage},
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private void Start() {
#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = gameObject;
#endif
            if (!adapter) adapter = GetComponent<CainSkillAdapter>();
            weaponType = type;
        }

        [Button("쿼드러플 스로우")]
        void DoQuadrupleThrow() {
            UseSkill(new QuadrupleThrow());
        }

        [Button("레이징 블로우")]
        void DoRaisingBlow() {
            UseSkill(new RaisingBlow());
        }
        
        [Button("언카운터블 애로우")]
        void DoUncountableArrow() {
            UseSkill(new UncountableArrow());
        }

        void UseSkill<T>(ISkillHandler<T> handler) where T : IWeapon, new() {
            if (weapon is Cain cain) {
                adapter.ActivateByCain(handler, cain);
                return;
            }
            if (!(weapon is T wp)) {
                Debug.LogError($"<color=yellow>적합한 무기를 장착해야 사용할 수 있습니다.</color>");
                return;
            }
            handler.Activate(wp);
        }
    }
}
