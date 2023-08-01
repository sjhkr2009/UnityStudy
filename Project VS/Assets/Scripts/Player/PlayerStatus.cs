using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerStatus {
    public GameObject GameObject { get; }
    private CharacterData _characterData { get; }
    public CharacterData ClonedOriginStat => _characterData.Clone();
    public AbilityController AbilityController { get; }
    
    public float MaxHp { get; private set; }
    private float _hp;
    public float Hp {
        get => _hp.Clamp(0, MaxHp);
        private set => _hp = value.Clamp(0, MaxHp);
    }

    public float Speed { get; private set; }
    public float AttackRange { get; private set; }
    public float AttackPower { get; private set; }
    
    public float Acceleration { get; set; } = 20f;
    public Vector2 InputVector { get; set; } = Vector2.zero;
    public Vector2 DeltaMove { get; set; } = Vector2.zero;
    public Vector2 ShowDirection { get; set; } = Vector2.right;
    public bool IsDead { get; set; } = false;
    public int LockHitCount { get; set; } = 0;

    public PlayerStatus(GameObject go, PlayerController.ComponentHolder componentHolder) {
        GameObject = go;
        
        _characterData = new CharacterData();
        AbilityController = componentHolder.abilityController;
        
        Initialize();
    }

    void Initialize() {
        UpdateStat();

        OnAfterInitialize();
    }

    // TODO: 캐릭터에 따라 초기에 몇 가지 업그레이드를 가지고 시작하는 경우 이곳에서 처리. 많아질 경우 PlayerStatus를 추상화해서 클래스 단위로 분리할 수 있음. 
    void OnAfterInitialize() {
        Hp = MaxHp;
        LockHitCount = 0;
    }
    public void UpdateStat() {
        var clonedData = ClonedOriginStat;
        foreach (var ability in AbilityController.AllAbilities) {
            if (ability is IPlayerStatModifier modifier) {
                modifier.ApplyModify(clonedData);
            }
        }
        MaxHp = clonedData.maxHp;
        if (Hp > MaxHp) Hp = MaxHp;
        Speed = clonedData.speed;
        AttackPower = clonedData.attackPower;
        AttackRange = clonedData.attackRange;
    }

    public bool IsHittable() {
        if (IsDead) return false;
        if (LockHitCount > 0) return false;

        return true;
    }

    public void Hit(float damage) {
        Hp -= damage;
        LockHitCount++;
        UniTask.Delay(200).ContinueWith(() => LockHitCount--).Forget();
    }
}

// TODO: 캐릭터가 추가되면 아이템처럼 데이터를 분리할 것
public class CharacterData {
    public float maxHp = 10f;
    public float speed = 3f;
    public float acceleration = 20f;
    public float attackPower = 1f;
    public float attackRange = 1f;
    public float skillHaste = 1f;

    public CharacterData Clone() {
        return new CharacterData() {
            maxHp = maxHp,
            speed = speed,
            acceleration = acceleration,
            attackPower = attackPower,
            attackRange = attackRange,
            skillHaste = skillHaste
        };
    }
}
