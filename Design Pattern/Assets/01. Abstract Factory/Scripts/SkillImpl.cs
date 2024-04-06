using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractFactory {
    public class MysticShot : QSkillHandler {
        public override string Name { get; protected set; } = "신비한 화살";

        public override float OriginCoolDown => 6f - (CurrentLevel * 0.5f);

        public float GetDamage => -5f + (25f * CurrentLevel)
                                      + Character.TotalAttackDamage * 1.3f
                                      + Character.TotalAbilityPower * 0.15f;

        public override void Activate() {
            base.Activate();
            bool isHit = Random.value > 0.2f;
            if (isHit) {
                RemainCoolDown -= 1.5f;
                Character.SkillW.RemainCoolDown -= 1.5f;
                Character.SkillE.RemainCoolDown -= 1.5f;
                Debug.Log($"처음 맞은 적에게 {GetDamage:0}의 피해를 입혔습니다. 쿨타임이 1.5초 감소합니다. " +
                          $"(남은 쿨타임: Q {RemainCoolDown:0.0}초 / W {Character.SkillW.RemainCoolDown:0.0}초 / E {Character.SkillE.RemainCoolDown}초)");
            }
            else {
                Debug.Log("스킬이 적중하지 못했습니다.");
            }
        }
    }

    public class EssenceFlux : WSkillHandler {

        public override string Name { get; protected set; } = "정수의 흐름";

        public override float OriginCoolDown => 12f;

        public float GetDamage => 25f + (55f * CurrentLevel)
                                      + Character.AdditionalStat.attackDamage * 0.6f
                                      + Character.TotalAbilityPower * (0.65f + CurrentLevel * 0.05f);

        public float RecoverCost => 60;

        public override void Activate() {
            base.Activate();
            bool isHit = Random.value > 0.25f;
            if (isHit) {
                RemainCoolDown -= 1.5f;
                Debug.Log($"적에게 표식을 남깁니다. 표식이 발동되면 {GetDamage:0}의 피해를 입히며 {RecoverCost:0}의 마나를 돌려받습니다.");
            }
            else {
                Debug.Log("스킬이 적중하지 못했습니다.");
            }
        }
    }

    public class ArcaneShift : ESkillHandler {
        public override string Name { get; protected set; } = "비전 이동";
        public override float OriginCoolDown => 31 - (CurrentLevel * 3f);

        public float GetDamage => 30f + (50f * CurrentLevel)
                                      + Character.AdditionalStat.attackDamage * 0.5f
                                      + Character.TotalAbilityPower * 0.75f;

        public float Range => 475;

        public override void Activate() {
            base.Activate();
            Debug.Log($"{Range} 거리만큼 순간이동하며 가까운 적에게 {GetDamage:0}의 피해를 입힙니다.");
        }
    }
    
    public class Glitterlance : QSkillHandler {
        public override string Name { get; protected set; } = "반짝반짝 창";
        public override float OriginCoolDown => 7f;
        public float GetDamage => 35f + (35f * CurrentLevel)
                                      + Character.TotalAbilityPower * 0.5f;
        
        public override void Activate() {
            base.Activate();
            Debug.Log($"룰루와 픽스가 마법 화살을 발사하여 {GetDamage:0}의 피해를 입힙니다.");
            if (Random.value > 0.75f)
                Debug.Log($"두 개의 마법 화살을 발사하여 {(GetDamage * 0.25f):0.0}의 추가 피해를 입힙니다.");
        }
    }

    public class Whimsy : WSkillHandler {
        public override string Name { get; protected set; } = "변덕쟁이";

        public override float OriginCoolDown => 17f - (CurrentLevel * 1f);

        public float BuffDuration => 2.75f + (0.25f * CurrentLevel);
        public float BuffAttackSpeed => 22.5f + (2.5f * CurrentLevel);
        public float BuffMovementSpeed => 30f + (0.05f * Character.TotalAbilityPower);

        public float StatusAilmentDuration => 1f + (0.25f * CurrentLevel);

        public override void Activate() {
            base.Activate();
            if (Random.value > 0.5f)
                Debug.Log($"아군에게 시전하여 {BuffDuration}초간 공격 속도 {BuffAttackSpeed}%, 이동 속도 {BuffMovementSpeed}% 만큼 증가시킵니다.");
            else
                Debug.Log($"{StatusAilmentDuration}초간 적을 변이시킵니다.");
        }
    }

    public class HelpPix : ESkillHandler {
        public override string Name { get; protected set; } = "도와줘, 픽스!";

        public override float OriginCoolDown => 8f;

        public float GetDamage => 40f + (40f * CurrentLevel)
                                      + Character.TotalAbilityPower * 0.4f;
        public float GetShieldCapacity => 40f + (40f * CurrentLevel)
                                      + Character.TotalAbilityPower * 0.6f;

        public override void Activate() {
            base.Activate();
            if (Random.value > 0.5f)
                Debug.Log($"아군에게 시전하여 2.5초간 {GetShieldCapacity}의 실드를 부여합니다.");
            else
                Debug.Log($"적에게 시전하여 {GetDamage}의 피해를 입힙니다.");
        }
    }
}
