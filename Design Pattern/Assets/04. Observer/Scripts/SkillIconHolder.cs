using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Observer {
    public class SkillIconHolder : TimeObserver {
        [SerializeField] private Button button;
        [SerializeField] private Image fillImage;
        [BoxGroup("Skill info"), SerializeField, Range(0f, 20f)] private float cooldown = 5f;
        [BoxGroup("Skill info"), SerializeField] private string skillName;

        [BoxGroup("Status"), SerializeField, ReadOnly] private bool isCoolTime = false;
        [BoxGroup("Status"), SerializeField, ReadOnly] private float remainCooltime = 0f;
        
        protected override void OnEnable() {
            base.OnEnable();
            button.onClick.AddListener(OnClick);
        }

        public override void OnUpdate(float deltaTime) {
            if (isCoolTime) {
                remainCooltime -= deltaTime;
                
                if (remainCooltime <= 0f) {
                    isCoolTime = false;
                    remainCooltime = 0f;
                    fillImage.fillAmount = 0f;
                } else {
                    fillImage.fillAmount = remainCooltime / cooldown;
                }
            }
        }

        void OnClick() {
            if (isCoolTime) {
                Debug.Log($"<color=yellow>쿨타임입니다. ({remainCooltime:0.0}/{cooldown:0.0}s)</color>");
                return;
            }
            
            Debug.Log($"<color=lime>스킬 사용: {skillName} (쿨타임 {cooldown:0.0}초)</color>");
            isCoolTime = true;
            remainCooltime = cooldown;
            fillImage.fillAmount = 1f;
        }
    }
}
