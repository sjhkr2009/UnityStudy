using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AbstractFactory {
    public class UICanvasController1 : MonoBehaviour {
        [SerializeField] private Player01 reference;
        [SerializeField] private Text text;
        [SerializeField] private Button skillQ;
        [SerializeField] private Button skillW;
        [SerializeField] private Button skillE;
        
        private Image cooldownQ;
        private Image cooldownW;
        private Image cooldownE;

        private void Start() {
            skillQ.onClick.AddListener(UseSkillQ);
            skillW.onClick.AddListener(UseSkillW);
            skillE.onClick.AddListener(UseSkillE);

            cooldownQ = skillQ.transform.Find("FillImage").GetComponent<Image>();
            cooldownW = skillW.transform.Find("FillImage").GetComponent<Image>();
            cooldownE = skillE.transform.Find("FillImage").GetComponent<Image>();
        }

        void Update() {
            if (!reference || reference.myCharacter == null) return;

            text.text = GetStatusMessage(reference.myCharacter);
            UpdateFillImage();
        }

        string GetStatusMessage(Character character) {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine("Status");
            sb.AppendLine();
            
            sb.AppendLine($"Name: {character.ChampionIndex}");
            sb.AppendLine($"AD: {character.TotalAttackDamage} ({character.OriginStat.attackDamage} + {character.AdditionalStat.attackDamage})");
            sb.AppendLine($"AP: {character.TotalAbilityPower}");
            sb.AppendLine($"HP: {character.OriginStat.health}");
            sb.AppendLine($"MP: {character.OriginStat.cost}");
            sb.AppendLine($"스킬 가속: {character.AdditionalStat.abilityHaste} (= 재사용 대기시간 {((1f - character.GetAbilityReductionPercent()) * 100f):0.0}% 감소)");
            sb.AppendLine();
            
            foreach (var skill in character.GetSkillList) {
                sb.AppendLine($"[{skill.Name} | Lv.{skill.CurrentLevel}] " + (skill.CanUseNow() ? 
                    "(사용 가능)" : 
                    $"(남은 쿨타임: {skill.RemainCoolDown:0.0} / {(skill.OriginCoolDown * character.GetAbilityReductionPercent()):0.0}초)"));
            }

            return sb.ToString();
        }

        void UseSkillQ() {
            if (!reference || reference.myCharacter == null) return;
            
            var character = reference.myCharacter;
            if (character.SkillQ.CanUseNow()) character.SkillQ.Activate();
        }
        
        void UseSkillW() {
            if (!reference || reference.myCharacter == null) return;
            
            var character = reference.myCharacter;
            if (character.SkillW.CanUseNow()) character.SkillW.Activate();
        }
        
        void UseSkillE() {
            if (!reference || reference.myCharacter == null) return;
            
            var character = reference.myCharacter;
            if (character.SkillE.CanUseNow()) character.SkillE.Activate();
        }

        void UpdateFillImage() {
            if (!reference || reference.myCharacter == null) return;
            
            var character = reference.myCharacter;
            
            cooldownQ.gameObject.SetActive(!character.SkillQ.CanUseNow());
            if (cooldownQ.gameObject.activeSelf)
                cooldownQ.fillAmount = character.SkillQ.RemainCoolDown / (character.SkillQ.OriginCoolDown * character.GetAbilityReductionPercent());
            
            cooldownW.gameObject.SetActive(!character.SkillW.CanUseNow());
            if (cooldownW.gameObject.activeSelf)
                cooldownW.fillAmount = character.SkillW.RemainCoolDown / (character.SkillW.OriginCoolDown * character.GetAbilityReductionPercent());
            
            cooldownE.gameObject.SetActive(!character.SkillE.CanUseNow());
            if (cooldownE.gameObject.activeSelf)
                cooldownE.fillAmount = character.SkillE.RemainCoolDown / (character.SkillE.OriginCoolDown * character.GetAbilityReductionPercent());
        }
    }
}
