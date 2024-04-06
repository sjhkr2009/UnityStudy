using UnityEngine;

namespace Strategy {
    public class CrescendumStrategy : IWeaponStrategy {
        public WeaponType WeaponType => WeaponType.Crescendum;

        public void ApplyPassive(IPlayerContext player) {
            Debug.Log("<color=white>무기를 반월검(Crescendum)으로 교체합니다.</color>");
            Debug.Log("<color=white>근거리일수록 빠르게 피해를 입힐 수 있습니다.</color>");
        }

        public void Reset(IPlayerContext player) { }

        public void OnAttack(Transform origin, Transform target) {
            Debug.Log("<color=white>반월검 기본 공격</color>");
            
            // 08. Template Method 예제 실행을 위해 추가됨
            new TemplateMethod.CrescendumProjectileView().Run(origin, target);
        }

        public void OnSkill(Transform origin, Vector3 targetPoint) {
            Debug.Log("<color=white>반월검 Q - 파수탑</color>");
        }
    }
}
