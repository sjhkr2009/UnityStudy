using UnityEngine;

namespace Strategy {
    public class CalibrumStrategy : IWeaponStrategy {
        public WeaponType WeaponType => WeaponType.Calibrum;

        public void ApplyPassive(IPlayerContext player) {
            Debug.Log("<color=lime>무기를 만월총(Calibrum)으로 교체합니다.</color>");
            Debug.Log("<color=lime>기본 공격 사거리가 100 증가합니다.</color>");
            player.Range += 100;
        }

        public void Reset(IPlayerContext player) {
            player.Range -= 100;
        }

        public void OnAttack(Transform origin, Transform target) {
            Debug.Log("<color=lime>만월총 기본 공격</color>");
            
            // 08. Template Method 예제 실행을 위해 추가됨
            new TemplateMethod.CalibrumProjectileView().Run(origin, target);
        }

        public void OnSkill(Transform origin, Vector3 targetPoint) {
            Debug.Log("<color=lime>만월총 Q - 달빛탄</color>");
        }
    }
}
