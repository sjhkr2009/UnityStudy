using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
    public class PlayerWeaponController {
        private const int MaxBulletCount = 50;
        private const int UseBulletCountOnSkill = 10;
        
        public class WeaponInfo {
            public WeaponType WeaponType { get; }
            public IWeaponStrategy Strategy { get; }
            public int BulletCount { get; private set; }
            private Action onBulletOver;

            public void UseBullet(int count) {
                BulletCount = Mathf.Clamp(BulletCount - count, 0, MaxBulletCount);
                if (BulletCount <= 0) onBulletOver?.Invoke();
                Debug.Log($"[{WeaponType}] 총알 {count}개 소비 (남은 총알: {BulletCount}개)");
            }

            public WeaponInfo(WeaponType type, Action bulletOverCallback = null) {
                WeaponType = type;
                Strategy = CreateStrategy(type);
                BulletCount = MaxBulletCount;
                onBulletOver = bulletOverCallback;
            }
            
            static IWeaponStrategy CreateStrategy(WeaponType weaponType) {
                return weaponType switch {
                    WeaponType.Calibrum => new CalibrumStrategy(),
                    WeaponType.Severum => new SeverumStrategy(),
                    WeaponType.Gravitum => new GravitumStrategy(),
                    WeaponType.Infernum => new InfernumStrategy(),
                    WeaponType.Crescendum => new CrescendumStrategy(),
                    _ => throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null)
                };
            }
        }

        public PlayerWeaponController(IPlayerContext player) {
            Player = player;
            SetMainWeapon(CreateWeaponInfo());
            SetSubWeapon(CreateWeaponInfo());
        }

        private IPlayerContext Player { get; }
        public event Action onWeaponStatusChanged;
        
        private readonly Queue<WeaponType> WeaponCycle = new Queue<WeaponType>(new[] {
            WeaponType.Calibrum,
            WeaponType.Severum,
            WeaponType.Gravitum,
            WeaponType.Infernum,
            WeaponType.Crescendum
        });
        
        WeaponInfo CreateWeaponInfo() {
            var type = WeaponCycle.Dequeue();
            return new WeaponInfo(type, GetNewMainWeapon);
        }

        void GetNewMainWeapon() {
            if (mainWeapon != null) WeaponCycle.Enqueue(mainWeapon.WeaponType);
            SetMainWeapon(CreateWeaponInfo());
        }

        void ChangeMainAndSubWeapon() {
            var originMain = GetMainWeapon();
            var originSub = GetSubWeapon();
            
            SetMainWeapon(originSub);
            SetSubWeapon(originMain);
        }

        private WeaponInfo mainWeapon;
        private WeaponInfo subWeapon;

        public WeaponInfo GetMainWeapon() {
            if (mainWeapon?.BulletCount == 0) {
                GetNewMainWeapon();
            }

            return mainWeapon;
        }

        void SetMainWeapon(WeaponInfo weaponInfo) {
            mainWeapon?.Strategy.Reset(Player);
            mainWeapon = weaponInfo;
            mainWeapon.Strategy.ApplyPassive(Player);
            onWeaponStatusChanged?.Invoke();
        }

        public WeaponInfo GetSubWeapon() {
            if (subWeapon?.BulletCount == 0) {
                GetNewMainWeapon();
            }

            return subWeapon;
        }

        void SetSubWeapon(WeaponInfo weaponInfo) {
            subWeapon = weaponInfo;
            onWeaponStatusChanged?.Invoke();
        }
        
        public void DoAttack(Transform origin, Transform target) {
            mainWeapon.Strategy.OnAttack(origin, target);
            mainWeapon.UseBullet(1);
            onWeaponStatusChanged?.Invoke();
        }

        public void DoSkillQ(Transform origin, Vector3 targetPoint) {
            mainWeapon.Strategy.OnSkill(origin, targetPoint);
            mainWeapon.UseBullet(UseBulletCountOnSkill);
            onWeaponStatusChanged?.Invoke();
        }
        
        public void Phase() {
            ChangeMainAndSubWeapon();
            onWeaponStatusChanged?.Invoke();
        }

        public WeaponType NextWeaponType() {
            return WeaponCycle.Peek();
        }
    }
}
