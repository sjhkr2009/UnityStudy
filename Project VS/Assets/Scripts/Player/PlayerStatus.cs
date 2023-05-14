using System;
using UnityEngine;

[Serializable]
public class PlayerStatus {
    public GameObject GameObject { get; }
    public CharacterData CharacterData { get; }
    public ItemController ItemController { get; }

    public float Speed {
        get {
            var speed = CharacterData.speed;
            foreach (var item in ItemController.Items) {
                if (item is ISpeedModifier speedModifier) {
                    speed = speedModifier.ModifySpeed(speed);
                }
            }
            return speed;
        }
    }
    
    public float AttackRange {
        get {
            var range = CharacterData.attackRange;
            foreach (var item in ItemController.Items) {
                if (item is IAttackRangeModifier modifier) {
                    range = modifier.ModifyAttackRange(range);
                }
            }
            return range;
        }
    }
    public float Acceleration { get; set; } = 20f;
    public Vector2 InputVector { get; set; } = Vector2.zero;
    public Vector2 DeltaMove { get; set; } = Vector2.zero;
    public bool IsDead { get; set; } = false;

    public PlayerStatus(GameObject playerObject, ItemController itemController) {
        GameObject = playerObject;
        CharacterData = new CharacterData();
        ItemController = itemController;
    }
}

// TODO: 캐릭터가 추가되면 아이템처럼 데이터를 분리할 것
public class CharacterData {
    public float speed = 3f;
    public float acceleration = 20f;
    public float attackPower = 1f;
    public float attackRange = 1f;
}
