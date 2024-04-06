using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace FactoryMethod {
    public class UICanvasController2 : MonoBehaviour {
        [SerializeField, BoxGroup("Scene Info")] private Player02 player;
        [SerializeField, BoxGroup("Scene Info")] private DragonHandler dragonHandler;
        
        [SerializeField, BoxGroup("Battle Info")] private Text isBattle;
        [SerializeField, BoxGroup("Battle Info")] private Text playerHp;
        [SerializeField, BoxGroup("Battle Info")] private Text playerDamage;
        [SerializeField, BoxGroup("Battle Info")] private Text dragonType;
        [SerializeField, BoxGroup("Battle Info")] private Text dragonHp;
        [SerializeField, BoxGroup("Battle Info")] private Text dragonDamage;
        [SerializeField, BoxGroup("Battle Info")] private Text dragonAttackSpeed;

        private void Update() {
            if (player != null) {
                playerHp.text = $"Player HP: {player.GetHp():0}";
                playerDamage.text = $"Player DPS: {player.damagePerSecond}";
                isBattle.text = player.isBattle ? "전투 중" : string.Empty;
            }

            if (dragonHandler != null) {
                var dragon = dragonHandler.dragon;
                var isSpawned = dragon != null;
                dragonType.text = isSpawned ? $"{(DragonType) dragon.Type} Dragon" : "용이 소환되어있지 않습니다.";
                dragonHp.text = isSpawned ? $"용 Hp: {dragon.Hp:0}" : string.Empty;
                dragonDamage.text = isSpawned ? $"용 공격력 : {dragon.AttackDamage:0} + 대상 현재 체력의 7%" : string.Empty;
                dragonAttackSpeed.text = isSpawned ? $"용 공격속도: {dragon.AttackSpeed:0.00}" : string.Empty;
            }
        }
    }
}
