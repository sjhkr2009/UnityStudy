using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Strategy {
    public class Player06 : MonoBehaviour, IPlayerContext {
        [ShowInInspector, ReadOnly] public float Range { get; set; } = 500;
        [ShowInInspector, ReadOnly] public float LifeSteal { get; set; } = 0;
        [SerializeField, ReadOnly] private WeaponType mainWeapon;
        [SerializeField, ReadOnly] private int bulletCount;
        [SerializeField, ReadOnly] private WeaponType subWeapon;
        [SerializeField, ReadOnly] private WeaponType nextWeapon;

        public PlayerWeaponController WeaponController { get; private set; }

        private void Awake() {
            WeaponController = new PlayerWeaponController(this);
            OnWeaponStatusStatusChange();
        }

        private void Start() {
#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = gameObject;
#endif
        }

        private void OnEnable() {
            WeaponController.onWeaponStatusChanged += OnWeaponStatusStatusChange;
        }

        private void OnDisable() {
            WeaponController.onWeaponStatusChanged -= OnWeaponStatusStatusChange;
        }

        protected virtual void Update() {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetMouseButtonDown(1)) {
                WeaponController.DoAttack(transform, null);
            }
            if (Input.GetKeyDown(KeyCode.Q)) {
                WeaponController.DoSkillQ(transform, Vector3.zero);
            }
            if (Input.GetKeyDown(KeyCode.W)) {
                WeaponController.Phase();
            }
        }

        void OnWeaponStatusStatusChange() {
            if (WeaponController == null) return;
            
            mainWeapon = WeaponController.GetMainWeapon().WeaponType;
            subWeapon = WeaponController.GetSubWeapon().WeaponType;
            bulletCount = WeaponController.GetMainWeapon().BulletCount;
            nextWeapon = WeaponController.NextWeaponType();
        }
    }
}
