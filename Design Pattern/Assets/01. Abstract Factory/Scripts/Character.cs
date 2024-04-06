using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractFactory {
    public class Character {
        public Character(IChampionFactory factory) {
            ChampionIndex = factory.GetChampionIndex;
            OriginStat = factory.GetOriginStat;
            SkillQ = factory.GetSkillQ;
            SkillW = factory.GetSkillW;
            SkillE = factory.GetSkillE;
            
            Debug.Log($"<color=lime>챔피언 생성 ({ChampionIndex}) / [스킬셋] Q: {SkillQ.Name} / W: {SkillW.Name} / E: {SkillE.Name}</color>\n" +
                      $"<color=lime>[능력치] AD: {OriginStat.attackDamage} / HP: {OriginStat.health} / MP: {OriginStat.cost}</color>");
        }
        public virtual ChampionIndex ChampionIndex { get; }
        public virtual CharacterStat OriginStat { get; }
        public virtual CharacterStat AdditionalStat { get; } = CharacterStat.AllZero;

        private QSkillHandler skillQ;
        private WSkillHandler skillW;
        private ESkillHandler skillE;

        public virtual QSkillHandler SkillQ {
            get => skillQ;
            protected set => skillQ = value.SetCharacter(this) as QSkillHandler;
        }
        public virtual WSkillHandler SkillW {
            get => skillW;
            protected set => skillW = value.SetCharacter(this) as WSkillHandler;
        }
        public virtual ESkillHandler SkillE {
            get => skillE;
            protected set => skillE = value.SetCharacter(this) as ESkillHandler;
        }

        protected virtual void SetSkillQ(QSkillHandler skill) {
            skill.SetCharacter(this);
            SkillQ = skill;
        }
        protected virtual void SetSkillW(WSkillHandler skill) {
            skill.SetCharacter(this);
            SkillW = skill;
        }
        protected virtual void SetSkillE(ESkillHandler skill) {
            skill.SetCharacter(this);
            SkillE = skill;
        }

        public virtual float GetAbilityReductionPercent() {
            float totalHaste = OriginStat.abilityHaste + AdditionalStat.abilityHaste;
            float per = 100f / (100f + totalHaste);
            return per;
        }

        public virtual float TotalAttackDamage => OriginStat.attackDamage + AdditionalStat.attackDamage;

        public virtual float TotalAbilityPower => OriginStat.abilityPower + AdditionalStat.abilityPower;

        public List<SkillBase> GetSkillList => new List<SkillBase>() {SkillQ, SkillW, SkillE};
    }
}
