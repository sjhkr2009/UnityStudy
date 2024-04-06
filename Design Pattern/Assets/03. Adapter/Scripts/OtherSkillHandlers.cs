using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adapter {
    public class UncountableArrow : ISkillHandler<Bow> {
        public void Activate(Bow weapon) {
            Debug.Log($"<color=lime>언카운터블 애로우 / 데미지: {weapon.AttackPower * weapon.Coefficient * 3f * 5:0} (공격력 {weapon.AttackPower}, 무기상수 {weapon.Coefficient}, 300% x 5회), 사정거리: {weapon.Range})</color>");
        }
    }
    public class RaisingBlow : ISkillHandler<OneHandedSword> {
        public void Activate(OneHandedSword weapon) {
            Debug.Log($"<color=yellow>레이징 블로우 / 데미지: {weapon.AttackPower * weapon.Coefficient * 2f * 7:0} (공격력 {weapon.AttackPower}, 무기상수 {weapon.Coefficient}, 200% x 7회), 사정거리: {weapon.Range})</color>");
        }
    }
    public class QuadrupleThrow : ISkillHandler<Claw> {
        public void Activate(Claw weapon) {
            Debug.Log($"<color=magenta>쿼드러플 스로우 / 데미지: {weapon.AttackPower * weapon.Coefficient * 4f * 4:0} (공격력 {weapon.AttackPower}, 무기상수 {weapon.Coefficient}, 400% x 4회), 사정거리: {weapon.Range})</color>");
        }
    }
}
