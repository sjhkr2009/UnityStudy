using System.Collections.Generic;
using UnityEngine;

namespace AbstractFactory {
    public class Player01 : MonoBehaviour {
        [SerializeField] private ChampionIndex selectedChampion;
        [SerializeField, Range(1, 5)] private int skillLevel = 1;
        [SerializeField, Range(0, 300)] private int attackDamage;
        [SerializeField, Range(0, 500)] private int abilityPower;
        [SerializeField, Range(0, 120)] private int abilityHaste;

        private ChampionIndex currentChampion;

        public Character myCharacter;
        private List<SkillBase> skills;
        
        private void Start() {
#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = gameObject;
#endif
            CreateCharacter();
            UpdateStat();
        }

        void CreateCharacter() {
            var factory = CharacterFactory.CreateFactory(selectedChampion);
            // 1. 팩토리를 인자로 캐릭터의 생성자를 직접 호출 -> 추상 객체의 생성자를 노출해야 하지만, 팩토리를 통해서만 생성하도록 강제할 수 있다.
            myCharacter = new Character(factory);
            // 2. 팩토리의 메서드를 통해 캐릭터를 생성 -> 클라이언트가 추상 객체의 생성자를 호출하지 않아도 되지만, 세부 객체의 팩토리가 필요할 때는?
            myCharacter = factory.Create();

            skills = myCharacter.GetSkillList;
            currentChampion = selectedChampion;
        }

        void UpdateStat() {
            myCharacter.AdditionalStat.attackDamage = attackDamage;
            myCharacter.AdditionalStat.abilityPower = abilityPower;
            myCharacter.AdditionalStat.abilityHaste = abilityHaste;
        }

        private void Update() {
            UpdateStat();
            foreach (var skill in skills) {
                skill.CurrentLevel = skillLevel;
                skill.Update(Time.deltaTime);
            }

            if (selectedChampion != currentChampion) {
                var prev = currentChampion;
                CreateCharacter();
                attackDamage = 0;
                abilityPower = 0;
                abilityHaste = 0;
                UpdateStat();
                Debug.Log($"<color=cyan>챔피언 변경: {prev} -> {currentChampion}</color>");
            }
        }
    }
}
