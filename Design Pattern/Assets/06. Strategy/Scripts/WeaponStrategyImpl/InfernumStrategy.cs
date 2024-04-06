using UnityEngine;

namespace Strategy {
    public class InfernumStrategy : IWeaponStrategy {
        public WeaponType WeaponType => WeaponType.Infernum;

        public void ApplyPassive(IPlayerContext player) {
            Debug.Log("<color=cyan>무기를 화염포(Infernum)로 교체합니다.</color>");
            Debug.Log("<color=cyan>적중 대상 뒤의 적들에게 광역 피해를 입힙니다.</color>");
        }
        
        public void Reset(IPlayerContext player) { }

        public void OnAttack(Transform origin, Transform target) {
            Debug.Log("<color=cyan>화염포 기본 공격</color>");
            
            // 08. Template Method 예제 실행을 위해 추가됨
            new TemplateMethod.InfernumProjectileView().Run(origin, target);
        }

        public void OnSkill(Transform origin, Vector3 targetPoint) {
            Debug.Log("<color=cyan>화염포 Q - 황혼파</color>");
        }
    }
}
