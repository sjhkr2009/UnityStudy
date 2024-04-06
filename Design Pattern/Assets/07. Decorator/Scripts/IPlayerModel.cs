using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Decorator {
    [Flags]
    public enum BehaviorFlag {
        None = 1,
        CanMove = 1 << 1,
        CanAttack = 1 << 2,
        CanSkill = 1 << 3,
        Normal = CanMove | CanAttack | CanSkill,

        CcImmune = 1 << 5,
        DamageImmune = 1 << 6,
        Invulnerable = CcImmune | DamageImmune
    }
    public class PlayerStat {
        public float hp;
        public float attackPower;
        public float defensePower;

        public PlayerStat Clone() {
            return new PlayerStat() {
                hp = this.hp,
                attackPower = this.attackPower,
                defensePower = this.defensePower
            };
        }
    }
    
    public interface IPlayerModel {
        PlayerStat GetStat();
        BehaviorFlag GetBehaviorStatus();
    }
}
