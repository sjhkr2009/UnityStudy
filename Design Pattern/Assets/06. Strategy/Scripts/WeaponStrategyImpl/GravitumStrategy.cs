using UnityEngine;

namespace Strategy {
    public class GravitumStrategy : IWeaponStrategy {
        public WeaponType WeaponType => WeaponType.Gravitum;

        public void ApplyPassive(IPlayerContext player) {
            Debug.Log("<color=magenta>무기를 중력포(Gravitum)로 교체합니다.</color>");
            Debug.Log("<color=magenta>적중당한 대상을 30% 둔화시킵니다.</color>");
        }

        public void Reset(IPlayerContext player) { }

        public void OnAttack(Transform origin, Transform target) {
            Debug.Log("<color=magenta>중력포 기본 공격</color>");
            
            // 08. Template Method 예제 실행을 위해 추가됨
            new TemplateMethod.GravitumProjectileView().Run(origin, target);
        }

        public void OnSkill(Transform origin, Vector3 targetPoint) {
            Debug.Log("<color=magenta>중력포 Q - 월식</color>");
        }
    }
}
